﻿
using DynamicData;
using System.Xml.Linq;

namespace ContextLabCar.Core.Strategies;

public interface IStrategiesBasa
{
  string NameStrateg { get; set; }
  IConnectLabCar IConLabCar { get; set; }
  void RunInit(string pathdir = "");
  void RunTest();
}

public class StrategiesBasa : StrategDanJson, IStrategiesBasa
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
    InicialJson(pathdir);
    if (!(DPath.TryGetValue("Workspace", out string vwork) && DPath.TryGetValue("Experiment", out string vexpe)))
      throw new MyException(" Error in json path Workspace or Experiment ", -5);

    DPath.Add("StDir", pathdir);
    NameStrateg = DSTParams["Name"];
    MaxWaitRez = (int) (DSTParams.TryGetValue("maxwait", out dynamic valRez) ? valRez : 10);

    return;
    Console.WriteLine("Подключение к LabCar");
    IConLabCar.Inicial(vwork, vexpe);
    Console.WriteLine($"Инициализация параметров для стратегии {NameStrateg}:");
    Console.WriteLine("  - Task");
    inicialloadTask();

    Console.WriteLine("  - Params");
    inicialParamsDic();

    Console.WriteLine("  - Файлы с калибровками");
    if(DSTsetStart.TryGetValue("loadfile", out dynamic valFiles))
    {
      foreach (var itFile in (List<string>) valFiles)
      {
        if (!DPath.TryGetValue(itFile, out string pathfull))
          throw new MyException($"Error in {itFile} StrategiesBasa.RunInit for inicialCalibrat({pathfull}) ", -2);
        inicialCalibrat(pathfull);
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
      ISignal measurement = IConLabCar.SignalSources.CreateMeasurement(val.TTask, val.NameLabCar);
      dMeasurement.AddOrUpdate(val.Signal, measurement, (_, _) => measurement);
    }
  }
  private string testFile(string path)
  {
    if (!DPath.TryGetValue(path, out string fullPath))
      throw new MyException($"No link to file {path}, in inicialCalibrat", -1);

    if (!File.Exists(fullPath))
      throw new MyException($"No file {path}, in inicialCalibrat", -1);

    return fullPath;
  }
  protected void inicialCalibrat(string file)
  {
    string fullPath = testFile(file);
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
    foreach (var (key, val) in DParameter)
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
  protected dynamic? getMeasurement(string name)
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
  protected void setDan(string name, dynamic dsn)
  {
    var ValueObject = (IScalarValue)dParams[name].GetValueObject();
    ValueObject.SetValue(dsn);
    dParams[name].SetValueObject(ValueObject);
  }
  private void factivCalibr()
  {
    Console.WriteLine(" Активируем калибровки ");
    if (DSTsetStart.TryGetValue("activfile", out dynamic valFiles))
    {
      foreach (var itFile in (List<string>)valFiles)
      {
        if (!DPath.TryGetValue(itFile, out string pathfull))
          throw new MyException($"Error in {itFile} StrategiesBasa.RunInit for inicialCalibrat({pathfull}) ", -2);

        activParamsCalibrat(pathfull);
      }
    }
    else
      Console.WriteLine(" Нет указаний для активации калибровок нет. ");

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
        var _xx0 = getMeasurement(keyGet);
        _oneStep.AddGetPoints(keyGet, _xx0);
        _rezul.Add(keyGet, _xx0);
      }
    }
    void _setDanLabCar(StOneStep _oneStep)
    {
      if (_oneStep.SetPoints.Count == 0)
        return;

      Console.WriteLine(" _ Пишем переменные _");

      foreach (var (keySet, valSet) in _oneStep.SetPoints)
      {
        setDan(keySet, valSet);
      }

    }
    bool _retDanLabCar(StOneStep _oneStep)
    {
      bool _rez = false;
      DateTime _dt0 = DateTime.Now;
      int _sec = 0;
      while ((!_rez) && ((DateTime.Now - _dt0).Seconds <= MaxWaitRez ))
      {
        _getDanLabCar(_oneStep);
        _rez = _oneStep.TestDan(_rezul);
      }
      return _rez;
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
      

      if ((_oneStep.LRezult.Count > 0) && _retDanLabCar(_oneStep))
        Console.WriteLine("-- !!!!  Test went well !!!!");
      else 
        Console.WriteLine("-- ==>  Test failed ((((( ----"); 

      _numSten += 1;
    }


  }

}

/*
AddDictRecVal(string key, dynamic value)

public Dictionary<string, dynamic> GetPoints { get; set; } = new ();
  public Dictionary<string, dynamic> SetPoints { get; set; } = new(); 
  public List<string> LRezult { get; set; } = new();


   public Dictionary<string, Parameter> DParameter; // = new();
  public Dictionary<string, DanOutput> DDanOutput; // = new();
  public Dictionary<string, Dictionary<string, Calibrat>> DCalibrat; //= new();
  public Dictionary<string, string> DPath; // = new();
  public Dictionary<string, LTask> DTask; // = new();
  public Dictionary<string, dynamic> DSTParams; // = new();
  public Dictionary<string, dynamic> DSTsetStart; // = new();
  public List<StOneStep> LsStOneStep; // = new List<StOneStep>();

 
 */