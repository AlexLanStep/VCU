
using System.Text.Json.Serialization;

namespace LabCarContext20.Core.Config;

public interface ICWriteLcBase
{
  string Signal { get; }
  string Comment { get; }
}

public class CWriteLcBase : ICWriteLcBase
{
  public string Signal { get;}
  public string Comment { get;}

  public CWriteLcBase(string nameModel, string name, string comment="")
  {
    Comment = comment;
    Signal = $"{nameModel}/{name}/Value";
  }
}

public class CWriteLcJson : ICWriteLcBase
{
  [JsonPropertyName("Signal")]
  public string Signal { get; }
  [JsonPropertyName("Comment")]
  public string Comment { get; }

  [Newtonsoft.Json.JsonConstructor]
  public CWriteLcJson(string signal, string comment = "")
  {
    Signal = signal;
    Comment = comment;
  }

}

public class CWriteLc : CWriteLcJson
{
  public string Name { get; set; }
  public ISignal? SignalParams { get; set; }
  public CWriteLc(IConnectLabCar iConLabCar, string nameField, string signal, string comment = "") : base(signal, comment)
  {
    Name = nameField;
    try
    {
      SignalParams = iConLabCar.SignalSources.CreateParameter(Signal);
    }
    catch (Exception e)
    {
      Console.WriteLine(e);
      throw new MyException($" Error in {nameField} -> {Signal}", -2);
    }
  }
  public CWriteLc(IConnectLabCar iConLabCar, string nameField, CWriteLcJson sourse) : base(sourse.Signal, sourse.Comment)
  {
    Name = nameField;
    try
    {
        SignalParams = iConLabCar.SignalSources.CreateParameter(Signal);
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
        throw new MyException($" Error in {nameField} -> {Signal}", -2);
    }
  }

  public bool SetValue(dynamic value)
  {
    if (SignalParams == null)
      return false;

    try
    {
      var valueObject = (IScalarValue)SignalParams.GetValueObject();
      valueObject.SetValue(value);
      SignalParams.SetValueObject(valueObject);
      return true;

    }
    catch (Exception e)
    {
      Console.WriteLine($"- Error в SetValue установка переменных key-> {Name}  val-> {value}");
      Console.WriteLine(e);
      return false;
    }
  }
}
