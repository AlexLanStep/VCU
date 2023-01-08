
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
      [Newtonsoft.Json.JsonConstructor]
      public ParameterJson(string signal, string comment)
      {
        Signal = signal;
        Comment = comment;
      }

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

    public Dictionary<string, string> PathLabCar { get; set; } = new Dictionary<string, string>();
    public Dictionary<string, TaskJsonLoad> LabCarTask { get; set; } = new Dictionary<string, TaskJsonLoad>();
    public Dictionary<string, ParameterJson> Parameters { get; set; } = new Dictionary<string, ParameterJson>();
    public Dictionary<string, Dictionary<string, Calibrations2Json>> Calibration { get; set; } = new Dictionary<string, Dictionary<string, Calibrations2Json>>();
    public Dictionary<string, VariableDataJson> Variable { get; set; } = new Dictionary<string, VariableDataJson>();
  }

  #endregion
  #region data
  #region Container
  private readonly ContainerManager? _container;
  private readonly ILoggerDisplay _iloggerdisplay;
  private readonly ICDopConfig _icdopConfig;
//  private readonly ILoadConfig _iloadConfig;
  private readonly IDanCalibrations2 _idanCalibrations2;
  private readonly IAllDan _iallDan; 
  private readonly IConnectLabCar _iconnectLabCar;
  private readonly ICPathLc _icPathLc;
  private readonly ICPaths? _icPaths;
  private readonly DanReadLc _danReadLc;
  private readonly DanValue _danValue;
  private readonly DanWriteLc _danWriteLc;
  #endregion


  public string DirConfig { get; set; } = "";
  public GlobalConfigLabCar? Config { get; set; }

  #endregion

  public LoadConfig()
  {
    _container = ContainerManager.GetInstance();

    _iconnectLabCar = _container.LabCar.Resolve<IConnectLabCar>();
    _iloggerdisplay = _container.LabCar.Resolve<ILoggerDisplay>();
    _idanCalibrations2 = _container.LabCar.Resolve<IDanCalibrations2>();
    _iallDan = _container.LabCar.Resolve<IAllDan>();
    _danReadLc = _container.LabCar.Resolve<DanReadLc>();
    _danValue = _container.LabCar.Resolve<DanValue>();
    _danWriteLc = _container.LabCar.Resolve<DanWriteLc>();
    _icPaths = _container.LabCar.Resolve<ICPaths>();
    _icPathLc = _container.LabCar.Resolve<ICPathLc>();
//    _iloadConfig = _container.LabCar.Resolve<ILoadConfig>();
    _icdopConfig = _container.LabCar.Resolve<ICDopConfig>();
  }
  public void ConfigLoad(string pathconfig="")
  {
    if (string.IsNullOrEmpty(pathconfig)) pathconfig = _icPaths?.GlConfig ?? string.Empty;

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

    if (Config != null && Config.PathLabCar.Count > 0)
    {
      _icPathLc.Workspace = Config.PathLabCar.TryGetValue("Workspace", out var outWorkspace) ? outWorkspace : "";
      _icPathLc.Experiment = Config.PathLabCar.TryGetValue("Experiment", out var outExperiment) ? outExperiment : "";
    }

#pragma warning disable CS8602
    foreach (var it in Config?.LabCarTask)
#pragma warning restore CS8602
      _danReadLc.Add(it.Key, _container?.LabCar.Resolve<CReadLc>()
        .Initialization(it.Key, it.Value.PathTask, it.Value.TimeLabCar, it.Value.Comment));
      
    foreach (var it in Config.Parameters)
    {
      var cw = _container?.LabCar.Resolve<CWriteLc>();
      cw?.Inicialization(it.Key, it.Value.Signal, it.Value.Comment);
      _danWriteLc.Add(it.Key, cw);
    }

    var iDanCalibr = _container?.LabCar.Resolve<IDanCalibrations2>();
    foreach (var it in Config.Calibration
               .Where(_ => _icPaths?.GlCalibr != null))
      iDanCalibr?.Add(it.Key, _container?.LabCar.Resolve<Calibrations2>()
#pragma warning disable CS8604
        .Initialization(_icPaths?.GlCalibr, it.Key, it.Value));
#pragma warning restore CS8604

    void LoadPathMatdan(string pathx)
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
        LoadPathMatdan(it.Value.Path);

      if (it.Value.Paths == null) continue;

      foreach (var it0 in it.Value.Paths)
        LoadPathMatdan(it0);
    }
  }

}
