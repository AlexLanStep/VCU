
namespace ContextLabCar.Core.Strategies;
public class ParserJsonDan
{
  #region ===> Data <===
  #region ==__ Public __==
  public Dictionary<string, DanInput> DDanInput = new();
  public Dictionary<string, DanOutput> DDanOutput = new();
  public Dictionary<string, FestWert> DFestWert = new();
  public Dictionary<string, object> BasaParams = new();
  public Dictionary<string, string> DPath = new();

  #endregion
  #region ___ Local ___
  private const string Path = "Path";
  private const string Input = "Input";
  private const string Output = "Output";
  private const string DanFestWert = "DanFestWert";

  private readonly string[] _fieldes = new[] { Path, Input, Output, DanFestWert };
  private readonly string _pathFiles;
  private JObject _jinfo;

  #endregion
  #endregion

  #region _ Constructor _
  public ParserJsonDan(string pathFiles) => _pathFiles = pathFiles;
  #endregion

  #region ___ run _ convert _ json
  public string? LoadFileJson(string filejson) => !File.Exists(filejson) ? null : File.ReadAllText(filejson);
  public void Run()
  {
    var tsxtJson = LoadFileJson(_pathFiles);
    if (tsxtJson != null)
      _convertJson(tsxtJson);
  }
  public virtual void AddNewPoleJson(ParserJsonDan parserjson)
  {
    //    var x = parserjson.BasaParams;
  }

  private void _convertJson(string tsxtJson)
  { //  Path, Input, Output, DanFestWert
    _jinfo = JObject.Parse(tsxtJson);
    var zJson = _jinfo.Children().ToList();
    var lsName = zJson.Select(item => ((string)((JProperty)item).Name)).ToList();

    lsName.ForEach(item => BasaParams.Add(item, _jinfo[item]));

    #region ----  Load  -> Path  -------
    if (BasaParams.TryGetValue(Path, out object valuePath))
      DPath = jsonToDicStStr(valuePath.ToString() ?? string.Empty);
    #endregion

    #region ----  Load  -> Input  -------
    if (BasaParams.TryGetValue(Input, out object valueInput))
    {
      var _params = jsonToDicStLsStr(valueInput.ToString());
      foreach( var it in _params)
      {
        var x = it.Value;
        DanInput _danInput = new DanInput(x.ElementAt(0), it.Key, x.Count == 2 ? x.ElementAt(1) : "");
        DDanInput.Add(it.Key, _danInput);
      }
//      BasaParams.AddOrUpdate(StParams, _params, (_, _) => _params);
    }
    #endregion

    #region ----  Load  -> Output  -------
    if (BasaParams.TryGetValue(Output, out object valueOutput))
    {
      var _params = jsonToDicStLsStr(valueOutput.ToString());
      foreach (var it in _params)
      {
        var x = it.Value;
        DanOutput _danOutput = new DanOutput(x.ElementAt(0), it.Key, x.Count == 2 ? x.ElementAt(1) : "");
        DDanOutput.Add(it.Key, _danOutput);
      }
      //      BasaParams.AddOrUpdate(StParams, _params, (_, _) => _params);
    }
    #endregion

    //#region ---- Load ->  StSetStart  ----------
    //if (BasaParams.TryGetValue(StSetStart, out object value1))
    //{
    //  var _setStart = CalcStSetStart(StSetStart, value1);
    //  BasaParams.AddOrUpdate(StSetStart, _setStart, (_, _) => _setStart);
    //}
    //testStSetStart();
    //#endregion

    //#region ------ Load ->  Stls  --------------
    //if (BasaParams.TryGetValue(Stls, out object value2))
    //  LsStOneStep = CalcStls(Stls, value2);
    //#endregion

  }
  #endregion

  #region function
  private Dictionary<string, dynamic> jsonToDicStDyn(string name) =>
                (Dictionary<string, dynamic>)JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(name); //
  private Dictionary<string, string> jsonToDicStStr(string name) =>
                (Dictionary<string, string>)JsonConvert.DeserializeObject<Dictionary<string, string>>(name); //
  private Dictionary<string, List<string>> jsonToDicStLsStr(string name) =>
                (Dictionary<string, List<string>>)JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(name); //

  private List<string> jsonLsString(string name) =>(List<string>)JsonConvert.DeserializeObject<List<string>>(name);
  #endregion   function
  
  //#region Stls
  //private List<StOneStep> CalcStls(string key, object val)
  //{
  //  List<StOneStep> stOneSteps = new();
  //  ConcurrentDictionary<string, object> basaParams = new();
  //  Dictionary<string, object> rezul = new();
  //  string pattern = @"[0-9]";
  //  Regex regex = new Regex(pattern);


  //  var lsSt = ((JToken)val).Children().ToList();
  //  foreach (JToken it in lsSt)
  //  {
  //    StOneStep _stOne = new();
  //    var _ee = jsonToDicStDyn(it.ToString());
  //    var _key0 = _ee.Keys;
  //    var _vv1 = _ee[_key0.ElementAt(0)];
  //    var _vv2 = jsonToDicStDyn(_vv1.ToString());


  //    if (_vv2.TryGetValue("t", out dynamic valueT))
  //      _stOne.TimeWait = (Regex.Replace(((string)(string)valueT.GetType().Name).ToLower(), pattern, "") == "string") 
  //                            ? (int)GetParams(((string)valueT)) : (int)valueT;

  //    if (_vv2.TryGetValue("get", out dynamic valueGet))
  //      _stOne.LoadInicialPosition(jsonLsString(valueGet.ToString() ?? string.Empty));

  //    if (_vv2.TryGetValue("set", out dynamic valueSet))
  //      _stOne.LoadInicialPosition(jsonToDicStDyn(valueSet.ToString() ?? string.Empty));

  //    if (_vv2.TryGetValue("rez", out dynamic valueRez))
  //      _stOne.LoadInicialRez(jsonLsString(valueRez.ToString() ?? string.Empty));

  //    AddNewPoleJson(this);

  //    stOneSteps.Add(_stOne);

  //  }

  //  return stOneSteps;
  //}
  //#endregion  Stls
  //#region SetStart
  //private Dictionary<string, dynamic> CalcStSetStart(string key, object val)
  //{
  //  ConcurrentDictionary<string, object> basaParams = new();
  //  Dictionary<string, dynamic> rezul = new();
  //  var lsName = ((JToken)val).Children()
  //                    .ToList()
  //                    .Select(item => ((string)((JProperty)item).Name)).ToList();

  //  foreach (var _w in lsName.Select(it => ((JToken)val)[it].ToString()))
  //  lsName.ForEach(item => basaParams.AddOrUpdate(item, ((JToken)val)[item], (_, _) => ((JToken)val)[item]));
  //  foreach (var (_key, _val) in basaParams)
  //  {
  //    switch (_key.ToLower())
  //    {
  //      case "loadfile":

  //      case "activfile":
  //        rezul.Add(_key, JsonConvert.DeserializeObject<List<string>>(((JToken)val)[_key].ToString()));
  //        break;

  //      case "maxwait":
  //        rezul.Add(_key, JsonConvert.DeserializeObject<dynamic>(((JToken)val)[_key].ToString()));
  //        break;
  //    }
  //  }
  //  return rezul;
  //}
  //protected dynamic? GetSetStart(string name)
  //{
  //  if (BasaParams.TryGetValue(StSetStart, out object value) && ((Dictionary<string, dynamic>)value).ContainsKey(name))
  //  {
  //    return name switch
  //    {
  //      "loadfile" => (List<string>)((Dictionary<string, dynamic>)BasaParams[StSetStart])["loadfile"],
  //      "activfile" => (List<string>)((Dictionary<string, dynamic>)BasaParams[StSetStart])["activfile"],
  //      "maxwait" => (double)((Dictionary<string, dynamic>)BasaParams[StSetStart])["maxwait"],
  //      _ => null,
  //    };
  //  }
  //  return null;
  //}
  //#endregion  SetStart

  //#region StParams
  //private Dictionary<string, dynamic> CalcStParams(string key, object val) => jsonToDicStDyn(val.ToString() ?? string.Empty);
  //protected dynamic? GetParams(string name) => (BasaParams.ContainsKey(StParams)
  //        && ((Dictionary<string, dynamic>)BasaParams[StParams]).TryGetValue(name, out dynamic valueName))? valueName : null;

  //#endregion  StParams

  //#region PTask
  //private ConcurrentDictionary<string, LTask> CalcPTask(string name, object val)
  //{
  //  ConcurrentDictionary<string, object> basaParams = new();
  //  ConcurrentDictionary<string, LTask> rezul = new();
  //  var lsName = ((JToken)val).Children()
  //                    .ToList()
  //                    .Select(item => ((string)((JProperty)item).Name)).ToList();

  //  foreach (var _w in lsName.Select(it => ((JToken)val)[it].ToString()))
  //  lsName.ForEach(item => basaParams.AddOrUpdate(item, ((JToken)val)[item], (_, _) => ((JToken)val)[item]));

  //  foreach (var (_key, _val) in basaParams)
  //  {
  //    LTask lTask=null;
  //    var _x = JsonConvert.DeserializeObject<List<string>>(((JToken)val)[_key].ToString());
  //    if (_x.Count < 2)
  //      continue;
  //    if (_x.Count == 2)
  //      lTask = new LTask(_key, _x.ElementAt(0), _x.ElementAt(1));
  //    if (_x.Count == 3)
  //      lTask = new LTask(_key, _x.ElementAt(0), _x.ElementAt(1), _x.ElementAt(2));

  //    if (lTask == null)
  //      continue;
  //    rezul.AddOrUpdate(_key, lTask, (_, _)=> lTask);
  //  }
  //  return rezul;

  //}
  //#endregion PTask

  //#region __ ==  Test == __
  //#region ___ == GetParams == ___
  //private void testGetparams()
  //{
  //  string stName = GetParams("Name")?.ToString();
  //  double? _wait0 = GetParams("Wait0");
  //  double? _wait1 = GetParams("Wait1");
  //  double? _wait2 = GetParams("Wait2");
  //  var _xcx = _wait2 * 3;

  //}
  //#endregion  ___ == GetParams == ___
  //#region ___ == StSetStart == ___
  //private void testStSetStart()
  //{
  //  var s = (List<string>)((Dictionary<string, dynamic>)BasaParams[StSetStart])["loadfile"];
  //  var s111 = GetSetStart("maxwait");
  //}
  //#endregion  ___ == StSetStart == ___
  //#endregion  __ ==  Test == __


}

/*
   "loadfile": [ "Str0Param0", "Str0Param1", "Str0Param2" ],
    "activfile": [ "Str0Param0", "Str0Param2" ],
    "maxwait": 10
 
 */