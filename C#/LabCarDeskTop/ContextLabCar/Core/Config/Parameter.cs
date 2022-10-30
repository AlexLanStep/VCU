
using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Linq;

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
  public ParameterJson(IParameter sourse)
  {
    Signal = sourse.Signal;
    Comment = sourse.Comment;
  }

}

public class ParameterNew : ParameterJson
{
  public string Name { get; set; }
  private readonly IConnectLabCar _iConLabCar;
  public ISignal? SignalParams { get; set; } = null;
  public ParameterNew(IConnectLabCar iConLabCar, string nameField, string signal, string comment = "") : base(signal, comment)
  {
    Name = nameField;
    _iConLabCar = iConLabCar;
    try
    {
      SignalParams = _iConLabCar.SignalSources.CreateParameter(Signal);
    }
    catch (Exception e)
    {
      Console.WriteLine(e);
      throw new MyException($" Error in {nameField} -> {Signal}", -2);
    }
  }
  public ParameterNew(IConnectLabCar iConLabCar, string nameField, ParameterJson sourse) : base(sourse)
  {
    Name = nameField;
    _iConLabCar = iConLabCar;
    //try
    //{
    //  SignalParams = _iConLabCar.SignalSources.CreateParameter(Signal);
    //}
    //catch (Exception e)
    //{
    //  Console.WriteLine(e);
    //  throw new MyException($" Error in {nameField} -> {Signal}", -2);
    //}
  }

  public bool SetValue(dynamic value)
  {
    if (SignalParams == null)
      return false;

    //protected void setDan(string name, dynamic dsn)
    //{
    //  var ValueObject = (IScalarValue)dParams[name].GetValueObject();
    //  ValueObject.SetValue(dsn);
    //  dParams[name].SetValueObject(ValueObject);
    //}

    try
    {
      var ValueObject = (IScalarValue)SignalParams.GetValueObject();
      ValueObject.SetValue(value);
      SignalParams.SetValueObject(ValueObject);
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
