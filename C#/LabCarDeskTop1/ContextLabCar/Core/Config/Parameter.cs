
using System.Text.Json.Serialization;

namespace ContextLabCar.Core.Config;
public class Parameter : IParameter
{
  public string Signal { get;}
  public string Comment { get;}

  public Parameter(string nameModel, string name, string comment="")
  {
    Comment = comment;
    Signal = $"{nameModel}/{name}/Value";
  }
}

public class ParameterNew : IParameter
{
  [JsonPropertyName("Signal")]
  public string Signal { get; }
  [JsonPropertyName("Comment")]
  public string Comment { get; }

  public ParameterNew(string signal, string comment = "")
  {
    Signal = signal;
    Comment = comment;
  }
}
