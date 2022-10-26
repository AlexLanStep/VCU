
using System.Diagnostics.Metrics;

namespace ContextLabCar.Core.Strategies;

public interface IStrategiesBasa
{
  string NameStrateg { get; set; }
  IConnectLabCar IConLabCar { get; set; }
  void RunInit(string pathdir = "");
  void RunTest();
  dynamic? GetMeasurement(string name);
  void SetDan(string name, dynamic dsn);

}

public class StrategiesBasa : StrategyDanJson, IStrategiesBasa
{
  #region Dan
  protected ConcurrentDictionary<string, ISignal> dMeasurement = new();
  protected ConcurrentDictionary<string, ISignal> dParams = new();
  public string NameStrateg { get; set; } = "";
  public int MaxWaitRez { get; set; } = 10;
  #endregion

  public StrategiesBasa(IConnectLabCar iConLabCar)
  {
    IConLabCar = iConLabCar;
  }

  public IConnectLabCar IConLabCar { get; set; }

  public void RunInit(string pathdir = "")
  {
    InitializationJson(pathdir);
    if (!(DConfig.TryGetValue("Workspace", out string vwork) && DConfig.TryGetValue("Experiment", out string vexpe)))
      throw new MyException(" Error in json path Workspace or Experiment ", -5);

    DConfig.Add("StDir", pathdir);
    NameStrateg = DstParams["Name"];
    MaxWaitRez = (int) (DstParams.TryGetValue("maxwait", out dynamic valRez) ? valRez : 10);
    IStrategiesBasaPRT = (IStrategiesBasa)this;
    return;
    Console.WriteLine("Подключение к LabCar");
    IConLabCar.Initialization(vwork, vexpe);
    IConLabCar.Connect();
    Console.WriteLine($"Инициализация параметров для стратегии {NameStrateg}:");
    Console.WriteLine("  - Task");
    inicialloadTask();

    Console.WriteLine("  - Params");
    inicialParamsDic();

    Console.WriteLine("  - Файлы с калибровками");
    if(DstSetStart.TryGetValue("loadfile", out dynamic valFiles))
    {

      foreach (var vparam in (List<string>) valFiles)
      {
        if (DCalibrationParams.ContainsKey(vparam))
          inicialCalibrat(vparam);
        //          throw new MyException($"Error in {itFile} StrategiesBasa.RunInit for inicialCalibrat({pathfull}) ", -2);

      }
    }
    else
      Console.WriteLine(" Калибровок нет. ");

  }

  protected void inicialloadTask() 
  { 

    foreach(var (key, val) in DTask) 
    {
//      ISignal measurement = IConLabCar.SignalSources.CreateMeasurement("TEST/Control_Signal/Value", "Acquisition");
      ISignal measurement = IConLabCar.SignalSources.CreateMeasurement(val.PathTask, val.NameInLabCar);
      dMeasurement.AddOrUpdate(key, measurement, (_, _) => measurement);
    }

  }
  private string testFile(string fullPath)
  {
        string path = "";
//    if (!DPath.TryGetValue(path, out string fullPath))
//      throw new MyException($"No link to file {path}, in inicialCalibrat", -1);

    if (!File.Exists(fullPath))
      throw new MyException($"No file {path}, in inicialCalibrat", -1);

    return fullPath;
  }
  protected void inicialCalibrat(string file)
  {
    string fullPath = testFile(DCalibrationParams[file].PathFiles);
    try
    {
      var _vCollebrator = IConLabCar.Experiment.CalibrationController;
      IConLabCar.Experiment.CalibrationController.LoadParameters(fullPath);
    }
    catch (Exception)
    { Console.WriteLine($" - parameters are already set {fullPath}  "); }

    try
    { IConLabCar.Experiment.AddFile(fullPath); }
    catch (Exception)
    { Console.WriteLine($" the file is already written {fullPath} "); }
  }
  protected void inicialParams(string name, string path)
  {
    ISignal isig = IConLabCar.SignalSources.CreateParameter(path);
    dParams.AddOrUpdate(name, isig, (_, _) => isig);
  }
  protected void inicialParamsDic()
  {
    foreach (var (key, val) in DParameterNew)
    {
      ISignal isig = IConLabCar.SignalSources.CreateParameter(val.Signal);
      dParams.AddOrUpdate(key, isig, (_, _) => isig);
    }
  }
  protected void activParamsCalibrat(string file)
  {
    string fullPath = testFile(file);
    try
    {
      IConLabCar.Experiment.ActivateFile(fullPath, true);
    }
    catch (Exception)
    {
      Console.WriteLine($" Problem with activating options {fullPath}");
    }
  }
  public dynamic? GetMeasurement(string name)
  {
    dynamic? rezult =null;
    if(dMeasurement.ContainsKey(name))
    {
      IScalarValue valueObject = (IScalarValue) dMeasurement[name].GetValueObject();
      rezult = valueObject.GetValue();
    }
    else
      Console.WriteLine($" - Error in reading measurement {name}  ");
    return rezult;
  }
  public void SetDan(string name, dynamic dsn)
  {
        //string s = DParameter[name].Signal;
        //ISignal parameter = IConLabCar.SignalSources.CreateParameter(s);
        //IScalarValue valueObject = (IScalarValue)parameter.GetValueObject();
        //valueObject.SetValue(2.000);
        //parameter.SetValueObject(valueObject);
        //int k = 1;
        var ValueObject = (IScalarValue)dParams[name].GetValueObject();
        ValueObject.SetValue(dsn);
        dParams[name].SetValueObject(ValueObject);

        //if (dParams.ContainsKey(name))
        //{
        //    var xx = (ISignal)dParams[name];
        //    xx.SetValueObject(ValueObject);
        //    dParams[name] = xx;
        //}
        //else
        //{
        //    //        dParams.TryAdd(name);
        //    int j = 1;
        //}
    }
  private void factivCalibr()
  {
    Console.WriteLine(" Активируем калибровки ");
    if (DstSetStart.TryGetValue("activfile", out dynamic valFiles))
    {
      foreach (var itFile in (List<string>)valFiles)
      {
        if(DCalibrationParams.TryGetValue(itFile, out var dan))
        {
          activParamsCalibrat(dan.PathFiles);
        }
//        if (!DPath.TryGetValue(itFile, out string pathfull))
//          throw new MyException($"Error in {itFile} StrategiesBasa.RunInit for inicialCalibrat({pathfull}) ", -2);
      }
    }
    else
      Console.WriteLine(" Нет указанных для активации калибровок. ");

  }
  public virtual void RunTest()
  {
    Dictionary<string, dynamic> _rezul = new();
    void _getDanLabCar(StOneStep _oneStep)
    {
      if (_oneStep.GetPoints.Count == 0)
        return;

      Console.WriteLine(" _ Читаем переменные _");
      foreach (var (keyGet, valGet) in _oneStep.GetPoints)
      {
        var _xx0 = GetMeasurement(keyGet);
        _oneStep.AddGetPoints(keyGet, _xx0);
        if(_rezul.ContainsKey(keyGet))
            _rezul[keyGet]=_xx0;
        else
            _rezul.Add(keyGet,_xx0);
      }
    }
    void _setDanLabCar(StOneStep _oneStep)
    {
      if (_oneStep.SetPoints.Count == 0)
        return;

      Console.WriteLine(" _ Пишем переменные _");

      foreach (var (keySet, valSet) in _oneStep.SetPoints)
      {
        SetDan(keySet, valSet);
      }

    }
    bool _rezDanLabCar(StOneStep oneStep)
    {
      var rez = false;
      var dt0 = DateTime.Now;
      while ((!rez) && ((DateTime.Now - dt0).Seconds <= MaxWaitRez))
      {
        _getDanLabCar(oneStep);
        rez = oneStep.TestRez(_rezul);
        if(!rez) Thread.Sleep(1000);
      }
      return rez;
    }
    bool _ifDanLabCar(StOneStep oneStep)
    {
      var rez = false;
      var dt0 = DateTime.Now;
      while ((!rez) && ((DateTime.Now - dt0).Seconds <= MaxWaitRez))
      {
        _getDanLabCar(oneStep);
        rez = oneStep.TestIf(_rezul);
        if (!rez) Thread.Sleep(1000);
      }
      return rez;
    }
    bool _ifOrDanLabCar(StOneStep oneStep)
    {
      var rez = false;
      var dt0 = DateTime.Now;
      while ((!rez) && ((DateTime.Now - dt0).Seconds <= MaxWaitRez))
      {
        _getDanLabCar(oneStep);
        rez = oneStep.TestIfOr(_rezul);
        if (!rez) Thread.Sleep(1000);
      }
      return rez;
    }


    if (LsStOneStep.Count < 2)
      throw new MyException("Not a complete strategy StrategiesBasa.RunTest() ", -2);

    factivCalibr();

    int _numSten = 0;
    Console.WriteLine($"  -  Start TEST - {NameStrateg} ");
    foreach (var _oneStep in LsStOneStep)
    {
      Console.WriteLine($"  -  Step -> {_numSten} ");

      _getDanLabCar(_oneStep);
      _setDanLabCar(_oneStep);

      if (_oneStep.LIfOr.Count > 0)
      {
        if (!_ifOrDanLabCar(_oneStep))
        {
          Console.WriteLine("-- ==>  Test failed (ifOr не прошел) ((((( ----");
          break;
        }
      }

      if (_oneStep.LIf.Count > 0)
      {
        if (!_ifDanLabCar(_oneStep))
        {
          Console.WriteLine("-- ==>  Test failed (if не прошел) ((((( ----");
          break;
        }
      }

      if (_oneStep.LResult.Count > 0)
      {
        if (_rezDanLabCar(_oneStep))
            Console.WriteLine("-- !!!!  Test went well !!!!");
        else
            Console.WriteLine("-- ==>  Test failed ((((( ----");
      }
      _numSten += 1;
    }


  }

}

