
using System.IO;

namespace LabCarContext20.Core.Strategies;

public interface IGeneralStrategy
{
  void Run(string pathstrategdir);
}
public class GeneralStrategy: IGeneralStrategy
{
  public Dictionary<string, dynamic>? ParamsStrategy { get; set; }=new();
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
    loadParams(pathstrategdir);



  }

  private void loadParams(string path)
  {
    _pathStrategDir = path;
    var _pathConfig = _pathStrategDir + "\\Config.json";
    if (File.Exists(_pathConfig))
      _iloadConfig.ConfigLoad(_pathConfig);

    var _pathParams = _pathStrategDir + "\\Params.json";
    //    ParamsStrategy
    try
    {
      ParamsStrategy = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(File.ReadAllText(_pathParams));
    }
    catch (Exception e)
    {
      _iloggerdisplay.Write($" Проблема с файлом Params.json  -> {_pathParams} ");
      _iloggerdisplay.Write($"   -- пишем данны по умолчанию ");
      ParamsStrategy = new() {
        {"Name", "- ? -" }, //  Название стратегии правти
        { "Wait0", 1000 },  // в миллисекундах задержка мнеду шагамо
        { "Wait1", 1500 },  // в миллисекундах задержка мнеду шагамо
        { "Maxwait", 10 },  // в секундах
        { "CongigStrateg", "StrateyBasa" } // Не трогать служебная информация
      };
    }

  }
}

