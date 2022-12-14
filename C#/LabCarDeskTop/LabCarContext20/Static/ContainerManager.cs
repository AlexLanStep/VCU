

namespace LabCarContext20.Static;

public class ContainerManager
{
  private static readonly Lazy<ContainerManager> Lazy = new Lazy<ContainerManager>(() => new ContainerManager());
  private ContainerManager()
  {
    // ReSharper disable once RedundantNameQualifier
    LabCar = new DryIoc.Container();              // new DryIoc.Container();

    LabCar.Register<ILoggerDisplay, LoggerDisplay>(Reuse.Singleton);
    LabCar.Register<IConnectLabCar, ConnectLabCar>(Reuse.Singleton);
    LabCar.Register<IArithmetic, Arithmetic>();

    LabCar.Register<CReadLc>();
    LabCar.Register<CWriteLc>();

    LabCar.Register<DanDanReadLc>(Reuse.Singleton);
    LabCar.Register<DanValue>(Reuse.Singleton);
    LabCar.Register<DanWriteLc>(Reuse.Singleton);
    LabCar.Register<DanLoggerLc>(Reuse.Singleton);
    LabCar.Register<ICalibrations2, Calibrations2>();
    LabCar.Register<IDanCalibrations2, DanCalibrations2>(Reuse.Singleton);
    LabCar.Register<IAllDan, AllDan>(Reuse.Singleton);
  }

  public static ContainerManager GetInstance() => Lazy.Value;

  public DryCont LabCar;
}


//LabCar.Register<ITWriteWin, TWriteWin>();
//    LabCar.Register<IDanBase<CReadLC>, ReadLC>();

//    LabCar.Register<IDanBase<CReadLC>, ReadLC>();
//    LabCar.Register<IReadLC, ReadLC<CReadLC>>();


//    LabCar.Register<IReadLC, ReadLC<CReadLC>>(IConnectLabCar);
//    LabCar.Register<IReadLC, ReadLC> :  DanBase<CReadLC> where CReadLC: class, IReadLC
//    LabCar.Register<IBaseContext, BaseContext>();
//    LabCar.Register<IStOneStepNew, StOneStep>();
//    LabCar.Register<IArifmetic, Arifmetic>();
