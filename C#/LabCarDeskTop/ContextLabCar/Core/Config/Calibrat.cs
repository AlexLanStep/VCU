
using System.Text.Json.Serialization;

namespace ContextLabCar.Core.Config;

public interface ICalibrationsJson
{
  string Signal { get; }
  dynamic Val { get;}
  string Comment { get; }
  string Text { get; }
}

public class CalibrationsJson : ICalibrationsJson
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
  public CalibrationsJson(string signal,  dynamic val, string comment = "")
  {
    Signal = signal;
    Val = (val is string) ? 0.0 : val;
    Comment = comment;
  }
  public CalibrationsJson(ICalibrationsJson sourser)
  {
    Signal = sourser.Signal;
    Val = (sourser.Val is string) ? 0.0 : sourser.Val;
    Comment = sourser.Comment;
  }

}

public interface ICalibrations
{
  void LoadingCalibrations();
  void ActionCalibrations();
}
public class Calibrations : ICalibrations
{
  private IConnectLabCar _iConLabCar;
  private string PathDirCalibration { get; }

  public Calibrations(IConnectLabCar iConLabCar, string pathDirCalibr, string nameCalibr, Dictionary<string, CalibrationsJson> dCalibrat)
  {
    var text = "";
    _iConLabCar = iConLabCar; 
    PathDirCalibration = pathDirCalibr + "\\"+nameCalibr+".dcm";

    foreach (var (_, value) in dCalibrat)
      text += value.Text;

    File.WriteAllText(PathDirCalibration, text);
  }

  public void LoadingCalibrations()
  {
    try
    {
      _iConLabCar.Experiment.CalibrationController.LoadParameters(PathDirCalibration);
    }
    catch (Exception)
    {
      // ReSharper disable free StringLiteralTypo
      Console.WriteLine($" - Calibration c сылка {PathDirCalibration} загружена. ", -3); 
    }

    try
    {
      _iConLabCar.Experiment.AddFile(PathDirCalibration);
    }
    catch (Exception)
    {
      Console.WriteLine($" - Calibration c сылка {PathDirCalibration} загружена.. ", -3);
    }

  }

  public void ActionCalibrations()
  {
    try
    {
      _iConLabCar.Experiment.ActivateFile(PathDirCalibration, true);
    }
    catch (Exception)
    {
      Console.WriteLine($" Problem with activating options {PathDirCalibration}");
    }

  }


}


