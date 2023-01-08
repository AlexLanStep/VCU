
namespace LabCarContext20.Data;

public class DanCalibrations2 : DanBase<Calibrations2>, IDanCalibrations2
{
  public DanCalibrations2(IConnectLabCar iConnectLc) : base(iConnectLc)
  {
  }

  public override void Add(string name, Calibrations2? calidr)
  {
    if (calidr != null) base.Add(name, calidr);
  }

  public bool Load(List<string> names)
  {
    if (names == null || names.Count == 0) return false;
    foreach (var name in names.Where(name => cDan.ContainsKey(name)))
      cDan[name].LoadingCalibrations();
    return true;
  }

  public bool Action(List<string> names)
  {
    if (names == null || names.Count == 0) return false;
    foreach (var name in names.Where(name => cDan.ContainsKey(name)))
      cDan[name].ActionCalibrations();
    return true;
  }

#pragma warning disable CS0108, CS0114, CS8603
  public object GetT(string name) => null;
#pragma warning restore CS0108, CS0114, CS8603
}





