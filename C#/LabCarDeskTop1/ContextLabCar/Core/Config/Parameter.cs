
namespace ContextLabCar.Core.Config;
public class Parameter : IParameter
{
  public string Signal { get;}
  public string Comment { get;}

  public Parameter(string nameModel, string name, string comment="")
  {
    Comment = comment;
    Signal = $"{nameModel}/{name}/Value";
  }
}

