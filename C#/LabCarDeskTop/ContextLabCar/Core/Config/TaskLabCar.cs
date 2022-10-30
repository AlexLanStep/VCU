#define  DataEmulation

namespace ContextLabCar.Core.Config;

public class TaskLabCar: TaskJsonLoad
{
#if DataEmulation
  private static Random rnd;
#endif

  public string NameField { get; set; } = null;
  public ISignal Measurement { get; set; } = null;
  private IConnectLabCar _iConLabCar { get; set; }
//  private dynamic? _valume = null;
  public dynamic? Valume 
    {
      get
      {
#if DataEmulation
        return rnd.Next();
#else
      if (Measurement == null)
          return null;
        else
        {
          IScalarValue valueObject = (IScalarValue)Measurement.GetValueObject();
          return valueObject.GetValue();
        }
#endif
    }
  }

  public TaskLabCar(IConnectLabCar iConLabCar, string nameField, TaskJsonLoad sourse )
  {
#if DataEmulation
    rnd = new Random();
#endif
    NameField = nameField;
    PathTask = sourse.PathTask;
    TimeLabCar = sourse.TimeLabCar;
    Comment = sourse.Comment;
    _iConLabCar = iConLabCar;

#if DataEmulation
#else
    try
    {
      Measurement = _iConLabCar.SignalSources.CreateMeasurement(PathTask, TimeLabCar);
    }
    catch
    {
      Measurement = null;
      _valume = null;
    }
#endif

  }
  public TaskLabCar(IConnectLabCar iConLabCar, string nameField, string pathTask, string timeLabCar, string comment = "")
  {
#if DataEmulation
    rnd = new Random();
#endif

    NameField = nameField;
    PathTask = pathTask;
    TimeLabCar = timeLabCar;
    Comment = comment;
    _iConLabCar = iConLabCar;

#if DataEmulation
#else
    try { Measurement = _iConLabCar.SignalSources.CreateMeasurement(PathTask, TimeLabCar); }
    catch
    {
      Measurement = null;
      _valume = null;
    }
#endif

  }

}
