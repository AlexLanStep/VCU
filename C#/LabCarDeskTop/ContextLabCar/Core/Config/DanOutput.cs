
namespace ContextLabCar.Core.Config;


public class DanOutput : IDanInOut
{
  public string Signal { get; }
  public string Comment { get; }

  public DanOutput(string nameModel, string name, string comment="")
  {
    Comment = comment;
    Signal = $"{nameModel}/{name}";
  }



}
