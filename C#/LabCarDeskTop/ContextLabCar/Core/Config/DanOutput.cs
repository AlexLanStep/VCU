
using System.Text.Json.Serialization;

namespace ContextLabCar.Core.Config;

public class DanOutput : IParameter
{
  public string Signal { get; }
  public string Comment { get; }

  public DanOutput(string nameModel, string name, string comment="")
  {
    Comment = comment;
    Signal = $"{nameModel}/{name}";
  }
}

public class DanOutputNew : IParameter
{
  [JsonPropertyName("Signal")]
  public string Signal { get; }
  [JsonPropertyName("Comment")]
  public string Comment { get; }

  public DanOutputNew(string signal, string comment = "")
  {
    Signal = signal;
    Comment = comment;
  }
}
