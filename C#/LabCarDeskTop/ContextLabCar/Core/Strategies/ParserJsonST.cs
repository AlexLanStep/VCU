


#nullable enable
namespace ContextLabCar.Core.Strategies;
public class ParserJsonSt: ParserJson
{
  #region ===> Data <===
  #region ==__ Public __==

  #endregion

  #region ___ Local ___
  private const string StParams = "STParams";
  private const string StSetStart = "STsetStart";
  private const string Stls = "STls";

  #endregion
  #endregion

  #region _ Constructor _
  public ParserJsonSt(string pathFiles) : base(pathFiles) { }
  #endregion

  #region ___ run _ convert _ json
  public virtual void AddNewPoleJson(ParserJsonSt parserjson) { }

  protected override void ConvertJson(string tsxtJson)
  {
    DstParams = new Dictionary<string, dynamic>();
    DsTsetStart = new Dictionary<string, dynamic?>();
    LsStOneStep = new List<StOneStep>();

    var jinfo = JObject.Parse(tsxtJson);
    var basaParams = StartParser(jinfo);

    #region ----  Load  -> StParams  -------
    if (basaParams.TryGetValue(StParams, out var value))
      DstParams = JsonToDicStDyn(value.ToString() ?? string.Empty);
    #endregion

    #region ---- Load ->  StSetStart  ----------
    if (basaParams.TryGetValue(StSetStart, out var value1))
      CalcStSetStart(value1);
    else
    {
      Console.WriteLine($" Error - {StSetStart} "); 
    }
    #endregion

    #region ------ Load ->  Stls  --------------
    if (basaParams.TryGetValue(Stls, out var valueStls))
      CalcStls(valueStls);
    else
    {
      Console.WriteLine($" Error - {Stls} ");
    }

    #endregion
  }
  #endregion

  #region function
  public dynamic? GetParams(string name) => DstParams != null && DstParams.TryGetValue(name, out var valueName) ? valueName : null;

#pragma warning disable CS8600
#pragma warning disable CS8605
  // ReSharper disable for StringLiteralTypo

  public dynamic? GetSetStart(string name)
  {
    if (DsTsetStart.TryGetValue(name, out _))
    {
      return name switch
      {
        "loadfile" => (List<string>)DsTsetStart["loadfile"],
        "activfile" => (List<string>)DsTsetStart["activfile"],
        "maxwait" => (double)DsTsetStart["maxwait"],
        _ => null,
      };
    }
    return null;
  }
#pragma warning restore CS8605
#pragma warning restore CS8600

  #endregion

  #region Stls
  private void CalcStls(object val)
  {
    LsStOneStep.Clear();
    const string pattern = @"[0-9]";

    var lsSt = ((JToken)val).Children().ToList();
    foreach (var it in lsSt)
    {
      StOneStep stOne = new();
      var ee = JsonToDicStDyn(it.ToString());
      var vv1 = ee?[ee.Keys.ElementAt(0)];
      var vv2 = JsonToDicStDyn(vv1?.ToString());

#pragma warning disable CS8605
      if (vv2.TryGetValue("t", out dynamic valueT))
        stOne.TimeWait = (Regex.Replace(((string)valueT.GetType().Name).ToLower(), pattern, "") == "string") 
          ? (int)GetParams(((string)valueT)) : (int)valueT;
#pragma warning restore CS8605

      if (vv2.TryGetValue("get", out dynamic valueGet))
        stOne.LoadInitializationPosition(JsonLsString(valueGet.ToString() ?? string.Empty));

      if (vv2.TryGetValue("set", out dynamic valueSet))
        stOne.LoadInitializationPosition(JsonToDicStDyn(valueSet.ToString() ?? string.Empty));

      if (vv2.TryGetValue("rez", out dynamic valueRez))
        stOne.LoadInitializationRez1(JsonLsString(valueRez.ToString() ?? string.Empty));

      if (vv2.TryGetValue("if", out dynamic valueif))
        stOne.LoadInitializationIf1(JsonLsString(valueif.ToString() ?? string.Empty));

      AddNewPoleJson(this);

      LsStOneStep.Add(stOne);

    }
  }
  #endregion

  #region SetStart
  private void CalcStSetStart(object val)
  {
    var basaParams = StartParser(val);
    foreach (var (key, _) in basaParams)
    {
      switch (key.ToLower())
      {
        case "loadfile":

        case "activfile":
          DsTsetStart.Add(key, JsonConvert.DeserializeObject<List<string>>(((JToken)val)[key]?.ToString() ?? string.Empty));
          break;

        case "maxwait":
          DsTsetStart.Add(key, JsonConvert.DeserializeObject<dynamic>(((JToken)val)[key]?.ToString() ?? string.Empty));
          break;
      }
    }
  }
  #endregion
}
