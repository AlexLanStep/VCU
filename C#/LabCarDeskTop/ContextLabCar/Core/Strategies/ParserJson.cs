


namespace ContextLabCar.Core.Strategies;
public class ParserJson
{
  #region ===> Data <===
  #region ==__ Public __==
  public Dictionary<string, Parameter> DParameter; // = new();
  public Dictionary<string, DanOutput> DDanOutput; // = new();
  public Dictionary<string, Dictionary<string, Calibrat>> DCalibrat; //= new();
  public Dictionary<string, string> DPath; // = new();
  public Dictionary<string, LTask> DTask; // = new();
  public Dictionary<string, dynamic> DSTParams; // = new();
  public Dictionary<string, dynamic> DSTsetStart; // = new();
  public List<StOneStep> LsStOneStep; // = new List<StOneStep>();

  #endregion
  #region ___ Local ___
  private readonly string _pathFiles;
  private ConcurrentDictionary<string, object> BasaParams = new();

  #endregion
  #endregion

  #region _ Constructor _
  public ParserJson(string pathFiles) => _pathFiles = pathFiles;
  #endregion

  #region ___ run _ convert _ json
  public string? LoadFileJson(string filejson) => !File.Exists(filejson) ? null : File.ReadAllText(filejson);
  public void Run()
  {
    var tsxtJson = LoadFileJson(_pathFiles);
    if (tsxtJson != null)
      ConvertJson(tsxtJson);
  }
  public virtual void AddNewPoleJson(ParserJson parserjson)
  {
    //    var x = parserjson.BasaParams;
  }

  protected virtual void ConvertJson(string tsxtJson)
  { 
  }
  #endregion

  #region function
  protected Dictionary<string, dynamic> jsonToDicStDyn(string name)=>
                (Dictionary<string, dynamic>)JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(name); //
  protected List<string> jsonLsString(string name) =>(List<string>)JsonConvert.DeserializeObject<List<string>>(name);

  protected Dictionary<string, string> jsonToDicStStr(string name) =>
                (Dictionary<string, string>)JsonConvert.DeserializeObject<Dictionary<string, string>>(name); //

  protected Dictionary<string, List<string>> jsonToDicStLsStr(string? name) =>
                (Dictionary<string, List<string>>)JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(name); //
  protected List<string>? JsonLsString(string name) => (List<string>)JsonConvert.DeserializeObject<List<string>>(name);

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

