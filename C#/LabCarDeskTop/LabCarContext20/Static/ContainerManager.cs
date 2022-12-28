

using LabCarContext20.Core.Strategies;

namespace LabCarContext20.Static;

public class ContainerManager
{
  private static readonly Lazy<ContainerManager> Lazy = new Lazy<ContainerManager>(() => new ContainerManager());
  private ContainerManager()
  {
    // ReSharper disable once RedundantNameQualifier
    LabCar = new DryIoc.Container();              // new DryIoc.Container();
    LabCar.Register<ICPaths, CPaths>(Reuse.Singleton);
    LabCar.Register<ICReport, CReport>(Reuse.Singleton);
    LabCar.Register<ICReportLocal, CReportLocal>();
    LabCar.Register<ICDopConfig,CDopConfig>(Reuse.Singleton);
    LabCar.Register<ILoggerDisplay, LoggerDisplay>(Reuse.Singleton);
    LabCar.Register<ICPathLc, CPathLc>(Reuse.Singleton);
    LabCar.Register<IConnectLabCar, ConnectLabCar>(Reuse.Singleton);
    LabCar.Register<AriPattern>();
//    LabCar.Register<IAriCalcOnStr, AriCalcOnStr>();

    LabCar.Register<CReadLc>();
    LabCar.Register<CWriteLc>();
    LabCar.Register<AriStrDisassemble>();
    LabCar.Register<DanReadLc>(Reuse.Singleton);
    LabCar.Register<DanValue>(Reuse.Singleton);
    LabCar.Register<DanWriteLc>(Reuse.Singleton);
    LabCar.Register<DanLoggerLc>(Reuse.Singleton);
    LabCar.Register<Calibrations2, Calibrations2>();
    LabCar.Register<ICalibrations2, Calibrations2>();
    LabCar.Register<IDanCalibrations2, DanCalibrations2>(Reuse.Singleton);
    LabCar.Register<IAllDan, AllDan>(Reuse.Singleton);
    LabCar.Register<ILoadConfig, LoadConfig>();
    LabCar.Register<StrateyBasa>();
    LabCar.Register<IStSetOneDan, StSetOneDan>();
    LabCar.Register<IStIfOne, StIfOne>();
    LabCar.Register<IGeneralStrategy, GeneralStrategy>();
  }

  public static void Initialization()
  {
    ContainerManager? _container = ContainerManager.GetInstance();
    var _p1 = _container.LabCar.Resolve<ICPaths>();
    var _p2 = _container.LabCar.Resolve<ICReport>();
    var _p3 = _container.LabCar.Resolve<ICReportLocal>();
    var _p31 = _container.LabCar.Resolve<ICDopConfig>();

    var _p4 = _container.LabCar.Resolve<ILoggerDisplay>();
    var _p5 = _container.LabCar.Resolve<IConnectLabCar>();
    var _p6 = _container.LabCar.Resolve<AriPattern>();
    var _p7 = _container.LabCar.Resolve<AriStrDisassemble>();
    var _p8 = _container.LabCar.Resolve<CReadLc>();
    var _p9 = _container.LabCar.Resolve<CWriteLc>();
    var _p10 = _container.LabCar.Resolve<DanReadLc>();
    var _p11 = _container.LabCar.Resolve<DanValue>();
    var _p12 = _container.LabCar.Resolve<DanWriteLc>();
    var _p13 = _container.LabCar.Resolve<DanLoggerLc>();
    var _p14 = _container.LabCar.Resolve<ICalibrations2>();
    var _p15 = _container.LabCar.Resolve<IDanCalibrations2>();
    var _p16 = _container.LabCar.Resolve<IAllDan>();
    var _p17 = _container.LabCar.Resolve<ILoadConfig>();
    var _p18 = _container.LabCar.Resolve<IStSetOneDan>();
    var _p19 = _container.LabCar.Resolve<IStIfOne>();
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
