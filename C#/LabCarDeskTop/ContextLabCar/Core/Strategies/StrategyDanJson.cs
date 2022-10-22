

namespace ContextLabCar.Core.Strategies;

public interface IStrategyDanJson
{
  void InitializationJson(string pathdir = "");
  Dictionary<string, Parameter> DParameter { get; set; }
  Dictionary<string, DanOutput> DDanOutput { get; set; }
  Dictionary<string, Dictionary<string, Calibrat>> DCalibrat { get; set; }
  Dictionary<string, string> DPath { get; set; }
  Dictionary<string, LTask> DTask { get; set; }
  Dictionary<string, dynamic> DstParams { get; set; }
  Dictionary<string, dynamic> DstSetStart { get; set; }
  List<StOneStep> LsStOneStep { get; set; }
}
public class StrategyDanJson: IStrategyDanJson
{
  #region ===> Data <===
  #region ==__ Public __==
  public Dictionary<string, Parameter> DParameter { get; set; }
  public Dictionary<string, DanOutput> DDanOutput { get; set; }
  public Dictionary<string, Dictionary<string, Calibrat>> DCalibrat { get; set; }
  public Dictionary<string, string> DPath { get; set; }
  public Dictionary<string, LTask> DTask { get; set; }
  public Dictionary<string, dynamic> DstParams { get; set; }
  public Dictionary<string, dynamic> DstSetStart { get; set; }
  public List<StOneStep> LsStOneStep { get; set; }

  #endregion
  #region ___ Local ___
  private string _pathdir;
  #endregion
  #endregion
  
  public StrategyDanJson(string pathdir = "")
  {
    _pathdir = pathdir;
    DParameter = new Dictionary<string, Parameter>();
    DDanOutput = new Dictionary<string, DanOutput>();
    DCalibrat = new Dictionary<string, Dictionary<string, Calibrat>>();
    DPath = new Dictionary<string, string>();
    DTask = new Dictionary<string, LTask>();
    DstParams = new Dictionary<string, dynamic>();
    DstSetStart = new Dictionary<string, dynamic>();
    LsStOneStep = new List<StOneStep>();
  }

public void InitializationJson(string pathdir = "")
  {
    if (string.IsNullOrEmpty(pathdir) && string.IsNullOrEmpty(_pathdir))
      throw new MyException("Not path for JSON", -2);

    _pathdir = !string.IsNullOrEmpty(pathdir) ? pathdir : _pathdir;
    
    if( !Directory.Exists(_pathdir))
      throw new MyException("Not dir strateg", -3);

    var paths = Directory.GetFiles(_pathdir, "*.json").Select(x=> x.ToLower()).ToArray();

    foreach (var path in paths)
    {
      if (path.Contains("strateg.json"))
      {
        var st = new ParserJsonSt(path);
        st.Run();
        DstParams = st.DstParams;
        DstSetStart = st.DsTsetStart;
        LsStOneStep = st.LsStOneStep;
      }

      if (!path.Contains("inicialparams.json")) continue;
      var stdan = new ParserJsonDan(path);
      stdan.Run();
      DParameter = stdan.DParameter;
      DDanOutput = stdan.DDanOutput;
      DCalibrat = stdan.DCalibrat;
      DPath = stdan.DPath;
      DTask = stdan.DTask;
    }
  }

}

