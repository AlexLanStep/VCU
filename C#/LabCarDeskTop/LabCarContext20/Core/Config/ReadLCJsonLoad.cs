using System.Text.Json.Serialization;

namespace LabCarContext20.Core.Config;

public class ReadLcJsonLoad
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
}

