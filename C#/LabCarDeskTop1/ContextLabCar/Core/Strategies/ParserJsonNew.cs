
using ContextLabCar.Core.Interface;
using System.IO;
using System.Runtime;
using ContextLabCar.Static;
using ContextLabCar.Core.Config;

namespace ContextLabCar.Core.Strategies;

public class ParserJsonNew
{
  #region Data
  public GlobalConfigLabCarNew Config { get; set; }
  public Dictionary<string, dynamic> ParamsStrategy { get; set; }
  private readonly string _dirStrategy;
  private readonly string _dirConfig;
  private readonly string _dirCalibration;
  private readonly string _pathFileConfig;
  private readonly string _pathFileParams;
  private readonly string _pathFileStrateg;
  private readonly bool _isPath;
  private const string Stls = "STls";
  //public List<StOneStepNew> LsStOneStep = new List<StOneStepNew>();
  private readonly Dictionary<string, string> _dConfig;
  private ContainerManager _container;
  private IConnectLabCar _connect;
  private List<IStOneStepNew> _lsStOneStep;
  #endregion

  public ParserJsonNew(Dictionary<string, string> dConfig, ref List<IStOneStepNew> lsStOneStep)
  {
    _lsStOneStep = lsStOneStep;
    _dConfig = dConfig;
    _container = ContainerManager.GetInstance();
    _connect = _container.LabCar.Resolve<IConnectLabCar>();

    _dirConfig = _dConfig["DirConfig"];
    _dirStrategy = _dConfig["StDir"];
    _dirCalibration = _dConfig["DirCalibrat"];
    var pathGlobalConfig = _dirConfig + "\\Config.json";
    var pathdirStrategyConfig = _dirStrategy + "\\Config.json";

    _pathFileConfig = File.Exists(pathdirStrategyConfig) ? pathdirStrategyConfig :
      File.Exists(pathGlobalConfig) ? pathGlobalConfig : "";

    _pathFileParams = File.Exists(_dirStrategy + "\\Params.json")? _dirStrategy + "\\Params.json" : "";
    _pathFileStrateg = File.Exists(_dirStrategy + "\\Strateg.json") ? _dirStrategy + "\\Strateg.json" : "";

    _isPath = (_pathFileConfig.Length==0) || (_pathFileParams.Length ==0) || (_pathFileStrateg.Length==0);

    if (_isPath)
      throw new MyException($" Нет файлов json ", -1);

    loadJsonConfig();

  }

  public void RunInicialDan()
  {
    void WriteError(List<string> ls, string s)
    {
      ls.ForEach(x => Console.WriteLine($" Error ->{x}"));
      throw new MyException($" Error in {s} ", -2);
    }


    Console.WriteLine("  - Task");
    var ls0 = LCDan.AddTaskRange(Config.LabCarTask);
    if (ls0 != null && ls0.Any()) WriteError(ls0, "Task");

    Console.WriteLine("  - Params");
    var ls1 = LCDan.AddParamsRange(Config.Parameters);
    if (ls1 != null && ls1.Any()) WriteError(ls1, "Parameters");


    Console.WriteLine("  - Файлы с калибровками");
    foreach (var (key,value) in Config.Calibration)
      LCDan.AddCalibration(key, new CalibratNew(_connect, _dConfig["DirCalibrat"], key, value));
  }

  private void loadJsonConfig()
  {
    try
    {
      Config = JsonConvert.DeserializeObject<GlobalConfigLabCarNew>(File.ReadAllText(_pathFileConfig));
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

    var lsName = ((JToken)jinfo).Children().ToList().Select(item => ((JProperty)item).Name).ToList();

    foreach (var w in lsName.Select(it => ((JToken)jinfo)[it]?.ToString()))
      lsName.ForEach(item => basaParams.AddOrUpdate(item, ((JToken)jinfo)[item], (_, _) => ((JToken)jinfo)[item]));

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
      IStOneStepNew stOneNew = _container.LabCar.Resolve<IStOneStepNew>();
//      StOneStepNew stOneNew = new();
      stOneNew.ParamsStrategy = new(ParamsStrategy);
      var ee = JsonToDicStDyn(it.ToString());
      if (ee == null)
        continue;
      stOneNew.StoneName = ee.Keys.ToArray()[0];
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

  protected Dictionary<string, List<string>>? JsonToDicStLsStr(string? name) => JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(name);

  protected ConcurrentDictionary<string, object> StartParser(object val)
  {
    ConcurrentDictionary<string, object> basaParams = new();

    var lsName = ((JToken)val).Children().ToList().Select(item => ((JProperty)item).Name).ToList();

    foreach (var w in lsName.Select(it => ((JToken)val)[it]?.ToString()))
      lsName.ForEach(item => basaParams.AddOrUpdate(item, ((JToken)val)[item], (_, _) => ((JToken)val)[item]));
    return basaParams;
  }
  #endregion


}

