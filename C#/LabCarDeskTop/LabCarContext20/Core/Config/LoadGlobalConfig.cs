using System.IO;
using System.Text.Json.Serialization;

namespace LabCarContext20.Core.Config;

public interface ILoadGlobalConfig
{
  string DirGlobConfig { get; set; }
}

public class LoadGlobalConfig: ILoadGlobalConfig
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
  public string DirGlobConfig { get; set; } = "";
  public GlobalConfigLabCar? Config { get; set; }
  #endregion
  public LoadGlobalConfig()
  {

  }
  public void LoadConfig(string pathjson)
  {
    if (!File.Exists(pathjson))
      throw new MyException($" Нет такого пути к файлу {pathjson} ", -21);

    DirGlobConfig = Path.GetDirectoryName(pathjson);
    string _pathConfig = DirGlobConfig + "\\Config.json";

    if (!File.Exists(_pathConfig))
      throw new MyException($" Нет файлов Config.json ", -1);

    try
    {
      Config = JsonConvert.DeserializeObject<GlobalConfigLabCar>(File.ReadAllText(_pathConfig));
    }
    catch (Exception e)
    {
      Console.WriteLine(e);
      throw new MyException($" Проблема с файлом Config.json -> {_pathConfig}  ", -1);
    }
    int kkk = 1;
    //  Загрузка class в контейнере

  }
}


