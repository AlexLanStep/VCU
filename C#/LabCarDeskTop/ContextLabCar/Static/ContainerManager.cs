
namespace ContextLabCar.Static;

public class ContainerManager
{
  private static readonly Lazy<ContainerManager> Lazy = new Lazy<ContainerManager>(() => new ContainerManager());
  private ContainerManager()
  {
    // ReSharper disable once RedundantNameQualifier
    LabCar = new DryIoc.Container();              // new DryIoc.Container();

    LabCar.Register<IConnectLabCar, ConnectLabCar>(Reuse.Singleton);
    LabCar.Register<IBaseContext, BaseContext>();
    LabCar.Register<IStOneStepNew, StOneStep>();
  }

  public static ContainerManager GetInstance() => Lazy.Value;

  public DryCont LabCar;
}
