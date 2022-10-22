﻿
namespace ContextLabCar.Static;

public class ContainerManager
{
  private static readonly Lazy<ContainerManager> Lazy = new Lazy<ContainerManager>(() => new ContainerManager());
  private ContainerManager()
  {
    LabCar = new DryIoc.Container();              // new DryIoc.Container();

    LabCar.Register<IConnectLabCar, ConnectLabCar>(Reuse.Singleton);
    LabCar.Register<IStrategyDanJson, StrategyDanJson>();
    LabCar.Register <IStrategiesBasa, StrategiesBasa>();
     

    //DbContainer.Register<IParsingXml, ParsingXml>(Reuse.Singleton);
    //DbContainer.Register<IConfigDb, ConfigDb>(Reuse.Singleton);
    //DbContainer.Register<IContext, Context>(Reuse.Singleton);
    //DbContainer.Register<IProcessingDb, ProcessingDb>();
    //DbContainer.Register<ICurrentConfig, CurrentConfig>(Reuse.Singleton);
    //DbContainer.Register<IHistoryConfigFull, HistoryConfigFull>();

  }

  public static ContainerManager GetInstance() => Lazy.Value;

  public DryCont LabCar;
}
