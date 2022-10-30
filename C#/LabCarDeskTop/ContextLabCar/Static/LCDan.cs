
using System.Collections.Concurrent;

namespace ContextLabCar.Static;

public static class LCDan
{
  private static ContainerManager _container;
  private static ConcurrentDictionary<string, TaskLabCar> _dTaskLabCar = new();
  private static ConcurrentDictionary<string, ParameterNew> _dParameters = new();
  private static ConcurrentDictionary<string, CalibratNew> _dCalibrats = new();

  private static IConnectLabCar _iconnect;

  //  private Dictionary<string, TaskJsonLoad> LabCarTask { get; set; } = new Dictionary<string, TaskJsonLoad>();
  //  private Dictionary<string, ParameterJson> Parameters = new Dictionary<string, ParameterJson>();
  //  private Dictionary<string, CalibrationParamsNew> Calibration = new Dictionary<string, CalibrationParamsNew>();



  static LCDan()
  {
    _container = ContainerManager.GetInstance();
    _iconnect = _container.LabCar.Resolve<IConnectLabCar>();
  }

  #region ___ Task ___
  public static bool AddTask(string nameTask, string pathTask, string timeLabCar, string comment = "")
  {
    var task = new TaskLabCar(_iconnect, pathTask, timeLabCar, comment);
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
    TaskLabCar _task = new TaskLabCar(_iconnect, nameTask, taskJson);
    //    if (_task.Measurement != null)
    {
      _dTaskLabCar.AddOrUpdate(nameTask, _task, (_, _) => _task);
      return true;
    }
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

  public static List<string> AddTaskRange(Dictionary<string, TaskJsonLoad> taskLabCar)
    => (from it in taskLabCar where !AddTask(it.Key, it.Value) select it.Key).ToList();

  public static dynamic? GetTask(string name) => _dTaskLabCar.TryGetValue(name, out var dan) ? dan.Valume : null;

  public static Dictionary<string, dynamic?> GetTaskRange(string[] names) =>
    names.ToDictionary(name => name, name => _dTaskLabCar.TryGetValue(name, out var dan) ? dan.Valume : null);
  #endregion

  #region ___ Params ____
  public static bool AddParams(string nameParama, ParameterJson param)
  {
    var parameter = new ParameterNew(_iconnect, nameParama, param);
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

  #region Calibration

  public static void AddCalibration(string name, CalibratNew calidr)
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
}
