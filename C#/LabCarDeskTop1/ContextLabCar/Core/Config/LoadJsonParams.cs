
namespace ContextLabCar.Core.Config;

//public class ParamsStrategy { }
public class CalibrationParams
{
  public string PathFiles { get; set; } = "";
  public Dictionary<string, CalibratJson> Parameter = new Dictionary<string, CalibratJson>();

  public CalibrationParams()
  {
  }
}

public class LoadJsonParams
{
  public Dictionary<string, dynamic> ParamsStrategy = new Dictionary<string, dynamic>();
  public Dictionary<string, ParameterJson> Parameters = new Dictionary<string, ParameterJson>();
  public Dictionary<string, DanOutputNew> Output = new Dictionary<string, DanOutputNew>();
  public Dictionary<string, CalibrationParams> Calibration = new Dictionary<string, CalibrationParams>();

  public LoadJsonParams()
  {
  }
}

