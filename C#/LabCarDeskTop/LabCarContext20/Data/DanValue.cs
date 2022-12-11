
using LabCarContext20.Core.Config;
using LabCarContext20.Data.Interface;

namespace LabCarContext20.Data;


public interface IDanValue : IGetDan 
{
  void Run();
  void Set(string name, dynamic d);

}

public class DanValue : DanBase<dynamic>, IDanValue
{
  public DanValue() : base()
  {
  }

  public void Set(string name, object d) => base.Add(name, d);

}

