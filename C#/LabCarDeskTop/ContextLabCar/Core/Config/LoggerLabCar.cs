﻿
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
      // ignored
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
      // ignored
    }
  }

}
