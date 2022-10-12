
namespace ContextLabCar.Static;

public class ContainerManager
{
  private static readonly Lazy<ContainerManager> Lazy = new Lazy<ContainerManager>(() => new ContainerManager());
  private ContainerManager()
  {
//    LabCarContainer = new DryCont();              // new DryIoc.Container();

    //DbContainer.Register<IParsingXml, ParsingXml>(Reuse.Singleton);
    //DbContainer.Register<IConfigDb, ConfigDb>(Reuse.Singleton);
    //DbContainer.Register<IContext, Context>(Reuse.Singleton);
    //DbContainer.Register<IProcessingDb, ProcessingDb>();
    //DbContainer.Register<ICurrentConfig, CurrentConfig>(Reuse.Singleton);
    //DbContainer.Register<IHistoryConfigFull, HistoryConfigFull>();

  }

  public static ContainerManager GetInstance() => Lazy.Value;

//  public DryCont LabCarContainer;
}
