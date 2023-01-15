
namespace LabCarContext20.Core.Config;

public interface ICPathLc
{
  string Workspace { get; set; }
  string Experiment { get; set; }
}
public class CPathLc: ICPathLc
{
  public string Workspace { get; set; } = "";
  public string Experiment { get; set; } = "";
}


