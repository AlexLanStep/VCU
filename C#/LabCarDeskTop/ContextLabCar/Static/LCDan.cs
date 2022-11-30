
namespace ContextLabCar.Static;

public static class LcDan
{
  // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
  private static readonly ContainerManager Container;
  private static ConcurrentDictionary<string, TaskLabCar> _dTaskLabCar = new();
  private static ConcurrentDictionary<string, ParameterNew> _dParameters = new();
  private static ConcurrentDictionary<string, Calibrations> _dCalibrats = new();
  private static ConcurrentDictionary<string, LoggerLabCar> _dLoggerLabCar = new();

  private static readonly IConnectLabCar Iconnect;

  static LcDan()
  {
    Container = ContainerManager.GetInstance();
    Iconnect = Container.LabCar.Resolve<IConnectLabCar>();
  }

  #region ___ Task ___
  public static bool AddTask(string nameTask, string pathTask, string timeLabCar, string comment = "")
  {
    var task = new TaskLabCar(Iconnect, pathTask, timeLabCar, comment);
    if (task.Measurement != null)
    {
      _dTaskLabCar.AddOrUpdate(nameTask, task, (_, _) => task);
      return true;
    }
    else
      return false;
  }

  public static bool AddTask(string nameTask, TaskJsonLoad taskJson)
  {
    var task = new TaskLabCar(Iconnect, nameTask, taskJson);
    if (task.Measurement != null)
    {
      _dTaskLabCar.AddOrUpdate(nameTask, task, (_, _) => task);
      return true;
    }
    else
      return false;
  }

  public static List<string> AddTaskRange(Dictionary<string, TaskJsonLoad> taskLabCar)
    => (from it in taskLabCar where !AddTask(it.Key, it.Value) select it.Key).ToList();

  public static dynamic? GetTask(string name) => _dTaskLabCar.TryGetValue(name, out var dan) ? dan.Valume : null;
  public static TaskLabCar? GetTaskInfo(string name) => _dTaskLabCar.TryGetValue(name, out var dan) ? dan : null;

  public static Dictionary<string, dynamic?> GetTaskRange(string[] names) =>
    names.ToDictionary(name => name, name => _dTaskLabCar.TryGetValue(name, out var dan) ? dan.Valume : null);
  #endregion

  #region ___ Params ____
  public static bool AddParams(string nameParama, ParameterJson param)
  {
    var parameter = new ParameterNew(Iconnect, nameParama, param);
    if (parameter.SignalParams == null) return false;
    _dParameters.AddOrUpdate(nameParama, parameter, (_, _) => parameter);
    return true;
    //    else
    //      return false;

    //if (_task.Measurement != null)
    //{
    //  _dTaskLabCar.AddOrUpdate(nameTask, _task, (_, _) => _task);
    //  return true;
    //}
    //else
    //  return false;
  }

  public static bool SetParam(string nameParama, dynamic param)
  {
    if (_dParameters.ContainsKey(nameParama))
      return _dParameters[nameParama].SetValue(param);
    else
        throw new MyException(" Error проблема с установкой переменных ", -4);
  }

  public static List<string> AddParamsRange(Dictionary<string, ParameterJson> paramsLabCar)
    =>(from it in paramsLabCar where !AddParams(it.Key, it.Value) select it.Key ).ToList();

  public static ParameterNew? GetParamInfo(string nameParama) =>
    _dParameters.ContainsKey(nameParama) ? _dParameters[nameParama] : null;
  

  //public static List<string> AddParamsRange(Dictionary<string, ParameterJson> paramsLabCar)
  //{
  //  var result = new List<string>();
  //  foreach (var (key, value) in paramsLabCar)
  //  {
  //    if (!AddParams(key, value))
  //      result.Add(key);
  //  }
  //  return result;
  //}
  #endregion

  #region ____  Calibration ____

  public static void AddCalibration(string name, Calibrations calidr)
  {
    _dCalibrats.AddOrUpdate(name, calidr, (_, _) => calidr);
  }

  public static void CalibrationLoad(string name)
  {
    if(_dCalibrats.TryGetValue(name, out var signal))
      signal.LoadingCalibrations();
    else
      throw new MyException($" Error проблкма звгрузкой с файлам калибровки {name}", -3);
  }
  public static void CalibrationAction(string name)
  {
    if (_dCalibrats.TryGetValue(name, out var signal))
      signal.ActionCalibrations();
    else
      throw new MyException($" Error проблкма с активацией калибровкок {name}", -3);
  }
  #endregion

  #region ___ Logger ___
  public static bool AddLogger(string nameDir, string nameDirDan, string[] signal, string[] task )
  {
    var logger = new LoggerLabCar() {Name=nameDir, PathDirDan=nameDirDan };
    try
    {
      // ReSharper disable once RedundantCast
      var idlcol = (IDataLoggerCollection)Iconnect.Experiment.DataLoggers;
      idlcol.ClearConfiguration();
    }
    catch (Exception)
    {
      // ignored
    }

    logger.Datalogger = Iconnect.Experiment.DataLoggers.GetDataloggerByName(logger.Name);
        if (logger.Datalogger == null)
            logger.Datalogger = Iconnect.Experiment.DataLoggers.CreateDatalogger(logger.Name);
        else
        {
            try
            {
              Iconnect.Experiment.DataLoggers.RemoveDatalogger(logger.Datalogger);
              logger.Datalogger = Iconnect.Experiment.DataLoggers.CreateDatalogger(logger.Name);

              //if (logger.Datalogger.State == EDataloggerState.Recording)
              //            logger.Datalogger.Deactivate();
            }
            catch (Exception)
            {
              // ignored
            }
        }

    if (logger.Datalogger == null) return false;

    logger.Datalogger.AddScalarRecordingSignals(signal, task);
    logger.Datalogger = Iconnect.Experiment.DataLoggers.GetDataloggerByName(logger.Name);
    logger.Datalogger.ConfigureRecordingFile(logger.PathDirDan, "MDF", true, 3);
    logger.Datalogger.ApplyConfiguration();
    logger.Datalogger.Activate();
    if (logger.Datalogger == null) return false;
    _dLoggerLabCar.AddOrUpdate(nameDir, logger, (_, _)=> logger);
    return true;
  }

  public static LoggerLabCar? GetLogger(string nameDir) =>
    _dLoggerLabCar.TryGetValue(nameDir, out var logger) ? logger : null;

  #endregion

}
