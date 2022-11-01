
namespace ContextLabCar.Core.Config;


public class GlobalConfigLabCar
{
  public GlobalConfigLabCar()
  {
  }

  public Dictionary<string, string> PathLabCar { get; set; } = new Dictionary<string, string>();
  public Dictionary<string, TaskJsonLoad> LabCarTask { get; set; } = new Dictionary<string, TaskJsonLoad>();
  public Dictionary<string, ParameterJson> Parameters = new Dictionary<string, ParameterJson>();
  public Dictionary<string, Dictionary<string, CalibrationsJson>> Calibration = new Dictionary<string, Dictionary<string, CalibrationsJson>>();

}
