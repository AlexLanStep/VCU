
namespace LabCarContext20.Core.Config;
public interface ICReadLc
{

}
public class CReadLc : ReadLcJsonLoad
{
  public string NameField { get; set; } = "";
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
  public CReadLc Initialization(string nameField, ReadLcJsonLoad sourсe)=>
          Initialization(nameField, sourсe.PathTask, sourсe.TimeLabCar, sourсe.Comment = "");
  public CReadLc Initialization(string nameField, string pathTask, string timeLabCar, string comment = "")
  {
    NameField = nameField;
    PathTask = pathTask;
    TimeLabCar = timeLabCar;
    Comment = comment;

#if MODEL
    return this;
#endif


    try { Measurement = _iConLabCar.SignalSources.CreateMeasurement(PathTask, TimeLabCar); }
    catch
    {
      Measurement = null;
    }
    return this;
  }

  public override string ToString()=> NameField;
  
}

/*
 
     //CReadLc Initialization(string nameField, string pathTask, string timeLabCar, string comment = "")
    //NameField = nameField;
    //PathTask = sourсe.PathTask;
    //TimeLabCar = sourсe.TimeLabCar;
    //Comment = sourсe.Comment;

    //try
    //{
    //  Measurement = _iConLabCar.SignalSources.CreateMeasurement(PathTask, TimeLabCar);
    //}
    //catch
    //{
    //  Measurement = null;
    //}
    //return this;

 */