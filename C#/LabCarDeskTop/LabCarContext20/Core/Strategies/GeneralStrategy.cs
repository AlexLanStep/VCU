namespace LabCarContext20.Core.Strategies;

public interface IGeneralStrategy
{
  void Run(string pathstrategdir);

}
public class GeneralStrategy: IGeneralStrategy
{
  public Dictionary<string, dynamic>? ParamsStrategy { get; set; }=new();

  #region Container
  private readonly ContainerManager? _container;
  private readonly ILoggerDisplay _iloggerdisplay;
//  private readonly ICPathLc _icpathLc;
  // ReSharper disable once NotAccessedField.Local
  //private readonly ICPaths _icpaths;
  private ICDopConfig _icdopConfig;
  private readonly ILoadConfig _iloadConfig;
  private readonly IConnectLabCar _iconnectLabCar;

  #endregion

  public string ParhStrategy { get; set; } = "";
//  public string NameDir { get; set; }

  private const string Stls = "STls";
    
  public GeneralStrategy()
  {
    _container = ContainerManager.GetInstance();
    
    _iconnectLabCar = _container.LabCar.Resolve<IConnectLabCar>();
    _iloggerdisplay = _container.LabCar.Resolve<ILoggerDisplay>();
//    _icpathLc = _container.LabCar.Resolve<ICPathLc>();
//    _icpaths = _container.LabCar.Resolve<ICPaths>();
    _icdopConfig = _container.LabCar.Resolve<ICDopConfig>();
    _iloadConfig = _container.LabCar.Resolve<ILoadConfig>();
  }
  public void Run(string pathstrategdir)
  {

    LoadParams(pathstrategdir);

    List<JToken>? lsSt;

    if (!LoadStrategy(out lsSt))
      return;

    _iconnectLabCar.Connect();

    IStrategy? ist = _container?.LabCar.Resolve<StrateyBasa>();
    ist?.SetParams(ParamsStrategy);  

    var repeat = 0;
    var success = true;
    while (repeat < _icdopConfig.Repeat && success)
    {
      if (_icdopConfig.Restart)
        _iconnectLabCar.ReStart();

      ist?.ParserStrateg(lsSt);

      if (ist != null) success = ist.Execute();

      repeat++;
    }



  // Определяем тип стратегии по умолчанию StrateyBasa
  //var _st = new StContext<StrateyBasa>();
  //_st.ParserStrateg();

  }

  private void LoadParams(string path)
  {

    ParhStrategy = path;

    var pathConfig = ParhStrategy + "\\Config.json";
    if (File.Exists(pathConfig))
      _iloadConfig.ConfigLoad(pathConfig);

    var pathParams = ParhStrategy + "\\Params.json";
    //    ParamsStrategy
    try
    {
      ParamsStrategy = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(File.ReadAllText(pathParams));
    }
    catch (Exception)
    {
      _iloggerdisplay.Write($" Проблема с файлом Params.json  -> {pathParams} ");
      _iloggerdisplay.Write($"   -- пишем данны по умолчанию ");
      ParamsStrategy = new Dictionary<string, dynamic>
      {
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
    var pathStrategy = ParhStrategy + "\\Strateg.json";

    string stStrategy = (File.Exists(pathStrategy) 
                          ? File.ReadAllText(pathStrategy) : null) 
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
       ls = ((JToken)valueStls).Children().ToList();
      return true;
    }
    else
      _iloggerdisplay.Write($" Error - {Stls} ");

    #endregion
    return false;

  }
}

