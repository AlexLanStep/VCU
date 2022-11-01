
using ETAS.EE.Scripting;
using System.Text.Json.Serialization;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace ContextLabCar.Core.Config;

// ReSharper disable six IdentifierTypo
public interface ICalibratJson
{
  string Signal { get; }
  dynamic Val { get;}
  string Comment { get; }
  string Text { get; }
}
public class Calibrat : ICalibratJson
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

public class CalibratJson : ICalibratJson
{
  [JsonPropertyName("Signal")]
  public string Signal { get; }
  [JsonPropertyName("Val")]
  public dynamic Val { get; }
  [JsonPropertyName("Comment")]
  public string Comment { get; }

  [Newtonsoft.Json.JsonIgnore]
  public string Text => $"FESTWERT {Signal} \n  WERT {((string)Val.ToString()).Replace(',', '.')} \nEND\n";

  [Newtonsoft.Json.JsonConstructor]
  public CalibratJson(string signal,  dynamic val, string comment = "")
  {
    Signal = signal;
    Val = (val is string) ? 0.0 : val;
    Comment = comment;
  }
  public CalibratJson(ICalibratJson sourser)
  {
    Signal = sourser.Signal;
    Val = (sourser.Val is string) ? 0.0 : sourser.Val;
    Comment = sourser.Comment;
  }

}

public interface ICalibratNew
{
  void LoadingCalibrations();
  void ActionCalibrations();
}
public class CalibratNew : ICalibratNew
{
  private IConnectLabCar _iConLabCar;
  private string _pathDirCalibrat { get; }
  private readonly string _text;

  public CalibratNew(IConnectLabCar iConLabCar, string pathDirCalibr, string nameCalibr, Dictionary<string, CalibratJson> dCalibrat)
  {
    _text = "";
    _iConLabCar = iConLabCar; 
    _pathDirCalibrat = pathDirCalibr + "\\"+nameCalibr+".dcm";

    foreach (var (_, value) in dCalibrat)
      _text += value.Text;

    File.WriteAllText(_pathDirCalibrat, _text);
  }

  public void LoadingCalibrations()
  {
    try
    {
      _iConLabCar.Experiment.CalibrationController.LoadParameters(_pathDirCalibrat);
    }
    catch (Exception)
    {
      Console.WriteLine($" - Calibration c сылка {_pathDirCalibrat} загружена. ", -3); 
    }

    try
    {
      _iConLabCar.Experiment.AddFile(_pathDirCalibrat);
    }
    catch (Exception)
    {
      Console.WriteLine($" - Calibration c сылка {_pathDirCalibrat} загружена.. ", -3);
    }

  }

  public void ActionCalibrations()
  {
    try
    {
      _iConLabCar.Experiment.ActivateFile(_pathDirCalibrat, true);
    }
    catch (Exception)
    {
      Console.WriteLine($" Problem with activating options {_pathDirCalibrat}");
    }

  }


}




//    Text = $"FESTWERT {nameModel}/{name}/Value \n  WERT {((string)Val.ToString()).Replace(',', '.')} \nEND\n";
//    Text = $"FESTWERT {nameModel} \n  WERT {((string)Val.ToString()).Replace(',', '.')} \nEND\n";

