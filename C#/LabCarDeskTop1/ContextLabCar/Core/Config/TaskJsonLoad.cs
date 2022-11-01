
using System.Text.Json.Serialization;

namespace ContextLabCar.Core.Config;

public class TaskJsonLoad
{
  [JsonPropertyName("PathTask")]
  public string PathTask { get; set; }
  [JsonPropertyName("TimeLabCar")]
  public string TimeLabCar { get; set; }
  [JsonPropertyName("Comment")]
  public string Comment { get; set; }

}
