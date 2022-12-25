
namespace LabCarContext20.Core;

public interface ICDopConfig
{
  int Repeat { get; set; }
  bool LoggerCar { get; set; }
  bool Restart { get; set; }
  void Set(int repeat, bool loggerCar, bool restart);

}

public class CDopConfig: ICDopConfig
{
  public int Repeat { get; set; } = 1;
  public bool LoggerCar { get; set; }=false;
  public bool Restart { get; set; } = false;

  public void Set(int repeat, bool loggerCar, bool restart)
  {
    Repeat = repeat;
    LoggerCar = loggerCar;
    Restart = restart;
  }
}


