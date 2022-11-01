//#define  DataEmulation

namespace ContextLabCar.Core.Config;

public class TaskLabCar: TaskJsonLoad
{

  public string NameField { get; set; } = null;
  public ISignal Measurement { get; set; } = null;
  private IConnectLabCar _iConLabCar { get; set; }
  public dynamic? Valume 
  {
    get
    {
      if (Measurement == null)
        return null;
      else
      {
        IScalarValue valueObject = (IScalarValue)Measurement.GetValueObject();
        return valueObject.GetValue();
      }
    }
  }

  public TaskLabCar(IConnectLabCar iConLabCar, string nameField, TaskJsonLoad sourse )
  {
    NameField = nameField;
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
    }

  }
  public TaskLabCar(IConnectLabCar iConLabCar, string nameField, string pathTask, string timeLabCar, string comment = "")
  {

    NameField = nameField;
    PathTask = pathTask;
    TimeLabCar = timeLabCar;
    Comment = comment;
    _iConLabCar = iConLabCar;

    try { Measurement = _iConLabCar.SignalSources.CreateMeasurement(PathTask, TimeLabCar); }
    catch
    {
      Measurement = null;
    }

  }

}
