

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System.Xml.Linq;

namespace ContextLabCar.Core.Strategies;
public class ParserJson
{
  public ConcurrentDictionary<string, object> BasaParams = new();
  private const string StParams = "STParams";
  private const string StSetStart = "STsetStart";
  private const string Stls = "STls";

  private readonly string[] _fieldes = new[] { StParams, StSetStart, Stls };

  private readonly string _pathFiles;
  public ParserJson(string pathFiles) => _pathFiles = pathFiles;
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

    foreach (var (key, val) in BasaParams)
    {
      //var x0 = key; var x1 = val;
      switch (key)
      {
        case StParams:
          var _params = CalcStParams(key, val); 
          break;
        case StSetStart:
          var _setStart = CalcStSetStart(key, val); break;
        case Stls:
          var _stls = CalcStls(key, val); break;

      }
    }
//    var z0 = BasaParams["STParams"].ToString();
//    var _z0 = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(z0);

    int kk = 1;
  }

  private object CalcStls(string key, object val)
  {
    ConcurrentDictionary<string, object> basaParams = new();
    Dictionary<string, object> rezul = new();


    var lsSt = ((JToken)val).Children().ToList();
    foreach (JToken it in lsSt)
    {
      var _ee = JsonConvert.DeserializeObject<Dictionary<string, Object>>(it.ToString());
      var _key0 = _ee.Keys;
      var _vv1 = _ee[_key0.ElementAt(0)];
      var _ee1 = JsonConvert.DeserializeObject<Dictionary<string, Object>>(_vv1.ToString());

    }

    return null;
  }

  private object CalcStSetStart(string key, object val)
  {
    ConcurrentDictionary<string, object> basaParams = new ();
    Dictionary<string, object> rezul = new ();
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
          var qs = JsonConvert.DeserializeObject<List<string>>(((JToken)val)[_key].ToString());
          rezul.Add(_key, qs);
          break;

        case "maxwait":
          var q = JsonConvert.DeserializeObject<dynamic>(((JToken)val)[_key].ToString());
          rezul.Add(_key, q);
          break;
      }
    }

    return rezul;
  }

  private object? CalcStParams(string key, object val)
  {
    return JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(val.ToString() ?? string.Empty); 
  }
}

/*
   "loadfile": [ "Str0Param0", "Str0Param1", "Str0Param2" ],
    "activfile": [ "Str0Param0", "Str0Param2" ],
    "maxwait": 10
 
 */