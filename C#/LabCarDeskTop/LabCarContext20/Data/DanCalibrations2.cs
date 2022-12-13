
namespace LabCarContext20.Data;

public class DanCalibrations2: DanBase<Calibrations2>, IDanCalibrations2
{
  public DanCalibrations2(IConnectLabCar iConnectLc) : base(iConnectLc) { }

  public override void Add(string name, Calibrations2 calidr)=>base.Add(name, calidr);

  private Calibrations2 TestCalibrations2(string name, string ss)
  {
    var x = (Calibrations2)GetT(name);
    if (x == null)
      throw new MyException($" Error проблкма {ss} с файлам калибровки {name}", -3);
    return x;
  }

  public void Load(string name)=>TestCalibrations2(name, "звгрузкой").LoadingCalibrations();

  public void Action(string name) => TestCalibrations2(name, "активацией").ActionCalibrations();

#pragma warning disable CS0108, CS0114, CS8603
  public object GetT(string name) => null;
#pragma warning restore CS0108, CS0114, CS8603



}
