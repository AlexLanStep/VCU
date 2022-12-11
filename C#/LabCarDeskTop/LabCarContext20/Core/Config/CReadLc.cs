using LabCarContext20.Data;

namespace LabCarContext20.Core.Config;

public interface ICReadLc
{

}
public class CReadLc : ReadLcJsonLoad
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
      var valueObject = (IScalarValue)Measurement?.GetValueObject();
#pragma warning restore CS8600
      return valueObject?.GetValue();
    }
  }

  public CReadLc(IConnectLabCar iConLabCar, string nameField, ReadLcJsonLoad sourсe)
  {
    NameField = nameField;
    PathTask = sourсe.PathTask;
    TimeLabCar = sourсe.TimeLabCar;
    Comment = sourсe.Comment;
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
  public CReadLc(IConnectLabCar iConLabCar, string nameField, string pathTask, string timeLabCar, string comment = "")
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

