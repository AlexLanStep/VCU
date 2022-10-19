

using ETAS.EE.Scripting;
using System;
using System.Diagnostics.Metrics;

namespace ContextLabCar.Core.Strategies;

public interface IStrategiesBasa
{
  IConnectLabCar IConLabCar { get; set; }
  void Run(string pathdir = "");

}

public class StrategiesBasa : StrategDanJson, IStrategiesBasa
{
  public StrategiesBasa(IConnectLabCar iConLabCar)
  {
    IConLabCar = iConLabCar;
  }

  public IConnectLabCar IConLabCar { get; set; }

  public void Run(string pathdir = "")
  {
    InicialJson(pathdir);
    if (!(DPath.TryGetValue("Workspace", out string vwork) && DPath.TryGetValue("Experiment", out string vexpe)))
      new MyException(" Error in json path Workspace or Experiment ", -5);

    IConLabCar.Inicial(vwork, vexpe);
  }
}


public interface ITestLabCar
{
  IConnectLabCar IConLabCar { get; set; }
  void Run(string s);
  string Name { get; set; }
}

public class TestLabCar : ITestLabCar
{
  public TestLabCar(IConnectLabCar iConLabCar)
  {
    IConLabCar = iConLabCar;
  }

  public IConnectLabCar IConLabCar { get; set; }

  public string Name { get; set; }

  public void Run(string s)
  {
    Name = s;
  }
}
