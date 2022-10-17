

using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace ContextLabCar.Core.Strategies;
public class ParserJson
{
  public ConcurrentDictionary<string, object> BasaParams = new();
  private const string StParams = "STParams";
  private const string StSetStart = "STsetStart";
  private const string Stls = "STls";
  //private Dictionary<string, string> groups = new() 
  //{
  //  { "StParams", "STParams" },
  //  { "StSetStart",  "STsetStart" },
  //  { "Stls",  "STls" }
  //};
  private readonly string[] _fieldes = new[] { StParams, StSetStart, Stls };
  private readonly string _pathFiles;
  public List<StOneStep> LsStOneStep = new List<StOneStep>();


  public ParserJson(string pathFiles) 
  {
//    _fieldes = groups.Keys.ToArray(); // new[] { StParams, StSetStart, Stls };
    _pathFiles = pathFiles; 
  }
  public string? LoadFileJson(string filejson) => !File.Exists(filejson) ? null: File.ReadAllText(filejson);
  private JObject _jinfo;

  public void Run()
  {
      var tsxtJson = LoadFileJson(_pathFiles);
      if (tsxtJson != null)
          _convertJson(tsxtJson);
  }

  private void _convertJson(string tsxtJson)
  {
    //    "STParams", "STsetStart",  "STls":
    _jinfo = JObject.Parse(tsxtJson);
    var zJson = _jinfo.Children().ToList();
    var lsName = zJson.Select(item => ((string)((JProperty)item).Name)).ToList();

    lsName.ForEach(item=> BasaParams.AddOrUpdate(item, _jinfo[item], (_, _) => _jinfo[item]));

    if (BasaParams.ContainsKey(StParams))
    { 
      var _params = CalcStParams(StParams, BasaParams[StParams]);
      BasaParams.AddOrUpdate(StParams, _params, (_, _) =>_params);    
    }

    string stName = GetParams("Name")?.ToString();
    double? _wait0 = GetParams("Wait0");
    double? _wait1 = GetParams("Wait1");
    double? _wait2 = GetParams("Wait2");
    var _xcx = _wait2 * 3;

    //----------------------------------------------------------------------------------

    if (BasaParams.ContainsKey(StSetStart))
    { 
      var _setStart = CalcStSetStart(StSetStart, BasaParams[StSetStart]);
      BasaParams.AddOrUpdate(StSetStart, _setStart, (_, _) => _setStart); 
    }

    var s = (List<string>)((Dictionary<string, dynamic>)BasaParams[StSetStart])["loadfile"];

    var s111 = GetSetStart("maxwait");

    if (BasaParams.ContainsKey(Stls))
    {
      LsStOneStep = CalcStls(Stls, BasaParams[Stls]);
    }


    int kk = 1;
  }

  private List<StOneStep> CalcStls(string key, object val)
  {
    List<StOneStep> stOneSteps = new ();
    ConcurrentDictionary<string, object> basaParams = new();
    Dictionary<string, object> rezul = new();
    string pattern = @"[0-9]";
    Regex regex = new Regex(pattern);


    var lsSt = ((JToken)val).Children().ToList();
    foreach (JToken it in lsSt)
    {
      StOneStep _stOne = new();
      var _ee = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(it.ToString());
      var _key0 = _ee.Keys;
      var _vv1 = _ee[_key0.ElementAt(0)];
      var _vv2 = (Dictionary<string, dynamic>) JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(_vv1.ToString());
      if (_vv2.ContainsKey("t"))
      {
        var _xtype = Regex.Replace(((string)(string)_vv2["t"].GetType().Name).ToLower(), pattern, "");
        _stOne.TimeWait = (_xtype == "string")? (int)GetParams(((string)_vv2["t"])):(int)_vv2["t"];
      }

      if (_vv2.ContainsKey("get"))
        _stOne.LoadInicialPosition((List<string>)JsonConvert.DeserializeObject<List<string>>(_vv2["get"].ToString()
                    ?? string.Empty));

      if (_vv2.ContainsKey("set"))
        _stOne.LoadInicialPosition((Dictionary<string, dynamic>)JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(_vv2["set"].ToString() 
                    ?? string.Empty));

      if (_vv2.ContainsKey("rez"))
        _stOne.LoadInicialRez((List<string>)JsonConvert.DeserializeObject<List<string>>(_vv2["rez"].ToString()
                    ?? string.Empty));

      stOneSteps.Add(_stOne);

    }

    return stOneSteps;
  }

  private Dictionary<string, dynamic> CalcStSetStart(string key, object val)
  {
    ConcurrentDictionary<string, object> basaParams = new ();
    Dictionary<string, dynamic> rezul = new ();
    var lsName = ((JToken)val).Children().ToList()
                                  .Select(item => ((string)((JProperty)item).Name)).ToList();

    foreach (var _w in lsName.Select(it => ((JToken) val)[it].ToString())) 
    
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

  private Dictionary<string, dynamic> CalcStParams(string key, object val) 
        => JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(val.ToString() ?? string.Empty);

//  protected dynamic GetParams(string name) => ((Dictionary<string, dynamic>)BasaParams[StParams])[name];
  protected dynamic? GetParams(string name)=> BasaParams.ContainsKey(StParams)
            && ((Dictionary<string, dynamic>)BasaParams[StParams]).TryGetValue(name, out dynamic value) ? value : null;
  

  protected dynamic? GetSetStart(string name)
  {
    if(BasaParams.ContainsKey(StSetStart) && ((Dictionary<string, dynamic>)BasaParams[StSetStart]).ContainsKey(name))
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
}

/*
   "loadfile": [ "Str0Param0", "Str0Param1", "Str0Param2" ],
    "activfile": [ "Str0Param0", "Str0Param2" ],
    "maxwait": 10
 
 */