using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContextLabCar.Core;

public interface IArElement
{
  string Name { get; set; }
  string StrCommand { get; set; }
  dynamic? Volume { get; set; }
  ArElement? PrivElement { get; set; }
  ArElement? NextElement { get; set; }
}
public class ArElement : IArElement
{
  public ArElement(string name, string strCommand, ArElement privElement)
  {
    Name = name;
    StrCommand = strCommand;
    PrivElement = privElement;
    Volume = null;
    NextElement = null;
    DElement = new Dictionary<string, ArElement>();
  }
  public Dictionary<string, ArElement> DElement { get; set; }
  public string Name { get; set; }
  public string StrCommand { get; set; }
  public dynamic Volume { get; set; }
  public ArElement? PrivElement { get; set; }
  public ArElement? NextElement { get; set; }
}
