
namespace LabCarContext20.Data;

public interface IDanValue : IGetDan 
{
  void Run();
  void Set(string name, dynamic d);

}

public class DanValue : DanBase<dynamic>, IDanValue
{
  // ReSharper disable once EmptyConstructor
  // ReSharper disable once RedundantBaseConstructorCall
  public DanValue() : base()
  {
  }

  public void Set(string name, object d) => base.Add(name, d);

}

