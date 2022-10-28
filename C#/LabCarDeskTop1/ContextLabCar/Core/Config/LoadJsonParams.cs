
namespace ContextLabCar.Core.Config;

//public class ParamsStrategy { }
public class CalibrationParams
{
  public string PathFiles { get; set; } = "";
  public Dictionary<string, CalibratNew> Parameter = new Dictionary<string, CalibratNew>();

  public CalibrationParams()
  {
  }
}

public class LoadJsonParams
{
  public Dictionary<string, dynamic> ParamsStrategy = new Dictionary<string, dynamic>();
  public Dictionary<string, ParameterNew> Parameters = new Dictionary<string, ParameterNew>();
  public Dictionary<string, DanOutputNew> Output = new Dictionary<string, DanOutputNew>();
  public Dictionary<string, CalibrationParams> Calibration = new Dictionary<string, CalibrationParams>();

  public LoadJsonParams()
  {
  }
}

