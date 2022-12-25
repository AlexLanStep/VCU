
using System.IO;

namespace LabCarContext20.Core;

public interface ICPaths
{
  string GlDir { get;}
  string GlReport { get; }
  string GlCalibr { get; }
  string GlConfig { get; }
  string GlVariableData { get; }
  bool SetPath(string path);
}
public class CPaths : ICPaths
{
  private string _gldir;
  private string _glreport;
  private string _glcalibr;
  private string _glconfig;
  private string _glvariabledata;


  public string GlDir => _gldir;
  public string GlReport => _glreport;
  public string GlCalibr => _glcalibr;
  public string GlConfig => _glconfig;
  public string GlVariableData => _glvariabledata;

  public bool SetPath(string path)
  {
    string NewDir(string path)
    {
      var _s = _gldir + $"\\{path}";
      if (!Directory.Exists(_s))
        Directory.CreateDirectory(_s);
      return _s;
    }
    if ((path == null)||(!File.Exists(path))) return false;
    _gldir = Path.GetDirectoryName(Path.GetFullPath(path));

    _glreport= NewDir("Report");
    _glcalibr = NewDir("Calibration");
    _glvariabledata = NewDir("VariableData");

    _glconfig = _gldir + "\\Config.json";
    _glconfig = File.Exists(_glconfig) ? _glconfig : "";

    return true;
  }

}
