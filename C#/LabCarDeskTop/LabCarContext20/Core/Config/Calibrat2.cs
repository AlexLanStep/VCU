
using System.Text.Json.Serialization;

namespace LabCarContext20.Core.Config;

public interface ICalibrationsJson
{
  string Signal { get; }
  dynamic Val { get;}
  string Comment { get; }
  string Text { get; }
}

public class Calibrations2Json : ICalibrationsJson
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
  public Calibrations2Json(string signal,  dynamic val, string comment = "")
  {
    Signal = signal;
    Val = (val is string) ? 0.0 : val;
    Comment = comment;
  }
  public Calibrations2Json(ICalibrationsJson sourser)
  {
    Signal = sourser.Signal;
    Val = (sourser.Val is string) ? 0.0 : sourser.Val;
    Comment = sourser.Comment;
  }
}

public interface ICalibrations2
{
  void LoadingCalibrations();
  void ActionCalibrations();
}
public class Calibrations2 : ICalibrations2
{
  private IConnectLabCar _iConLabCar;
  private ILoggerDisplay _iDisplay;
  private string PathDirCalibration { get; }

  public Calibrations2(string pathDirCalibr, string nameCalibr, Dictionary<string, Calibrations2Json> dCalibrat)
  {
    var container = ContainerManager.GetInstance();
    _iConLabCar = container.LabCar.Resolve<IConnectLabCar>();
    _iDisplay = container.LabCar.Resolve<ILoggerDisplay>();

    var text = "";
    
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
      _iDisplay.Write($" - Calibration c сылка {PathDirCalibration} загружена. -3 ");
    }

    try
    {
      _iConLabCar.Experiment.AddFile(PathDirCalibration);
    }
    catch (Exception)
    {
      _iDisplay.Write($" - Calibration c сылка {PathDirCalibration} загружена.. -3");
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
      _iDisplay.Write($" Problem with activating options {PathDirCalibration}");
    }
  }


}


