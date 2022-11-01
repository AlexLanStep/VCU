//#define  DataEmulation

namespace ContextLabCar.Core.Config;

public class TaskLabCar: TaskJsonLoad
{

  public string NameField { get; set; }
  public ISignal? Measurement { get; set; }
  // ReSharper disable once InconsistentNaming
  private IConnectLabCar IConLabCar { get; set; }
  public dynamic? Valume 
  {
    get
    {
#pragma warning disable CS8600
      var valueObject = (IScalarValue) Measurement?.GetValueObject();
#pragma warning restore CS8600
      return valueObject?.GetValue();
    }
  }

  public TaskLabCar(IConnectLabCar iConLabCar, string nameField, TaskJsonLoad sourse )
  {
    NameField = nameField;
    PathTask = sourse.PathTask;
    TimeLabCar = sourse.TimeLabCar;
    Comment = sourse.Comment;
    IConLabCar = iConLabCar;

    try
    {
      Measurement = IConLabCar.SignalSources.CreateMeasurement(PathTask, TimeLabCar);
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
    IConLabCar = iConLabCar;

    try { Measurement = IConLabCar.SignalSources.CreateMeasurement(PathTask, TimeLabCar); }
    catch
    {
      Measurement = null;
    }

  }

}
