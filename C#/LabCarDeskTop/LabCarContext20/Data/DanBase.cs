
namespace LabCarContext20.Data;

public class DanBase<T> : IDanBase<T> where T : class
{
  // ReSharper disable once InconsistentNaming
  protected IConnectLabCar _iConnectLC { get; set; }
  // ReSharper disable once InconsistentNaming
  protected ConcurrentDictionary<string, T> cDan { get; set; } = new ();
  public DanBase(IConnectLabCar iConnectLc) => _iConnectLC = iConnectLc;
#pragma warning disable CS8618
  public DanBase() { }
#pragma warning restore CS8618

  public virtual void Add(string name, T tDan)
    => cDan.AddOrUpdate(name, tDan, (_, _) => tDan);
  
  public virtual T? GetT(string name)
    => cDan.TryGetValue(name, out var val) ? val : null;

  public dynamic? Get(string name)
    => cDan.TryGetValue(name, out var val) ? val : null;
  
  public virtual void Run()
  {
  }





}