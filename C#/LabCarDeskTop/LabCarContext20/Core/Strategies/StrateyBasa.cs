
namespace LabCarContext20.Core.Strategies;

public interface IStrategy
{
  bool Execute();
  void ParserStrateg(List<JToken>? stBasa);
}
public class StrateyBasa: StContext, IStrategy
{

  public StrateyBasa():base() 
  {
  }

  public bool Execute()
  {
    return true;
  }
  public void ParserStrateg(List<JToken>? stBasa)
  {
    base.ParserStrateg(stBasa);

  }
}
