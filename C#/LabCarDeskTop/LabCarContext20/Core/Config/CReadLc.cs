
namespace LabCarContext20.Core.Config;

public interface ICReadLc
{

}
public class CReadLc : ReadLcJsonLoad
{

  public string NameField { get; set; }
  public ISignal? Measurement { get; set; }
  // ReSharper disable once InconsistentNaming
  private IConnectLabCar _iConLabCar { get; set; }
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

  public CReadLc(IConnectLabCar iConLabCar)=> _iConLabCar = iConLabCar;

  public CReadLc Inicialisaci(string nameField, ReadLcJsonLoad sourсe)
  {
    NameField = nameField;
    PathTask = sourсe.PathTask;
    TimeLabCar = sourсe.TimeLabCar;
    Comment = sourсe.Comment;

    try
    {
      Measurement = _iConLabCar.SignalSources.CreateMeasurement(PathTask, TimeLabCar);
    }
    catch
    {
      Measurement = null;
    }
    return this;
  }
  public CReadLc Inicialisaci(string nameField, string pathTask, string timeLabCar, string comment = "")
  {
    NameField = nameField;
    PathTask = pathTask;
    TimeLabCar = timeLabCar;
    Comment = comment;

    try { Measurement = _iConLabCar.SignalSources.CreateMeasurement(PathTask, TimeLabCar); }
    catch
    {
      Measurement = null;
    }
    return this;
  }
}

