
#nullable enable
namespace ContextLabCar.Core.Strategies;
public class ParserJson
{
  #region ===> Data <===
  #region ==__ Public __==
  public Dictionary<string, Parameter> DParameter; 
  public Dictionary<string, DanOutput> DDanOutput; 
  public Dictionary<string, Dictionary<string, Calibrat>> DCalibrat; 
//  public Dictionary<string, string>? DPath; 
//  public Dictionary<string, LTask> DTask; 
  public Dictionary<string, dynamic>? DstParams;
  public Dictionary<string, dynamic?> DsTsetStart;
  public List<StOneStep> LsStOneStep; 

  #endregion
  #region ___ Local ___
  private readonly string _pathFiles;

  #endregion
  #endregion

  public IStrategiesBasa _istBasa;
  #region _ Constructor _
  public ParserJson(IStrategiesBasa istBasa, string pathFiles) 
  {
    _istBasa = istBasa;
    _pathFiles = pathFiles;
    DParameter = new Dictionary<string, Parameter>();
    DDanOutput = new Dictionary<string, DanOutput>();
    DCalibrat= new Dictionary<string, Dictionary<string, Calibrat>>();
//    DPath = new Dictionary<string, string>(); 
//    DTask = new Dictionary<string, LTask>(); 
    DstParams = new Dictionary<string, dynamic>(); 
    DsTsetStart = new Dictionary<string, dynamic?>();
    LsStOneStep = new List<StOneStep>();
  }
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
  protected Dictionary<string, dynamic>? JsonToDicStDyn(string name)=>
                JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(name); 
  protected List<string>? JsonLsString(string name) =>JsonConvert.DeserializeObject<List<string>>(name);

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

