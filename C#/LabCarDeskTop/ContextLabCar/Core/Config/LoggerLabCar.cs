
namespace ContextLabCar.Core.Config;

public class LoggerLabCar
{
  public string Name { get; set; } = "";
  public string PathDirDan { get; set; } = "";
  public IDataLogger? Datalogger { get; set; } = null;
  public void Start() 
  {
				try
				{
						Datalogger?.Start();
				}
				catch (Exception)
				{
				}
		}
  public void Stop()
  {
				try
				{
						Datalogger?.Stop();
				}
				catch (Exception)
				{
				}
		}
//  public void AddScalarRecordingSignals(string[] name, string[] tasks) => Datalogger?.AddScalarRecordingSignals(name, tasks);
//  public void AddScalarSignals(string name, string tasks) { }

}
