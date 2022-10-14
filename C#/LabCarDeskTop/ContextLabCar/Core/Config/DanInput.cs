using ETAS.EE.Scripting;
using System.Xml.Linq;

namespace ContextLabCar.Core.Config;
public class DanInput : IDanInOut
{
  public string Signal { get;}
  public string Comment { get;}

  public DanInput(string nameModel, string name, string comment="")
  {
    Comment = comment;
    Signal = $"{nameModel}/{name}/Value";
  }
}

