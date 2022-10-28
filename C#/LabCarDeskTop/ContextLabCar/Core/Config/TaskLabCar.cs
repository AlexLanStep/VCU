
namespace ContextLabCar.Core.Config;

public class TaskLabCar: TaskJsonLoad
{
////  [JsonPropertyName("PathTask")]
//  public string PathTask { get; set; }
////  [JsonPropertyName("TimeLabCar")]
//  public string TimeLabCar { get; set; }
////  [JsonPropertyName("Comment")]
//  public string Comment { get; set; }
  public ISignal Measurement { get; set; } = null;
  private IConnectLabCar _iConLabCar { get; set; }
  private dynamic? _valume = null;
  public dynamic Valume 
    { get { return _valume; } 
      private set 
      {
        if (Measurement == null) _valume = null;
        else 
        {
          IScalarValue valueObject = (IScalarValue)Measurement.GetValueObject();
          _valume = valueObject.GetValue();
        }
      }
  }

  public TaskLabCar(IConnectLabCar iConLabCar, TaskJsonLoad sourse )
  {
    PathTask = sourse.PathTask;
    TimeLabCar = sourse.TimeLabCar;
    Comment = sourse.Comment;
    _iConLabCar = iConLabCar;
    try 
    { 
      Measurement = _iConLabCar.SignalSources.CreateMeasurement(PathTask, TimeLabCar); 
    }
    catch 
    {
      Measurement = null;
      _valume = null;
    }
  }
  public TaskLabCar(IConnectLabCar iConLabCar, string pathTask, string timeLabCar, string comment = "")
  {
    PathTask = pathTask;
    TimeLabCar = timeLabCar;
    Comment = comment;
    _iConLabCar = iConLabCar;
    try { Measurement = _iConLabCar.SignalSources.CreateMeasurement(PathTask, TimeLabCar); }
    catch
    {
      Measurement = null;
      _valume = null;
    }
  }

}
