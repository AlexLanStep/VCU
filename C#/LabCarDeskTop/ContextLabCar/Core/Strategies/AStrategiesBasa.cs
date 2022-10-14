
using ContextLabCar.Core.Config;

namespace ContextLabCar.Core.Strategies;

public interface IAStrategiesBasa
{
  IConnectLabCar IConLabCar { get; set; }
}
public abstract class AStrategiesBasa: StrategDanJson, IAStrategiesBasa
{
  public IConnectLabCar IConLabCar { get; set; }
  protected AStrategiesBasa(IConnectLabCar iConLabCar, string jsonPath):base(jsonPath)
  {
    IConLabCar = iConLabCar;
  }


}
