
namespace LabCarContext20.Core;

public interface ICReport
{
  bool IsRezulta { get; set; }
  string TxtReport { get; set; }
  string DirRepost { get; set; }
  void Clear();
}
public class CReport: ICReport
{
  public bool IsRezulta { get; set; } = true;
  public string TxtReport { get; set; } = "";
  public string DirRepost { get; set; } = "";
  private readonly ICPaths _icPath = null;

  public CReport(ICPaths icPath)
  {
    _icPath = icPath;
    Clear();
  }
  public void Clear() 
  {
    IsRezulta = true;
    TxtReport = "";
    DirRepost = "";

  }
}
