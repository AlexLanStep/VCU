
namespace ContextLabCar.Core.Config;

public class LoggerLabCar
{
  public string Name { get; set; }
  public string PathDirDan { get; set; }
  public IDataLogger Datalogger { get; set; }
  public void AddScalarRecordingSignals(string[] name, string[] tasks) 
        => Datalogger?.AddScalarRecordingSignals(name, tasks);
  public void AddScalarSignals(string name, string tasks) { }
  public void Start() => Datalogger?.Start();
  public void Stop() => Datalogger?.Stop();
  
}
