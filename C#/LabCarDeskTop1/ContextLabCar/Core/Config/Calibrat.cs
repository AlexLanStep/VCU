
using ETAS.EE.Scripting;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace ContextLabCar.Core.Config;

// ReSharper disable six IdentifierTypo
public interface ICalibrat
{
  string Signal { get; }
  dynamic Val { get;}
  string Comment { get; }
  string Text { get; }
}
public class Calibrat : ICalibrat
{
  public string Signal { get; }
  public dynamic Val { get; }
  public string Comment { get; }
  public string Text { get; }
  public Calibrat(string nameModel, string name, dynamic val, string comment="")
  {
    Signal = Signal;
    Val = (val is string)?0.0:val;
    Comment = comment;
    Text = $"FESTWERT {nameModel}/{name}/Value \n  WERT {((string)Val.ToString()).Replace(',', '.')} \nEND\n";
  }
}

public class CalibratNew : ICalibrat
{
  [JsonPropertyName("Signal")]
  public string Signal { get; }
  [JsonPropertyName("Val")]
  public dynamic Val { get; }
  [JsonPropertyName("Comment")]
  public string Comment { get; }

  [Newtonsoft.Json.JsonIgnore]
  public string Text => $"FESTWERT {Signal} \n  WERT {((string)Val.ToString()).Replace(',', '.')} \nEND\n";
  public CalibratNew(string signal,  dynamic val, string comment = "")
  {
    Signal = signal;
    Val = (val is string) ? 0.0 : val;
    Comment = comment;
//    Text = $"FESTWERT {nameModel}/{name}/Value \n  WERT {((string)Val.ToString()).Replace(',', '.')} \nEND\n";
//    Text = $"FESTWERT {nameModel} \n  WERT {((string)Val.ToString()).Replace(',', '.')} \nEND\n";
  }
}
