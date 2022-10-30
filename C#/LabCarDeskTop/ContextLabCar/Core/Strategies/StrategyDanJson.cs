

namespace ContextLabCar.Core.Strategies;

public interface IStrategyDanJson
{
  void InitializationJson(string pathdir = "");
//  Dictionary<string, Parameter> DParameter { get; set; }
  Dictionary<string, ParameterJson> DParameterNew { get; set; }
//  Dictionary<string, DanOutput> DDanOutput { get; set; }
  Dictionary<string, DanOutputNew> DDanOutputNew { get; set; }
//  Dictionary<string, Dictionary<string, Calibrat>> DCalibrat { get; set; }
//  Dictionary<string, Dictionary<string, CalibratJson>> DCalibration { get; set; }
  //  Dictionary<string, string> DPath { get; set; }
  //  Dictionary<string, LTask> DTask { get; set; }
  Dictionary<string, dynamic> DstParams { get; set; }
  Dictionary<string, dynamic> DstSetStart { get; set; }
  List<StOneStep> LsStOneStep { get; set; }
  Dictionary<string, dynamic> DParamsStrategy { get; set; }
  Dictionary<string, CalibrationParams> DCalibrationParams { get; set; }


}
public class StrategyDanJson: IStrategyDanJson 
{
  #region ===> Data <===
  #region ==__ Public __==
  public Dictionary<string, ParameterJson> DParameterNew { get; set; }
  public Dictionary<string, DanOutputNew> DDanOutputNew { get; set; }
  public Dictionary<string, LabCarTaskName> DTask { get; set; }
  public Dictionary<string, dynamic> DstParams { get; set; }
  public Dictionary<string, dynamic> DstSetStart { get; set; }
  public List<StOneStep> LsStOneStep { get; set; }
  public Dictionary<string, string> DConfig { get; set; }
  public Dictionary<string, dynamic> DParamsStrategy { get; set; }
  public Dictionary<string, CalibrationParams> DCalibrationParams { get; set; }

  #endregion
  #region ___ Local ___
  private string _pathdir;
  #endregion
  #endregion
  
  public StrategyDanJson(string pathdir = "")
  {
    _pathdir = pathdir;
    DConfig = new Dictionary<string, string>();
    DstParams = new Dictionary<string, dynamic>();
    DstSetStart = new Dictionary<string, dynamic>();
    LsStOneStep = new List<StOneStep>();
    DParamsStrategy = new Dictionary<string, dynamic>();
    DParameterNew = new Dictionary<string, ParameterJson>();
    DDanOutputNew = new Dictionary<string, DanOutputNew>();
    DCalibrationParams = new Dictionary<string, CalibrationParams>();
  }

  public void InitializationJson(string pathdir = "")
  {
    void JsonConfigLoad(string path)
    {
      if (!File.Exists(path)) return;

      var config = JsonConvert.DeserializeObject<GlobalConfigLabCar>(File.ReadAllText(path));

      if (config == null) return;

      DConfig = config.PathLabCar;
      DTask = config.LabCarTask;
    }

    if (pathdir == null) return;

    if (string.IsNullOrEmpty(pathdir) && string.IsNullOrEmpty(_pathdir))
      throw new MyException("Not path for JSON", -2);

        _pathdir = pathdir; //!string.IsNullOrEmpty(pathdir) ? pathdir : _pathdir;

    if ( !Directory.Exists(_pathdir))
      throw new MyException("Not dir strateg", -3);

    JsonConfigLoad(pathdir.ToLower().Split("strategies")[0] + "\\Config.json");
    JsonConfigLoad(_pathdir + "\\Config.json");

    var _pathParams = pathdir + "\\Params.json";
    if (File.Exists(_pathParams))
    {
      var _loadParams = JsonConvert.DeserializeObject<LoadJsonParams>(File.ReadAllText(_pathParams));
      if (_loadParams != null)
      {
        DParamsStrategy = _loadParams.ParamsStrategy;
        DParameterNew = _loadParams.Parameters;
        DDanOutputNew = _loadParams.Output;
        DCalibrationParams = _loadParams.Calibration;
        WriteCalibrationDan();
      }
    }

    var _pathStrategy = pathdir + "\\strateg.json";
    if (!File.Exists(_pathStrategy)) return;

    var st = new ParserJsonSt(_pathStrategy);
    st.Run();
    DstParams = st.DstParams;
    DstSetStart = st.DsTsetStart;
    LsStOneStep = st.LsStOneStep;
  }

  private void WriteCalibrationDan()
  {
    foreach (var (key, val) in DCalibrationParams)
    {
      var text = "";
      var vCalibration = val.Parameter;
      foreach (var (_, vald) in vCalibration)
        text += vald.Text;

      File.WriteAllText(val.PathFiles, text);
    }
  }
}

