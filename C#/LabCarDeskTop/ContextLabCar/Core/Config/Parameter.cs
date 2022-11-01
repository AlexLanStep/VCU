
using System.Text.Json.Serialization;

namespace ContextLabCar.Core.Config;
public class Parameter : IParameter
{
  public string Signal { get;}
  public string Comment { get;}

  public Parameter(string nameModel, string name, string comment="")
  {
    Comment = comment;
    Signal = $"{nameModel}/{name}/Value";
  }
}

public class ParameterJson : IParameter
{
  [JsonPropertyName("Signal")]
  public string Signal { get; }
  [JsonPropertyName("Comment")]
  public string Comment { get; }

  [Newtonsoft.Json.JsonConstructor]
  public ParameterJson(string signal, string comment = "")
  {
    Signal = signal;
    Comment = comment;
  }

}

public class ParameterNew : ParameterJson
{
  public string Name { get; set; }
  public ISignal? SignalParams { get; set; }
  public ParameterNew(IConnectLabCar iConLabCar, string nameField, string signal, string comment = "") : base(signal, comment)
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
  public ParameterNew(IConnectLabCar iConLabCar, string nameField, ParameterJson sourse) : base(sourse.Signal, sourse.Comment)
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
