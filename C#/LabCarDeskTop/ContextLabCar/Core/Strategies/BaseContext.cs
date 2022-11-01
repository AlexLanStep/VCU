
using ContextLabCar.Core.Config;
using ETAS.EE.Scripting;
using System.Xml.Linq;

namespace ContextLabCar.Core.Strategies;

public interface IBaseContext
{
  Dictionary<string, string> DConfig { get; set; }
  //  Dictionary<string, dynamic> ParamsStrategy { get; set; }
  bool IsResult { get; set; }
  void Initialization(string pathdir);
  void RunTest();

}
public class BaseContext: IBaseContext
{
  #region Data
  public bool IsResult { get; set; }

  public Dictionary<string, string> DConfig { get; set; }

  public Dictionary<string, dynamic> ParamsStrategy { get; set; }
  private List<IStOneStepNew> LsStOneStep = new List<IStOneStepNew>();

  private Dictionary<string, string> PathLabCar { get; set; } = new Dictionary<string, string>();

  #endregion
  private IConnectLabCar _iConLabCar;
  public BaseContext(IConnectLabCar iConLabCar)
  {
    _iConLabCar = iConLabCar;
    IsResult = true;
    DConfig = new Dictionary<string, string>();
  }

    public void Initialization(string pathdir)
  {
    if (!Directory.Exists(pathdir))
      throw new MyException($"Нет каталога - {pathdir}", -1);

    DConfig.Add("StDir", pathdir);
    weFormThePaths();
    var _parser = new ParserJsonNew(DConfig, ref LsStOneStep);

    foreach (var (key, value) in _parser.Config.PathLabCar)
    {
      if(DConfig.ContainsKey(key))
        DConfig[key] = value;
      else
        DConfig.Add(key, value);
    }

    _parser.ParamsStrategy.Add("Logger", true);
    ParamsStrategy = new Dictionary<string, dynamic>(_parser.ParamsStrategy);

    Console.WriteLine("Грузим переменные для старта LabCar");
    _iConLabCar.Initialization(DConfig["Workspace"], DConfig["Experiment"]);
    Console.WriteLine("Подключение к LabCar");
    _iConLabCar.Connect();
    Console.WriteLine($"Инициализация параметров для стратегии {ParamsStrategy["Name"]}:");

    _parser.LoadJsonStrategy();

    _parser.RunInicialDan();

  }
  private void weFormThePaths()
  {
    var pathdir = DConfig["StDir"];
    var dirs = pathdir.Split('\\');
    string nameDir = dirs[^1];
    DConfig.Add("NameDir", nameDir);

    string pathDan = pathdir + "\\Dan";
    if (!Directory.Exists(pathDan))
      Directory.CreateDirectory(pathDan);
    DConfig.Add("FileLogger", pathDan + "\\" + "Logger" + nameDir + "_.dat");

    var count = dirs.Length;
    var dirs0 = dirs.ToList();
    dirs0.RemoveAt(count - 1);
    dirs0.RemoveAt(count - 2);
    string pathConfigDir = string.Join("\\", dirs0.ToArray());
    DConfig.Add("DirConfig", pathConfigDir);
    DConfig.Add("DirCalibrat", pathConfigDir+ "\\Calibration");
    if (!Directory.Exists(DConfig["DirCalibrat"]))
        Directory.CreateDirectory(DConfig["DirCalibrat"]);
            

  }
  public void RunTest()
  {
    Console.WriteLine($"  -  Start TEST - {ParamsStrategy["Name"]} ");

    var _numSten =0; 
    while (IsResult && _numSten < LsStOneStep.Count)
    {
      IsResult = LsStOneStep[_numSten].Run(DConfig);

      _numSten += 1;

    }
    Console.WriteLine(IsResult ? "-- !!!!  Test Прошел !!!!" : "-- ==>  Test ERROR ((((( ----");
  }
}

/*
 
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
 



 */