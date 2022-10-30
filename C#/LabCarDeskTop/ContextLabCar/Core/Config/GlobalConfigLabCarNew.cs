
namespace ContextLabCar.Core.Config;


public class GlobalConfigLabCarNew
{
  public GlobalConfigLabCarNew()
  {
  }

  public Dictionary<string, string> PathLabCar { get; set; } = new Dictionary<string, string>();
  public Dictionary<string, TaskJsonLoad> LabCarTask { get; set; } = new Dictionary<string, TaskJsonLoad>();
  public Dictionary<string, ParameterJson> Parameters = new Dictionary<string, ParameterJson>();
  public Dictionary<string, Dictionary<string, CalibratJson>> Calibration = new Dictionary<string, Dictionary<string, CalibratJson>>();

}
