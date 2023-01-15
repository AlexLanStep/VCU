
using LabCarContext20.Core.Config.Interface;

namespace LabCarContext20.Core.Config;

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
  Calibrations2? Initialization(string pathDirCalibr, string nameCalibr, Dictionary<string, Calibrations2Json> dCalibrat);
}
public class Calibrations2 : ICalibrations2
{
  private IConnectLabCar _iConLabCar;
  private string _pathDirCalibration; 

  public Calibrations2(IConnectLabCar iConLabCar)
  {
    _pathDirCalibration = "";
    _iConLabCar = iConLabCar;
  }

  public Calibrations2 Initialization(string pathDirCalibr, string nameCalibr, Dictionary<string, Calibrations2Json> dCalibrat)
  {
    var text = "";

    _pathDirCalibration = pathDirCalibr + "\\" + nameCalibr + ".dcm";

    foreach (var (_, value) in dCalibrat)
      text += value.Text;

    File.WriteAllText(_pathDirCalibration, text);
    return this;
  }


  public void LoadingCalibrations()
  {
#if MODEL
    return;
#endif
    try
    {
      _iConLabCar.Experiment.CalibrationController.LoadParameters(_pathDirCalibration);
    }
    catch (Exception)
    {
      _iConLabCar.Write($" - Calibration c сылка {_pathDirCalibration} загружена. -3 ");
    }

    try
    {
      _iConLabCar.Experiment.AddFile(_pathDirCalibration);
    }
    catch (Exception)
    {
      _iConLabCar.Write($" - Calibration c сылка {_pathDirCalibration} загружена.. -3");
    }

  }

  public void ActionCalibrations()
  {
#if MODEL
    return;
#endif

    try
    {
      _iConLabCar.Experiment.ActivateFile(_pathDirCalibration, true);
    }
    catch (Exception)
    {
      _iConLabCar.Write($" Problem with activating options {_pathDirCalibration}");
    }
  }
}


/*
   //public Calibrations2(string pathDirCalibr, string nameCalibr, Dictionary<string, Calibrations2Json> dCalibrat)
  //{
  //  var container = ContainerManager.GetInstance();
  //  _iConLabCar = container.LabCar.Resolve<IConnectLabCar>();
  //  _iDisplay = container.LabCar.Resolve<ILoggerDisplay>();

  //  var text = "";
    
  //  PathDirCalibration = pathDirCalibr + "\\"+nameCalibr+".dcm";

  //  foreach (var (_, value) in dCalibrat)
  //    text += value.Text;

  //  File.WriteAllText(PathDirCalibration, text);
  //}

 
 */