

using LabCarContext20.Core.Strategies;

namespace LabCarContext20.Static;

public class ContainerManager
{
  private static readonly Lazy<ContainerManager> Lazy = new Lazy<ContainerManager>(() => new ContainerManager());
  private ContainerManager()
  {
    // ReSharper disable once RedundantNameQualifier
    LabCar = new DryIoc.Container();              // new DryIoc.Container();

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
    LabCar.Register<ICalibrations2, Calibrations2>();
    LabCar.Register<IDanCalibrations2, DanCalibrations2>(Reuse.Singleton);
    LabCar.Register<IAllDan, AllDan>(Reuse.Singleton);
    LabCar.Register<ILoadConfig, LoadConfig>(); 
    LabCar.Register<IStSetOneDan, StSetOneDan>();
    LabCar.Register<IStIfOne, StIfOne>();
  }

  public static void Initialization()
  {
    ContainerManager? _container = ContainerManager.GetInstance();
    ILoggerDisplay _iloggerDisplay = _container.LabCar.Resolve<ILoggerDisplay>();
    IConnectLabCar _iconnect = _container.LabCar.Resolve<IConnectLabCar>();
    AriPattern _pattern = _container.LabCar.Resolve<AriPattern>();
    //    IAriCalcOnStr _IAriCalcOnStr = _container.LabCar.Resolve<IAriCalcOnStr>();
    AriStrDisassemble _ariStrDisassemble = _container.LabCar.Resolve<AriStrDisassemble>();
    CReadLc _creadLc = _container.LabCar.Resolve<CReadLc>();
    CWriteLc _cwriteLc = _container.LabCar.Resolve<CWriteLc>();
    DanReadLc _DanReadLc = _container.LabCar.Resolve<DanReadLc>();
    DanValue _danValue = _container.LabCar.Resolve<DanValue>();
    DanWriteLc _danWriteLc = _container.LabCar.Resolve<DanWriteLc>();
    DanLoggerLc _idanLoggerLc = _container.LabCar.Resolve<DanLoggerLc>();
    ICalibrations2 _iCalibrations2 = _container.LabCar.Resolve<ICalibrations2>();
    IDanCalibrations2 _iDanCalibrations2 = _container.LabCar.Resolve<IDanCalibrations2>();
    IAllDan _iAllDan = _container.LabCar.Resolve<IAllDan>();
    ILoadConfig _iloadConfig = _container.LabCar.Resolve<ILoadConfig>();
    IStSetOneDan _iStSetOneDan = _container.LabCar.Resolve<IStSetOneDan>();
    IStIfOne _iStIfOne = _container.LabCar.Resolve<IStIfOne>();
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
