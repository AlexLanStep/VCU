
using LabCarContext20.Core.Config;
using LabCarContext20.Data.Interface;
using LabCarContext20.Static;

namespace LabCarContext20.Data;

public interface IDanReadLc : IGetDan //: IDanBase<T> where T : class
{
  void Run();
  bool Add(string nameTask, string pathTask, string timeLabCar, string comment = "");

}

public class DanDanReadLc :  DanBase<CReadLc>, IDanReadLc
{
  public DanDanReadLc(IConnectLabCar iconnectlc):base(iconnectlc)
  {
  }
  public override void Run()
  {
    int i = 1;
  }

  public override void Add(string name, CReadLc? tDan) => base.Add(name, tDan);

  public  bool Add(string nameTask, string pathTask, string timeLabCar, string comment = "")
  {
    var task = new CReadLc(iConnectLC, pathTask, timeLabCar, comment);
    if (task.Measurement != null)
    {
      Add(nameTask, task);
      return true;
    }
    else
      return false;
  }

  public dynamic? Get(string name) => ((CReadLc)GetT(name))?.Valume;
  public object GetT(string name) => base.GetT(name);

}


