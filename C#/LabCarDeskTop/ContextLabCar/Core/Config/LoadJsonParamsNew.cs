
namespace ContextLabCar.Core.Config;

//public class ParamsStrategy { }
public class CalibrationParamsNew
{
  public string PathFiles { get; set; } = "";
  public Dictionary<string, CalibratNew> Parameter = new Dictionary<string, CalibratNew>();

  public CalibrationParamsNew()
  {
  }
}

public class LoadJsonParamsNew 
{
  public Dictionary<string, dynamic> ParamsStrategy = new Dictionary<string, dynamic>();
  public Dictionary<string, ParameterNew> Parameters = new Dictionary<string, ParameterNew>();
  public Dictionary<string, DanOutputNew> Output = new Dictionary<string, DanOutputNew>();
  public Dictionary<string, CalibrationParamsNew> Calibration = new Dictionary<string, CalibrationParamsNew>();

  public LoadJsonParamsNew()
  {
  }
}

