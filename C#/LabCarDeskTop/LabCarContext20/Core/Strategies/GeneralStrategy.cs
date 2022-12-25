
namespace LabCarContext20.Core.Strategies;

public interface IGeneralStrategy
{
  void Run(string pathstrategdir);
}
public class GeneralStrategy: IGeneralStrategy
{
  private string _pathStrategDir;
  private readonly ILoggerDisplay _iloggerdisplay;
  private readonly ICPathLc _icpathLc;
  private readonly ICPaths _icpaths;
  private readonly ICDopConfig _icdopConfig;
  private readonly ILoadConfig _iloadConfig;
  public GeneralStrategy(ILoggerDisplay iloggerdisplay, 
                          ICPathLc icpathLc, 
                          ICPaths icpaths, 
                          ICDopConfig icdopConfig,
                          ILoadConfig iloadConfig)
  {
    _iloggerdisplay = iloggerdisplay;
    _icpathLc = icpathLc;
    _icpaths = icpaths;
    _icdopConfig = icdopConfig;
    _iloadConfig = iloadConfig;
  }
  public void Run(string pathstrategdir)
  {
    _pathStrategDir = pathstrategdir;
    var _pathConfig = _pathStrategDir + "\\Config.json";
    if (File.Exists(_pathConfig))
      _iloadConfig.ConfigLoad(_pathConfig);

  }
}

