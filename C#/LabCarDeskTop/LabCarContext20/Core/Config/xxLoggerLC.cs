namespace LabCarContext20.Core.Config;

public class xxLoggerLc
{
  public string Name { get; set; } = "";
  public string PathDirDan { get; set; } = "";
  public IDataLogger? Datalogger { get; set; } = null;

  public xxLoggerLc() { }
  public xxLoggerLc? SetNamePath(string name, string path)
  {
    Name = name;
    PathDirDan = path;
    return this;
  }

  public void Start()
  {
#if MODEL
    return;
#endif

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
#if MODEL
    return;
#endif

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
