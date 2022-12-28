
using DryIoc;
using System.Xml.Xsl;

namespace LabCarContext20.Core.Strategies;

public interface IStContext
{
  void ParserStrateg(List<JToken>? stBasa);
}

public class StContext: IStContext
{
  #region Conteiners
  protected ContainerManager? _container = null;
  protected readonly IAllDan _allDan;
  protected readonly ILoggerDisplay _ilogDisplay;
  #endregion

  protected List<JToken>? _stBasa = new();

  public StContext()
  {
    _container = ContainerManager.GetInstance();
    _allDan = _container.LabCar.Resolve<IAllDan>();
    _ilogDisplay = _container.LabCar.Resolve<ILoggerDisplay>(); 
  }

  public void ParserStrateg(List<JToken>? stBasa)
  {
    _stBasa = stBasa;
  }
}

