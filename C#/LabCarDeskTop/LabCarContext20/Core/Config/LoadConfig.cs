using LabCarContext20.Data;
using System.IO;
using System.Text.Json.Serialization;

namespace LabCarContext20.Core.Config;

public interface ILoadConfig
{
  string DirConfig { get; set; }
  void ConfigLoad(string pathDir);
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

      [Newtonsoft.Json.JsonConstructor]
      public TaskJsonLoad(string pathTask, string timeLabCar, string comment)
      {
        PathTask = pathTask;
        TimeLabCar = timeLabCar;
        Comment = comment;
      }

    }
    public class ParameterJson
    {
      [JsonPropertyName("Signal")]
      public string Signal { get; }
      [JsonPropertyName("Comment")]
      public string Comment { get; }

      [Newtonsoft.Json.JsonConstructor]
      public ParameterJson(string signal, string comment = "")
      {
        Signal = signal;
        Comment = comment;
      }

    }

    public class CalibrationsJson
    {
      [JsonPropertyName("Signal")]
      public string Signal { get; }
      [JsonPropertyName("Val")]
      public dynamic Val { get; }
      [JsonPropertyName("Comment")]
      public string Comment { get; }

      [Newtonsoft.Json.JsonIgnore]
      public string Text => $"FESTWERT {Signal} \n  WERT {((string)Val.ToString()).Replace(',', '.')} \nEND\n";

      [Newtonsoft.Json.JsonConstructor]
      public CalibrationsJson(string signal, dynamic val, string comment = "")
      {
        Signal = signal;
        Val = (val is string) ? 0.0 : val;
        Comment = comment;
      }
    }

    public GlobalConfigLabCar()
    {
    }
    public Dictionary<string, string> PathLabCar { get; set; } = new Dictionary<string, string>();
    public Dictionary<string, TaskJsonLoad> LabCarTask { get; set; } = new Dictionary<string, TaskJsonLoad>();
    public Dictionary<string, ParameterJson> Parameters = new Dictionary<string, ParameterJson>();
    public Dictionary<string, Dictionary<string, CalibrationsJson>> Calibration = new Dictionary<string, Dictionary<string, CalibrationsJson>>();
  }

  #endregion
  #region data
  public string DirConfig { get; set; } = "";
  public GlobalConfigLabCar? Config { get; set; }
  private readonly DanValue _danValue;
  private readonly DanWriteLc _danWriteLc;
  private readonly IDanCalibrations2 _idanCalibrations2;
  private readonly IAllDan _iallDan;
  private ICPathLc icPathLc;
  private ContainerManager? _container = null;

  #endregion
  public LoadConfig(ICPathLc icpathlc, 
                      DanValue danValue, 
                      DanWriteLc danWriteLc, 
                      IDanCalibrations2 idanCalibrations2, 
                      IAllDan iallDan)
  {
    icPathLc = icpathlc;
    _danValue = danValue;
    _danWriteLc = danWriteLc;
    _idanCalibrations2 = idanCalibrations2;
    _iallDan = iallDan;
    _container = ContainerManager.GetInstance();
  }
  public void ConfigLoad(string pathDir)
  {
    string _pathConfig = pathDir + "\\Config.json";

    if (!File.Exists(_pathConfig))
      throw new MyException($" Нет файлов Config.json ", -1);

    try
    {
      Config = JsonConvert.DeserializeObject<GlobalConfigLabCar>(File.ReadAllText(_pathConfig));
    }
    catch (Exception e)
    {
      throw new MyException($" Проблема с файлом Config.json -> {_pathConfig} \n {e}  ", -1);
    }

    if (Config.PathLabCar.Count()>0)
    {
      icPathLc.Workspace = Config.PathLabCar.TryGetValue("Workspace", out var outWorkspace) ? outWorkspace : "";
      icPathLc.Experiment = Config.PathLabCar.TryGetValue("Experiment", out var outExperiment) ? outExperiment : "";
    }

    foreach (var it in Config.LabCarTask)
    {
      CReadLc _cr = _container.LabCar.Resolve<CReadLc>();
      _cr.Initialization(it.Key, it.Value.PathTask, it.Value.TimeLabCar, it.Value.Comment);
      _iallDan.Add<CReadLc>(it.Key, _cr);

    }

    int kkk = 1;
    //  Загрузка class в контейнере


  }
}

/*
     public Dictionary<string, TaskJsonLoad> LabCarTask { get; set; } = new Dictionary<string, TaskJsonLoad>();
    public Dictionary<string, ParameterJson> Parameters = new Dictionary<string, ParameterJson>();
    public Dictionary<string, Dictionary<string, CalibrationsJson>> Calibration = new Dictionary<string, Dictionary<string, CalibrationsJson>>();

 
 */
