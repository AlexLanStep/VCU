
using System.ComponentModel.DataAnnotations;

namespace LabCarContext20.Core.Strategies;

public interface IStrategy
{
  bool Execute();
  void ParserStrateg(List<JToken>? stBasa);
  void SetParams(Dictionary<string, dynamic>? paramsStrategy);
}
public class StrateyBasa: StContext, IStrategy
{

  public StrateyBasa():base() 
  {
  }

  public bool Execute()
  {
    var _z = base.Execute();

    return true;
  }
  public void ParserStrateg(List<JToken>? stBasa)
  {
    base.ParserStrateg(stBasa);


  }
}
