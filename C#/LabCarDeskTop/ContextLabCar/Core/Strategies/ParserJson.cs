
namespace ContextLabCar.Core.Strategies;

public class ParserJson
{
  #region Data
  public GlobalConfigLabCar? Config { get; set; }
  public Dictionary<string, dynamic>? ParamsStrategy { get; set; }
  // ReSharper disable once NotAccessedField.Local
  private readonly string _dirCalibration;
  private readonly string _pathFileConfig;
  private readonly string _pathFileParams;
  private readonly string _pathFileStrateg;
  private const string Stls = "STls";
  private readonly Dictionary<string, string> _dConfig;
  private ContainerManager _container;
  private IConnectLabCar _connect;
  private List<IStOneStepNew> _lsStOneStep;
  #endregion

  public ParserJson(Dictionary<string, string> dConfig, ref List<IStOneStepNew> lsStOneStep)
  {
    _lsStOneStep = lsStOneStep;
    _dConfig = dConfig;
    _container = ContainerManager.GetInstance();
    _connect = _container.LabCar.Resolve<IConnectLabCar>();

    var dirConfig = _dConfig["DirConfig"];
    var dirStrategy = _dConfig["StDir"];
    _dirCalibration = _dConfig["DirCalibrat"];
    var pathGlobalConfig = dirConfig + "\\Config.json";
    var pathdirStrategyConfig = dirStrategy + "\\Config.json";

    _pathFileConfig = File.Exists(pathdirStrategyConfig) ? pathdirStrategyConfig :
      File.Exists(pathGlobalConfig) ? pathGlobalConfig : "";

    _pathFileParams = File.Exists(dirStrategy + "\\Params.json")? dirStrategy + "\\Params.json" : "";
    _pathFileStrateg = File.Exists(dirStrategy + "\\Strateg.json") ? dirStrategy + "\\Strateg.json" : "";

    if ((_pathFileConfig.Length == 0) || (_pathFileParams.Length == 0) || (_pathFileStrateg.Length == 0))
      throw new MyException($" Нет файлов json ", -1);

    LoadJsonConfig();

  }

  public void RunInicialDan()
  {
    void WriteError(List<string> ls, string s)
    {
      ls.ForEach(x => Console.WriteLine($" Error ->{x}"));
      throw new MyException($" Error in {s} ", -2);
    }


    Console.WriteLine("  - Task");
#pragma warning disable CS8602
    var ls0 = LcDan.AddTaskRange(Config.LabCarTask);
#pragma warning restore CS8602
    if (ls0 != null && ls0.Any()) WriteError(ls0, "Task");

    Console.WriteLine("  - Params");
    var ls1 = LcDan.AddParamsRange(Config.Parameters);
    if (ls1 != null && ls1.Any()) WriteError(ls1, "Parameters");


    Console.WriteLine("  - Файлы с калибровками");
    foreach (var (key,value) in Config.Calibration)
      LcDan.AddCalibration(key, new Calibrations(_connect, _dConfig["DirCalibrat"], key, value));
  }

  private void LoadJsonConfig()
  {
    try
    {
      Config = JsonConvert.DeserializeObject<GlobalConfigLabCar>(File.ReadAllText(_pathFileConfig));
    }
    catch (Exception e)
    {
      Console.WriteLine(e);
      throw new MyException($" Проблема с файлом Config.json -> {_pathFileConfig}  ", -1);
    }

    if (Config == null)
      throw new MyException($" Проблема с файлом Config.json  -> {_pathFileConfig} ", -1);

    try
    {
      ParamsStrategy = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(File.ReadAllText(_pathFileParams));
    }
    catch (Exception e)
    {
      Console.WriteLine(e);
      throw new MyException($" Проблема с файлом Params.json  -> {_pathFileParams} ", -1);
    }

    if (ParamsStrategy == null)
      throw new MyException($" Проблема с файлом Params.json  -> {_pathFileParams} ", -1);

  }
  public void LoadJsonStrategy()
  {
    string? stStrategy = File.Exists(_pathFileStrateg) ? File.ReadAllText(_pathFileStrateg) : null;
    if (stStrategy == null)
      throw new MyException(" Error проблема в файле -> Strateg.json", -1);

    JObject jinfo;
    try
    {
      jinfo = JObject.Parse(stStrategy);
    }
    catch (Exception e)
    {
      Console.WriteLine(e);
      throw new MyException(" Error - проблема в структуре файла  Strateg.json ", -1);
    }

    ConcurrentDictionary<string, object> basaParams = new();

    var lsName = jinfo.Children().ToList().Select(item => ((JProperty)item).Name).ToList();

    // ReSharper disable free UnusedVariable
    foreach (var w in lsName.Select(it => ((JToken)jinfo)[it]?.ToString()))
#pragma warning disable CS8604
#pragma warning disable CS8603
      lsName.ForEach(item => basaParams.AddOrUpdate(item, jinfo[item], (_, _) => jinfo[item]));
#pragma warning restore CS8603
#pragma warning restore CS8604

    #region ------ Load ->  Stls  --------------
    if (basaParams.TryGetValue(Stls, out var valueStls))
      CalcStls(valueStls);
    else
    {
      Console.WriteLine($" Error - {Stls} ");
    }

    #endregion

  }

  #region Stls
  private void CalcStls(object val)
  {

    _lsStOneStep.Clear();
    const string pattern = @"[0-9]";

    var lsSt = ((JToken)val).Children().ToList();
    foreach (var it in lsSt)
    {
      var stOneNew = _container.LabCar.Resolve<IStOneStepNew>();

      if (ParamsStrategy != null) stOneNew.ParamsStrategy = new Dictionary<string, dynamic>(ParamsStrategy);
      var ee = JsonToDicStDyn(it.ToString());
      if (ee == null)
        continue;
      stOneNew.StoneName = ee.Keys.ToArray()[0];
      // ReSharper disable once ConstantConditionalAccessQualifier
      var vv1 = ee?[ee.Keys.ElementAt(0)];
      var vv2 = JsonToDicStDyn(vv1?.ToString());

#pragma warning disable CS8605
      if (vv2.TryGetValue("t", out dynamic valueT))
        stOneNew.TimeWait = (Regex.Replace(((string)valueT.GetType().Name).ToLower(), pattern, "") == "string")
          ? (int)GetParams(((string)valueT)) : (int)valueT;
#pragma warning restore CS8605

      if (vv2.TryGetValue("get", out dynamic valueGet))
        stOneNew.LoadInitializationPosition(JsonLsString(valueGet.ToString() ?? string.Empty));

      if (vv2.TryGetValue("set", out dynamic valueSet))
        stOneNew.LoadInitializationPosition(JsonToDicStDyn(valueSet.ToString() ?? string.Empty));

      if (vv2.TryGetValue("if", out dynamic valueif))
        stOneNew.LoadInitializationIf(JsonLsString(valueif.ToString() ?? string.Empty));

      if (vv2.TryGetValue("restart", out dynamic valuerestart))
      {
        if(stOneNew.StCommand.ContainsKey("restart"))
          stOneNew.StCommand["restart"] = (string)"";
        else
          stOneNew.StCommand.Add("restart", (string)"");
      }
      if (vv2.TryGetValue("sim", out dynamic valuesim))
      {
        if (stOneNew.StCommand.ContainsKey("sim"))
          stOneNew.StCommand["sim"] = (string)valuesim;
        else
          stOneNew.StCommand.Add("sim", (string)valuesim);
      }

      if (vv2.TryGetValue("loggerset", out dynamic valuelogset))
        stOneNew.LoggerNamePole = JsonLsString(valuelogset.ToString() ?? string.Empty);

      if (vv2.TryGetValue("logger", out dynamic valuelog))
      {
        if (stOneNew.StCommand.ContainsKey("logger"))
          stOneNew.StCommand["logger"] = (string)valuelog;
        else
          stOneNew.StCommand.Add("logger", (string)valuelog);
      }

      if (vv2.TryGetValue("loadfile", out dynamic valueloadfile))
        stOneNew.CalibrationsLoad = JsonLsString(valueloadfile.ToString() ?? string.Empty);

      if (vv2.TryGetValue("activfile", out dynamic valueactivfile))
        stOneNew.CalibrationsActiv = JsonLsString(valueactivfile.ToString() ?? string.Empty);
!!!!!!!!!!!!!!!!!!!!!!!!!!!!
      if (vv2.TryGetValue("let", out dynamic valueLet))
        stOneNew.LoadArifmetic(JsonLsString(valueLet.ToString() ?? string.Empty));


      _lsStOneStep.Add(stOneNew);

    }
  }
  #endregion

  #region function
  public dynamic? GetParams(string name) => ParamsStrategy != null && ParamsStrategy.TryGetValue(name, out var valueName) ? valueName : null;

#pragma warning disable CS8600
#pragma warning disable CS8605

  protected Dictionary<string, dynamic>? JsonToDicStDyn(string name) =>
    JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(name);
  protected List<string>? JsonLsString(string name) => JsonConvert.DeserializeObject<List<string>>(name);

  protected Dictionary<string, string>? JsonToDicStStr(string name) => JsonConvert.DeserializeObject<Dictionary<string, string>>(name);

#pragma warning disable CS8604
  protected Dictionary<string, List<string>>? JsonToDicStLsStr(string? name) => JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(name);
#pragma warning restore CS8604

  protected ConcurrentDictionary<string, object> StartParser(object val)
  {
    ConcurrentDictionary<string, object> basaParams = new();

    var lsName = ((JToken)val).Children().ToList().Select(item => ((JProperty)item).Name).ToList();

    // ReSharper disable for UnusedVariable
    foreach (var w in lsName.Select(it => ((JToken)val)[it]?.ToString()))
#pragma warning disable CS8604
#pragma warning disable CS8603
      lsName.ForEach(item => basaParams.AddOrUpdate(item, ((JToken)val)[item], (_, _) => ((JToken)val)[item]));
#pragma warning restore CS8603
#pragma warning restore CS8604
    return basaParams;
  }
  #endregion


}

