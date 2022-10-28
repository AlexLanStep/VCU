
namespace ContextLabCar.Static;

public static class LCDan
{
  private static ContainerManager _container;
  private static ConcurrentDictionary<string, TaskLabCar> _dTaskLabCar = new();
  private static IConnectLabCar _iconnect;

  static LCDan()
  {
    _container = ContainerManager.GetInstance();
    _iconnect = _container.LabCar.Resolve<IConnectLabCar>();
  }


  public static bool AddTask(string nameTask, string pathTask, string timeLabCar, string comment = "") 
  {
    TaskLabCar _task = new TaskLabCar(_iconnect, pathTask, timeLabCar, comment);
    if(_task.Measurement != null ) 
    {
      _dTaskLabCar.AddOrUpdate(nameTask, _task, (_, _) => _task);
      return true;
    }
    else
      return false;
  }

  public static bool AddTask(string nameTask, TaskJsonLoad taskJson)
  {
    TaskLabCar _task = new TaskLabCar(_iconnect, taskJson);
    if (_task.Measurement != null)
    {
      _dTaskLabCar.AddOrUpdate(nameTask, _task, (_, _) => _task);
      return true;
    }
    else
      return false;
  }

  public static List<string> AddTaskRange(Dictionary<string, TaskJsonLoad> taskLabCar)
  {
    List<string> _error = new List<string>();
    foreach (var it in taskLabCar)
    {
      if(!AddTask(it.Key, it.Value))
        _error.Add(it.Key);
    }
    return _error;
  }


}
