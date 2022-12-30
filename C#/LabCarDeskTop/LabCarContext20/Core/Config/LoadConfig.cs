
using LabCarContext20.Data.Interface;

namespace LabCarContext20.Core.Config;

public interface ILoadConfig
{
  string DirConfig { get; set; }
  void ConfigLoad(string pathconfig="");

}

public class LoadConfig: ILoadConfig
{
  #region class config

  public class GlobalConfigLabCar
  {
    public class TaskJsonLoad
    {
      [JsonPropertyName("PathTask")]
#pragma warning disable CS8618
      public string PathTask { get; set; }
#pragma warning restore CS8618
      [JsonPropertyName("TimeLabCar")]
#pragma warning disable CS8618
      public string TimeLabCar { get; set; }
#pragma warning restore CS8618
      [JsonPropertyName("Comment")]
#pragma warning disable CS8618
      public string Comment { get; set; }
#pragma warning restore CS8618

      //[Newtonsoft.Json.JsonConstructor]
      //public TaskJsonLoad(string pathTask, string timeLabCar, string comment)
      //{
      //  PathTask = pathTask;
      //  TimeLabCar = timeLabCar;
      //  Comment = comment;
      //}

    }
    public class ParameterJson
    {
      [JsonPropertyName("Signal")]
      public string Signal { get; }
      [JsonPropertyName("Comment")]
      public string Comment { get; }

      //[Newtonsoft.Json.JsonConstructor]
      //public ParameterJson(string signal, string comment = "")
      //{
      //  Signal = signal;
      //  Comment = comment;
      //}

    }
    public class VariableDataJson
    {
      [JsonPropertyName("Value")]
      public dynamic? Value { get; set; }
      [JsonPropertyName("Path")]
      public string? Path { get; set; }
      [JsonPropertyName("Paths")]
      public List<string>? Paths { get; set; }

    }
    public GlobalConfigLabCar()
    {
    }
    public Dictionary<string, string> PathLabCar { get; set; } = new Dictionary<string, string>();
    public Dictionary<string, TaskJsonLoad> LabCarTask { get; set; } = new Dictionary<string, TaskJsonLoad>();
    public Dictionary<string, ParameterJson> Parameters { get; set; } = new Dictionary<string, ParameterJson>();
    public Dictionary<string, Dictionary<string, Calibrations2Json>> Calibration { get; set; } = new Dictionary<string, Dictionary<string, Calibrations2Json>>();
    public Dictionary<string, VariableDataJson> Variable { get; set; } = new Dictionary<string, VariableDataJson>();
  }

  #endregion
  #region data
  #region Container
  private ContainerManager? _container = null;
  private readonly ILoggerDisplay _iloggerdisplay;
  private readonly ICPathLc _icpathLc;
  private readonly ICPaths _icpaths;
  private readonly ICDopConfig _icdopConfig;
  private readonly ILoadConfig _iloadConfig;
  private readonly IConnectLabCar _iconnectLabCar;
  private readonly IDanCalibrations2 _idanCalibrations2;
  private readonly IAllDan _iallDan;
  private readonly ICPathLc icPathLc;
  private readonly ICPaths? _icPaths = null;
  private readonly DanReadLc _danReadLc;
  private readonly DanValue _danValue;
  private readonly DanWriteLc _danWriteLc;
  #endregion


  public string DirConfig { get; set; } = "";
  public GlobalConfigLabCar? Config { get; set; }

  /*
    #region Container
    private ContainerManager? _container = null;
    private readonly ILoggerDisplay _iloggerdisplay;
    private readonly ICPathLc _icpathLc;
    private readonly ICPaths _icpaths;
    private ICDopConfig _icdopConfig;
    private readonly ILoadConfig _iloadConfig;
    private readonly IConnectLabCar _iconnectLabCar;

    #endregion
    public string ParhStrategy { get; set; }
    public string NameDir { get; set; }

    private const string Stls = "STls";

    public GeneralStrategy()
    {
      _container = ContainerManager.GetInstance();

      _iconnectLabCar = _container.LabCar.Resolve<IConnectLabCar>();
      _iloggerdisplay = _container.LabCar.Resolve<ILoggerDisplay>();
      _icpathLc = _container.LabCar.Resolve<ICPathLc>();
      _icpaths = _container.LabCar.Resolve<ICPaths>();
      _icdopConfig = _container.LabCar.Resolve<ICDopConfig>();
      _iloadConfig = _container.LabCar.Resolve<ILoadConfig>();
    }


   */



  #endregion
//  public LoadConfig(ICPathLc icpathlc, ICPaths icPaths, IDanCalibrations2 idanCalibrations2, IAllDan iallDan)
  public LoadConfig()
  {
    _container = ContainerManager.GetInstance();

    _iconnectLabCar = _container.LabCar.Resolve<IConnectLabCar>();
    _iloggerdisplay = _container.LabCar.Resolve<ILoggerDisplay>();
    _icpathLc = _container.LabCar.Resolve<ICPathLc>();
    _icpaths = _container.LabCar.Resolve<ICPaths>();
    _idanCalibrations2 = _container.LabCar.Resolve<IDanCalibrations2>();
    _iallDan = _container.LabCar.Resolve<IAllDan>();
    _danReadLc = _container.LabCar.Resolve<DanReadLc>();
    _danValue = _container.LabCar.Resolve<DanValue>();
    _danWriteLc = _container.LabCar.Resolve<DanWriteLc>();


  //    _icdopConfig = _container.LabCar.Resolve<ICDopConfig>();
  //    _iloadConfig = _container.LabCar.Resolve<ILoadConfig>();
}
public void ConfigLoad(string pathconfig="")
  {
    if (string.IsNullOrEmpty(pathconfig)) pathconfig = _icPaths.GlConfig;

    if (string.IsNullOrEmpty(pathconfig))
      return;

    try
    {
      Config = JsonConvert.DeserializeObject<GlobalConfigLabCar>(File.ReadAllText(pathconfig));
    }
    catch (Exception e)
    {
      throw new MyException($" Проблема с файлом Config.json -> {pathconfig} \n {e}  ", -1);
    }

    if (Config.PathLabCar.Count > 0)
    {
      icPathLc.Workspace = Config.PathLabCar.TryGetValue("Workspace", out var outWorkspace) ? outWorkspace : "";
      icPathLc.Experiment = Config.PathLabCar.TryGetValue("Experiment", out var outExperiment) ? outExperiment : "";
    }

    foreach (var it in Config.LabCarTask)
//    {
      //CReadLc _cr = _container.LabCar.Resolve<CReadLc>();
      //_cr.Initialization(it.Key, it.Value.PathTask, it.Value.TimeLabCar, it.Value.Comment);
      //_iallDan.Add<CReadLc>(it.Key, _cr);
      
      _danReadLc.Add(it.Key, _container.LabCar.Resolve<CReadLc>()
        .Initialization(it.Key, it.Value.PathTask, it.Value.TimeLabCar, it.Value.Comment));
      
    foreach (var it in Config.Parameters)
    {
      CWriteLc _cw = _container.LabCar.Resolve<CWriteLc>();
      _cw.Initialization(it.Value.Signal, it.Value.Comment);
      _danWriteLc.Add(it.Key, _cw);
    }

    IDanCalibrations2 _iDanCalibr = _container.LabCar.Resolve<IDanCalibrations2>();
    foreach (var it in Config.Calibration)
//    {
      //      Calibrations2 _ccalibr = _container.LabCar.Resolve<Calibrations2>();
      //      _ccalibr.Initialization(_icPaths.GlCalibr, it.Key, it.Value);
      //      _iDanCalibr.Add(it.Key, _ccalibr);
      _iDanCalibr.Add(it.Key, _container.LabCar.Resolve<Calibrations2>().Initialization(_icPaths.GlCalibr, it.Key, it.Value));
//    }

    void loadPathMatdan(string pathx)
    {
      var xx = new MatLabConvert(pathx);
      if (xx.Dan.Count <= 0)
        return;

      foreach (var it in xx.Dan)
      _danValue.Add(it.Key, it.Value);
    }

    foreach (var it in Config.Variable)
    {
      if(it.Value.Value != null) 
        _danValue.Add(it.Key, it.Value.Value);

      if(it.Value.Path != null)
        loadPathMatdan(it.Value.Path);

      if (it.Value.Paths != null)
        foreach (var it0 in it.Value.Paths)
          loadPathMatdan(it0);
    }
  }

}
