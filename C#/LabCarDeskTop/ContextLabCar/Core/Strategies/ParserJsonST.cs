


namespace ContextLabCar.Core.Strategies;
public class ParserJsonST: ParserJson
{
  #region ===> Data <===
  #region ==__ Public __==

//  public ConcurrentDictionary<string, object> BasaParams = new();
  #endregion

  #region ___ Local ___
  private const string StParams = "STParams";
  private const string StSetStart = "STsetStart";
  private const string Stls = "STls";

  private readonly string[] _fieldes = new[] { StParams, StSetStart, Stls };
  private readonly string _pathFiles;

  #endregion
  #endregion

  #region _ Constructor _
  public ParserJsonST(string pathFiles) : base(pathFiles) { }
  #endregion

  #region ___ run _ convert _ json
  public virtual void AddNewPoleJson(ParserJsonST parserjson)
  {
    //    var x = parserjson.BasaParams;
  }

  protected override void ConvertJson(string tsxtJson)
  {
    DSTParams = new();
    DSTsetStart = new();
    LsStOneStep = new ();

    JObject _jinfo = JObject.Parse(tsxtJson);
    var BasaParams = StartParser(_jinfo);

    #region ----  Load  -> StParams  -------
    if (BasaParams.TryGetValue(StParams, out object value))
      DSTParams = jsonToDicStDyn(value.ToString() ?? string.Empty);
    #endregion

    #region ---- Load ->  StSetStart  ----------
    if (BasaParams.TryGetValue(StSetStart, out object value1))
      CalcStSetStart(value1);
    else
    { var s1111 = "!!!!!  error "; }
    #endregion

    #region ------ Load ->  Stls  --------------
    if (BasaParams.TryGetValue(Stls, out object valueStls))
      CalcStls(valueStls);
    else
    { var s1111 = "!!!!!  error "; }

    #endregion
  }
  #endregion

  #region function
  public dynamic? GetParams(string name) => DSTParams.TryGetValue(name, out dynamic valueName) ? valueName : null;
  public dynamic? GetSetStart(string name)
  {
    if (DSTsetStart.TryGetValue(name, out object value))
    {
      return name switch
      {
        "loadfile" => (List<string>)DSTsetStart["loadfile"],
        "activfile" => (List<string>)DSTsetStart["activfile"],
        "maxwait" => (double)DSTsetStart["maxwait"],
        _ => null,
      };
    }
    return null;
  }

  #endregion

  #region Stls
  private void CalcStls(object val)
  {
    LsStOneStep.Clear();
    ConcurrentDictionary<string, object> basaParams = new();
    Dictionary<string, object> rezul = new();
    string pattern = @"[0-9]";
    Regex regex = new(pattern);

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

      LsStOneStep.Add(_stOne);

    }
  }
  #endregion

  #region SetStart
  private void CalcStSetStart(object val)
  {
    var basaParams = StartParser(val);
    foreach (var (_key, _val) in basaParams)
    {
      switch (_key.ToLower())
      {
        case "loadfile":

        case "activfile":
          DSTsetStart.Add(_key, JsonConvert.DeserializeObject<List<string>>(((JToken)val)[_key].ToString()));
          break;

        case "maxwait":
          DSTsetStart.Add(_key, JsonConvert.DeserializeObject<dynamic>(((JToken)val)[_key].ToString()));
          break;
      }
    }
  }
  #endregion

  #region __ ==  Test == __
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
    var s = (List<string>)DSTsetStart["loadfile"];
    var s111 = GetSetStart("maxwait");
  }
  #endregion
  



}
