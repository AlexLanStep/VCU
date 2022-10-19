

namespace ContextLabCar.Core.Strategies;

public interface IStrategDanJson
{
  void InicialJson(string pathdir = "");
  Dictionary<string, DanInput> DDanInput { get; set; }
  Dictionary<string, DanOutput> DDanOutput { get; set; }
  Dictionary<string, Dictionary<string, FestWert>> DFestWert { get; set; }
  Dictionary<string, string> DPath { get; set; }
  Dictionary<string, LTask> DTask { get; set; }
  Dictionary<string, dynamic> DSTParams { get; set; }
  Dictionary<string, dynamic> DSTsetStart { get; set; }
  List<StOneStep> LsStOneStep { get; set; }

}
public class StrategDanJson: IStrategDanJson
{
  #region ===> Data <===
  #region ==__ Public __==
  public Dictionary<string, DanInput> DDanInput { get; set; }
  public Dictionary<string, DanOutput> DDanOutput { get; set; }
  public Dictionary<string, Dictionary<string, FestWert>> DFestWert { get; set; }
  public Dictionary<string, string> DPath { get; set; }
  public Dictionary<string, LTask> DTask { get; set; }
  public Dictionary<string, dynamic> DSTParams { get; set; }
  public Dictionary<string, dynamic> DSTsetStart { get; set; }
  public List<StOneStep> LsStOneStep { get; set; }

  #endregion
  #region ___ Local ___
  private string _pathdir;
  private ParserJsonDan _parserDan;
  private ParserJsonST _parserSt;
  #endregion
  #endregion


  public StrategDanJson(string pathdir = "")
  {
    _pathdir = pathdir;
    DDanInput = new();
    DDanOutput = new();
    DFestWert = new();
    DPath = new();
    DTask = new();
    DSTParams = new();
    DSTsetStart = new();
    LsStOneStep = new();
  }


public void InicialJson(string pathdir = "")
  {
    if (string.IsNullOrEmpty(pathdir) && string.IsNullOrEmpty(_pathdir))
      new MyException("Not path for JSON", -2);

    _pathdir = !string.IsNullOrEmpty(pathdir) ? pathdir : _pathdir;
    
    if( !Directory.Exists(_pathdir))
      new MyException("Not dir strateg", -3);

    var paths = Directory.GetFiles(_pathdir, "*.json").Select(x=> x.ToLower()).ToArray();

    foreach (var path in paths)
    {
      if (path.Contains("strateg.json"))
      {
        var _st = new ParserJsonST(path);
        _st.Run();
        DSTParams = _st.DSTParams;
        DSTsetStart = _st.DSTsetStart;
        LsStOneStep = _st.LsStOneStep;
      }

      if (path.Contains("inicialparams.json"))
      {
        var _stdan = new ParserJsonDan(path);
        _stdan.Run();
        DDanInput = _stdan.DDanInput;
        DDanOutput = _stdan.DDanOutput;
        DFestWert = _stdan.DFestWert;
        DPath = _stdan.DPath;
        DTask = _stdan.DTask;
      }
    }
  }

}

