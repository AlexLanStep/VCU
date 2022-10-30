﻿
using ContextLabCar.Core.Interface;
using ETAS.EE.Scripting;
using System.Security.Cryptography;

namespace ContextLabCar.Core.Strategies;

public interface IStrategiesBasa
{
  string NameStrateg { get; set; }
  IConnectLabCar IConLabCar { get; set; }
  void RunInit(string pathdir = "");
  void RunTest();
  bool IsRezulta { get; set; }

}

public class StrategiesBasa : StrategyDanJson, IStrategiesBasa
{
  #region Dan
  protected ConcurrentDictionary<string, ISignal> dMeasurement = new();
  protected ConcurrentDictionary<string, ISignal> dParams = new();
  public string NameStrateg { get; set; } = "";
  public int MaxWaitRez { get; set; } = 10;
  public IDataLogger Datalogger { get; set; }
  public bool IsRezulta { get; set; }
  private bool _isLogger = false;
  #endregion

  public StrategiesBasa(IConnectLabCar iConLabCar)
  {
    IConLabCar = iConLabCar;
    _isLogger = true;
    IsRezulta = true;
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
    var _dirs =pathdir.Split('\\');
    string _nameDir = _dirs[_dirs.Length - 1];
    DConfig.Add("NameDir", _nameDir);
    string _pathDan = pathdir + "\\Dan";
    if(!Directory.Exists(_pathDan))
        Directory.CreateDirectory(_pathDan);
    DConfig.Add("FileLogger", _pathDan + "\\"+"Logger"+ _nameDir+"_.dat");

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
    if (!File.Exists(fullPath))
      throw new MyException($"No file {path}, in inicialCalibrat", -1);

    return fullPath;
  }
  protected void inicialCalibrat(string file)
  {
    string fullPath = testFile(DCalibrationParams[file].PathFiles);
    try
    {
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
        var _xx0 = getMeasurement(keyGet);
        _oneStep.AddGetPoints(keyGet, _xx0);
        if(_rezul.ContainsKey(keyGet))
            _rezul[keyGet]=_xx0;
        else
            _rezul.Add(keyGet,_xx0);
        Console.WriteLine($" {keyGet} = {_xx0}");
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
    void setPathInLogger(StOneStep  _oneStep)
    {
      try
      {
        Datalogger = IConLabCar.Experiment.DataLoggers.GetDataloggerByName(DConfig["NameDir"]);
        if (Datalogger == null)
          Datalogger = IConLabCar.Experiment.DataLoggers.CreateDatalogger(DConfig["NameDir"]);
      }
      catch (Exception)
      {
        Console.WriteLine("Error inicial Logger");
        Datalogger = null;
      }

      if (Datalogger != null)
      {
        List<string> _lsPath = new List<string>();
        List<string> _lsTask = new List<string>();
        foreach (var item in _oneStep.LoggerNamePole)
        {
          if (DTask.ContainsKey(item))
          {
            _lsPath.Add(DTask[item].PathTask);
            _lsTask.Add(DTask[item].NameInLabCar);
            //              Datalogger.AddScalarRecordingSignal(DTask[item].PathTask, "");
          }
          else if (DParameterNew.ContainsKey(item))
          {
            _lsPath.Add(DParameterNew[item].Signal);
            _lsTask.Add("");

            //              Datalogger.AddScalarRecordingSignal(DParameterNew[item].Signal, "");
            //              Datalogger.AddScalarRecordingSignal(DParameterNew[item].Signal, "");
          }
        }
        if (_lsPath.Count > 0) 
        {
          try
          {
            Datalogger.AddScalarRecordingSignals(_lsPath.ToArray(), _lsTask.ToArray());
          }
          catch (Exception)
          {
            Console.WriteLine("Ошибка записи в Datalogger сигналов ");
          }
        }

        Datalogger = IConLabCar.Experiment.DataLoggers.GetDataloggerByName(DConfig["NameDir"]);
        Datalogger.ConfigureRecordingFile(DConfig["FileLogger"], "MDF", true, 3);  //ASCII "MDF"
        Datalogger.ApplyConfiguration();
        Datalogger.Activate();
      }

    }

    if (LsStOneStep.Count < 2)
      throw new MyException("Not a complete strategy StrategiesBasa.RunTest() ", -2);

    factivCalibr();

    int _numSten = 0;
    Console.WriteLine($"  -  Start TEST - {NameStrateg} ");
    IsRezulta = true;

    while(IsRezulta && _numSten < LsStOneStep.Count)
    {
      var _oneStep = LsStOneStep[_numSten];
      Console.WriteLine($"  -  Step -> {_oneStep.StoneName} ");

      _getDanLabCar(_oneStep);
      _setDanLabCar(_oneStep);

      if(_isLogger && _oneStep.LoggerNamePole.Count> 0)
      {
        setPathInLogger(_oneStep);
      }

      if(_isLogger && _oneStep.StCommand.ContainsKey("logger") && (_oneStep.StCommand["logger"] == "start"))
        Datalogger?.Start();
      

      if (_oneStep.LIfOr.Count > 0)
      {
        if (!_ifOrDanLabCar(_oneStep))
        {
          Console.WriteLine("-- ==>  Test failed (ifOr не прошел) ((((( ----");
          IsRezulta = false;
          continue;
        }
      }

      if (_oneStep.LIf.Count > 0)
      {
        if (!_ifDanLabCar(_oneStep))
        {
          Console.WriteLine("-- ==>  Test failed (if не прошел) ((((( ----");
          IsRezulta = false;
          continue;
        }
      }

      if (_oneStep.LResult.Count > 0)
      {
        if (_rezDanLabCar(_oneStep))
        {
          Console.WriteLine("-- !!!!  Test went well !!!!");
        }
        else
        { 
          Console.WriteLine("-- ==>  Test failed ((((( ----");
          IsRezulta = false;
          continue;
        }
      }

      if (_isLogger && _oneStep.StCommand.ContainsKey("logger") && (_oneStep.StCommand["logger"] == "end"))
        Datalogger?.Stop();

      _numSten += 1;

    }

    if (IsRezulta)
      Console.WriteLine("-- !!!!  Test Прошел !!!!");
    else
      Console.WriteLine("-- ==>  Test ERROR ((((( ----");

  }

}

