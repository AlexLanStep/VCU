namespace LabCarContext20.Data;
public interface IDanReadLc : IGetDan //: IDanBase<T> where T : class
{
  void Run();
  bool Add(string nameTask, string pathTask, string timeLabCar, string comment = "");
}
public class DanDanReadLc :  DanBase<CReadLc>, IDanReadLc
{
  private readonly ContainerManager? _container;
  public DanDanReadLc(IConnectLabCar iconnectlc):base(iconnectlc) =>
    _container = ContainerManager.GetInstance();
  public override void Run()
  {
  }

  // ReSharper disable once RedundantOverriddenMember
  public override void Add(string name, CReadLc? tDan)
  {
    if (tDan != null) base.Add(name, tDan);
  }

  public  bool Add(string nameTask, string pathTask, string timeLabCar, string comment = "")
  {
    var readLc = _container?.LabCar.Resolve<CReadLc>().Initialization(pathTask, timeLabCar, comment);

    if (readLc?.Measurement == null)
      return false;

    Add(nameTask, readLc);
    return true;
  }

  public new dynamic? Get(string name) => ((CReadLc)GetT(name))?.Valume;
#pragma warning disable CS8603
  public new object GetT(string name) => base.GetT(name);
#pragma warning restore CS8603

}


