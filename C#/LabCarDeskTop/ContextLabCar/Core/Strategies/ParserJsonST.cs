


namespace ContextLabCar.Core.Strategies;
public class ParserJsonST
{
  #region ===> Data <===
  #region ==__ Public __==
  public ConcurrentDictionary<string, LTask> DTask = new();
  public ConcurrentDictionary<string, object> BasaParams = new();
  public List<StOneStep> LsStOneStep = new List<StOneStep>();

  #endregion
  #region ___ Local ___
  private const string StParams = "STParams";
  private const string StSetStart = "STsetStart";
  private const string Stls = "STls";
  private const string PTask = "Task";

  private readonly string[] _fieldes = new[] { PTask, StParams, StSetStart, Stls };
  private readonly string _pathFiles;
  private JObject _jinfo;

  #endregion
  #endregion

  #region _ Constructor _
  public ParserJsonST(string pathFiles) => _pathFiles = pathFiles;
  #endregion

  #region ___ run _ convert _ json
  public string? LoadFileJson(string filejson) => !File.Exists(filejson) ? null : File.ReadAllText(filejson);
  public void Run()
  {
    var tsxtJson = LoadFileJson(_pathFiles);
    if (tsxtJson != null)
      _convertJson(tsxtJson);
  }
  public virtual void AddNewPoleJson(ParserJsonST parserjson)
  {
    //    var x = parserjson.BasaParams;
  }

  private void _convertJson(string tsxtJson)
  { //  "Task", "STParams", "STsetStart", "STls":
    _jinfo = JObject.Parse(tsxtJson);
    var zJson = _jinfo.Children().ToList();
    var lsName = zJson.Select(item => ((string)((JProperty)item).Name)).ToList();

    lsName.ForEach(item => BasaParams.AddOrUpdate(item, _jinfo[item], (_, _) => _jinfo[item]));

    #region ----  Load  -> Task  -------
    if (BasaParams.TryGetValue(PTask, out object valueTask))
    {
      var _params = CalcPTask(PTask, valueTask);
      BasaParams.AddOrUpdate(PTask, _params, (_, _) => _params);
    }
    testGetparams();

    #region ----  Load  -> StParams  -------
    if (BasaParams.TryGetValue(StParams, out object value))
    {
      var _params = CalcStParams(StParams, value);
      BasaParams.AddOrUpdate(StParams, _params, (_, _) => _params);
    }
    testGetparams();

    #endregion

    #region ---- Load ->  StSetStart  ----------
    if (BasaParams.TryGetValue(StSetStart, out object value1))
    {
      var _setStart = CalcStSetStart(StSetStart, value1);
      BasaParams.AddOrUpdate(StSetStart, _setStart, (_, _) => _setStart);
    }
    testStSetStart();
    #endregion

    #region ------ Load ->  Stls  --------------
    if (BasaParams.TryGetValue(Stls, out object value2))
      LsStOneStep = CalcStls(Stls, value2);
    #endregion
  }
  #endregion

  #region function
  private Dictionary<string, dynamic> jsonToDicStDyn(string name)=>
                (Dictionary<string, dynamic>)JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(name); //

  private List<string> jsonLsString(string name) =>(List<string>)JsonConvert.DeserializeObject<List<string>>(name);
  #endregion
  #region Stls
  private List<StOneStep> CalcStls(string key, object val)
  {
    List<StOneStep> stOneSteps = new();
    ConcurrentDictionary<string, object> basaParams = new();
    Dictionary<string, object> rezul = new();
    string pattern = @"[0-9]";
    Regex regex = new Regex(pattern);


    var lsSt = ((JToken)val).Children().ToList();
    foreach (JToken it in lsSt)
    {
      StOneStep _stOne = new();
      var _ee = jsonToDicStDyn(it.ToString());
      var _key0 = _ee.Keys;
      var _vv1 = _ee[_key0.ElementAt(0)];
      var _vv2 = jsonToDicStDyn(_vv1.ToString());


      if (_vv2.TryGetValue("t", out dynamic valueT))
        _stOne.TimeWait = (Regex.Replace(((string)(string)valueT.GetType().Name).ToLower(), pattern, "") == "string") 
                              ? (int)GetParams(((string)valueT)) : (int)valueT;

      if (_vv2.TryGetValue("get", out dynamic valueGet))
        _stOne.LoadInicialPosition(jsonLsString(valueGet.ToString() ?? string.Empty));

      if (_vv2.TryGetValue("set", out dynamic valueSet))
        _stOne.LoadInicialPosition(jsonToDicStDyn(valueSet.ToString() ?? string.Empty));

      if (_vv2.TryGetValue("rez", out dynamic valueRez))
        _stOne.LoadInicialRez(jsonLsString(valueRez.ToString() ?? string.Empty));

      AddNewPoleJson(this);

      stOneSteps.Add(_stOne);

    }

    return stOneSteps;
  }
  #endregion
  #region SetStart
  private Dictionary<string, dynamic> CalcStSetStart(string key, object val)
  {
    ConcurrentDictionary<string, object> basaParams = new();
    Dictionary<string, dynamic> rezul = new();
    var lsName = ((JToken)val).Children()
                      .ToList()
                      .Select(item => ((string)((JProperty)item).Name)).ToList();

    foreach (var _w in lsName.Select(it => ((JToken)val)[it].ToString()))
    lsName.ForEach(item => basaParams.AddOrUpdate(item, ((JToken)val)[item], (_, _) => ((JToken)val)[item]));
    foreach (var (_key, _val) in basaParams)
    {
      switch (_key.ToLower())
      {
        case "loadfile":

        case "activfile":
          rezul.Add(_key, JsonConvert.DeserializeObject<List<string>>(((JToken)val)[_key].ToString()));
          break;

        case "maxwait":
          rezul.Add(_key, JsonConvert.DeserializeObject<dynamic>(((JToken)val)[_key].ToString()));
          break;
      }
    }
    return rezul;
  }
  protected dynamic? GetSetStart(string name)
  {
    if (BasaParams.TryGetValue(StSetStart, out object value) && ((Dictionary<string, dynamic>)value).ContainsKey(name))
    {
      return name switch
      {
        "loadfile" => (List<string>)((Dictionary<string, dynamic>)BasaParams[StSetStart])["loadfile"],
        "activfile" => (List<string>)((Dictionary<string, dynamic>)BasaParams[StSetStart])["activfile"],
        "maxwait" => (double)((Dictionary<string, dynamic>)BasaParams[StSetStart])["maxwait"],
        _ => null,
      };
    }
    return null;
  }


  #endregion

  #region StParams
  private Dictionary<string, dynamic> CalcStParams(string key, object val) => jsonToDicStDyn(val.ToString() ?? string.Empty);
  protected dynamic? GetParams(string name) => (BasaParams.ContainsKey(StParams)
          && ((Dictionary<string, dynamic>)BasaParams[StParams]).TryGetValue(name, out dynamic valueName))? valueName : null;

  #endregion

  #region PTask
  private ConcurrentDictionary<string, LTask> CalcPTask(string name, object val)
  {
    ConcurrentDictionary<string, object> basaParams = new();
    ConcurrentDictionary<string, LTask> rezul = new();
    var lsName = ((JToken)val).Children()
                      .ToList()
                      .Select(item => ((string)((JProperty)item).Name)).ToList();

    foreach (var _w in lsName.Select(it => ((JToken)val)[it].ToString()))
    lsName.ForEach(item => basaParams.AddOrUpdate(item, ((JToken)val)[item], (_, _) => ((JToken)val)[item]));

    foreach (var (_key, _val) in basaParams)
    {
      LTask lTask=null;
      var _x = JsonConvert.DeserializeObject<List<string>>(((JToken)val)[_key].ToString());
      if (_x.Count < 2)
        continue;
      if (_x.Count == 2)
        lTask = new LTask(_key, _x.ElementAt(0), _x.ElementAt(1));
      if (_x.Count == 3)
        lTask = new LTask(_key, _x.ElementAt(0), _x.ElementAt(1), _x.ElementAt(2));

      if (lTask == null)
        continue;
      rezul.AddOrUpdate(_key, lTask, (_, _)=> lTask);
    }
    return rezul;

  }
  #endregion

  #region __ ==  Test == __
  #region ___ == GetParams == ___
  private void testGetparams()
  {
    string stName = GetParams("Name")?.ToString();
    double? _wait0 = GetParams("Wait0");
    double? _wait1 = GetParams("Wait1");
    double? _wait2 = GetParams("Wait2");
    var _xcx = _wait2 * 3;

  }
  #endregion
  #region ___ == StSetStart == ___
  private void testStSetStart()
  {
    var s = (List<string>)((Dictionary<string, dynamic>)BasaParams[StSetStart])["loadfile"];
    var s111 = GetSetStart("maxwait");
  }
  #endregion
  #endregion
  #endregion



}

/*
   "loadfile": [ "Str0Param0", "Str0Param1", "Str0Param2" ],
    "activfile": [ "Str0Param0", "Str0Param2" ],
    "maxwait": 10
 
 */