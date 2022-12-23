
namespace LabCarContext20.Core;

public interface ICPaths
{
  string GlDir { get; set; }
  string GlReport { get; set; }
  string GlCalibr { get; set; }
  string GlConfig { get; set; }
  bool SetPath(string path);
}
public class CPaths : ICPaths
{
  public string GlDir { get; set; }
  public string GlReport 
  {
    get => GlDir != "" ? GlDir + "\\Report" : "";
    set { } 
  }

  public string GlCalibr
  { 
    get => GlDir != "" ? GlDir + "\\Calibration" : "";
    set { } 
  }
  public string GlConfig 
  { get => GlDir != "" ? GlDir + "\\Config.json" : "";
    set { } 
  }

  public bool SetPath(string path)
  {
    if ((path == null)||(!File.Exists(path))) return false;

    GlDir = Path.GetDirectoryName(Path.GetFullPath(path));

    return true;
  }

}
