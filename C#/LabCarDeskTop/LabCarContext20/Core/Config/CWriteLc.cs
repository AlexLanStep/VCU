
using System.Text.Json.Serialization;

namespace LabCarContext20.Core.Config;

public interface ICWriteLcBase
{
  string Signal { get; set; }
  string Comment { get; set; }
  void Inicialisation(string signal, string comment = "");

}

public class CWriteLcBase : ICWriteLcBase
{
  public string Signal { get; set; }
  public string Comment { get; set;}

  public CWriteLcBase(string nameModel, string name, string comment="")
  {
    Comment = comment;
    Signal = $"{nameModel}/{name}/Value";
  }
  public CWriteLcBase()  {  }

  public void Inicialisation(string signal, string comment = "")
  {
    Signal = signal;
    Comment = comment;
  }
}

public class CWriteLcJson : ICWriteLcBase
{
  [JsonPropertyName("Signal")]
  public string Signal { get; set; }
  [JsonPropertyName("Comment")]
  public string Comment { get; set; }

  [Newtonsoft.Json.JsonConstructor]
  public CWriteLcJson(string signal, string comment = "")
  {
    Signal = signal;
    Comment = comment;
  }
  public CWriteLcJson()
  {
  }
  public void Inicialisation(string signal, string comment = "")
  {
    Signal = signal;
    Comment = comment;
  }

}

public class CWriteLc : CWriteLcJson
{
  public string Name { get; set; }
  public ISignal? SignalParams { get; set; }
  private IConnectLabCar _iConLabCar;
  //public CWriteLc(IConnectLabCar iConLabCar, string nameField, string signal, string comment = "") : base(signal, comment)
  //{
  //  Name = nameField;
  //  try
  //  {
  //    SignalParams = iConLabCar.SignalSources.CreateParameter(Signal);
  //  }
  //  catch (Exception e)
  //  {
  //    Console.WriteLine(e);
  //    throw new MyException($" Error in {nameField} -> {Signal}", -2);
  //  }
  //}

  public CWriteLc(IConnectLabCar iConLabCar) : base()
  {
    _iConLabCar = iConLabCar;
  }
  public CWriteLc Inicialization(string nameField, string signal, string comment="")
  {
    base.Inicialisation(signal, comment);
    Name = nameField;
    try
    {
      SignalParams = _iConLabCar.SignalSources.CreateParameter(Signal);
    }
    catch (Exception e)
    {
      _iConLabCar.Write(e.ToString());
      throw new MyException($" Error in {nameField} -> {Signal}", -2);
    }
    return this;
  }

  public CWriteLc Inicialization(string nameField, CWriteLcJson sourse)
  {
    base.Inicialisation(sourse.Signal, sourse.Comment);
    Name = nameField;
    try
    {
      SignalParams = _iConLabCar.SignalSources.CreateParameter(Signal);
    }
    catch (Exception e)
    {
      _iConLabCar.Write(e.ToString());
      throw new MyException($" Error in {nameField} -> {Signal}", -2);
    }
    return this;
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
      _iConLabCar.Write($"- Error в SetValue установка переменных key-> {Name}  val-> {value}");
      _iConLabCar.Write(e.ToString());
      return false;
    }
  }
}
