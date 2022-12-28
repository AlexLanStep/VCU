
using LabCarContext20.Core.Config;
using System.IO;

namespace LabCarContext20.Core.Strategies;

public interface IGeneralStrategy
{
  void Run(string pathstrategdir);
}
public class GeneralStrategy: IGeneralStrategy
{
  public Dictionary<string, dynamic>? ParamsStrategy { get; set; }=new();

  #region Container
  private ContainerManager? _container = null;
  private readonly ILoggerDisplay _iloggerdisplay;
  private readonly ICPathLc _icpathLc;
  private readonly ICPaths _icpaths;
  private readonly ICDopConfig _icdopConfig;
  private readonly ILoadConfig _iloadConfig;
  private readonly IConnectLabCar _iconnectLabCar;

  #endregion
  private string _pathStrategDir;

  private const string Stls = "STls";
    
  public GeneralStrategy()
  {
    _container = ContainerManager.GetInstance();
    
    _iconnectLabCar = _container.LabCar.Resolve<IConnectLabCar>();
    _iloggerdisplay = _container.LabCar.Resolve<ILoggerDisplay>();
    _icpathLc = _container.LabCar.Resolve<ICPathLc>();
    _icpaths = _container.LabCar.Resolve<ICPaths>();
    _icdopConfig = _container.LabCar.Resolve<ICDopConfig>();
    _iloadConfig = _container.LabCar.Resolve<ILoadConfig>();
  }
  public void Run(string pathstrategdir)
  {
    loadParams(pathstrategdir);

    List<JToken>? lsSt = null;

    if (!LoadStrategy(out lsSt))
      return;

    _iconnectLabCar.Connect();

    IStrategy _ist = _container.LabCar.Resolve<StrateyBasa>();
  
    int _repeat = 0;
    bool _success = true;
    while (_repeat < _icdopConfig.Repeat && _success)
    {
      if (_icdopConfig.Restart)
        _iconnectLabCar.ReStart();

      _ist.ParserStrateg(lsSt);

      _success = _ist.Execute();

      _repeat++;
    }



  // Определяем тип стратегии по умолчанию StrateyBasa
  //var _st = new StContext<StrateyBasa>();
  //_st.ParserStrateg();

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

  private  bool LoadStrategy(out List<JToken>? ls)
  {
    ls = null;
    var _pathStrategy = _pathStrategDir + "\\Strateg.json";

    string? stStrategy = (File.Exists(_pathStrategy) 
      ? File.ReadAllText(_pathStrategy) : null) 
      ?? throw new MyException(" Error проблема в файле -> Strateg.json", -1);


    JObject jinfo;
    try
    {
      jinfo = JObject.Parse(stStrategy);
    }
    catch (Exception e)
    {
      _iloggerdisplay.Write(e.ToString());
      throw new MyException(" Error - проблема в структуре файла  Strateg.json ", -1);
    }

    ConcurrentDictionary<string, object> basaParams = new();

    var lsName = jinfo.Children().ToList().Select(item => ((JProperty)item).Name).ToList();

    // ReSharper disable free UnusedVariable
#pragma warning disable CS8604, CS8603
    foreach (var w in lsName.Select(it => ((JToken)jinfo)[it]?.ToString()))
      lsName.ForEach(item => basaParams.AddOrUpdate(item, jinfo[item], (_, _) => jinfo[item]));
#pragma warning restore CS8603, CS8604

    #region ------ Load ->  Stls  --------------
    if (basaParams.TryGetValue(Stls, out var valueStls))
    { 
//      var sss111 = valueStls;
       ls = ((JToken)valueStls).Children().ToList();
      return true;
    }
    //      CalcStls(valueStls);
    else
      _iloggerdisplay.Write($" Error - {Stls} ");

    #endregion
    return false;

  }
}

