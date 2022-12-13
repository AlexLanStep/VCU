using LabCarContext20.Data.Interface;

namespace LabCarContext20.Data;

public class DanBase<T> : IDanBase<T> where T : class
{
  public DanBase(IConnectLabCar iConnectLc)
  {
    iConnectLC = iConnectLc;
    cDan = new();
  }
  public DanBase()
  {
    cDan = new();
  }

  protected IConnectLabCar iConnectLC { get; set; }
  protected ConcurrentDictionary<string, T> cDan { get; set; }
  public virtual void Add(string name, T tDan)
    => cDan.AddOrUpdate(name, tDan, (s, lc) => tDan);
  
  public virtual T? GetT(string name)
    => cDan.TryGetValue(name, out var val) ? val : null;

  public dynamic? Get(string name)
    => cDan.TryGetValue(name, out var val) ? val : null;

   
  public virtual void Run()
  {
  }





}