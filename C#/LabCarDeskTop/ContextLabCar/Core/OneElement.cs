using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ContextLabCar.Core;

public interface IOneElement
{
  Dictionary<string, OneElement> DElement { get; set; }
//  OneElement oneElement { get; set; }

  string Name { get; set; }
}
public class OneElement : IOneElement
{
  public string Name { get; set; }
  public string? NameValue { get; set; }
  public string? CommandAri { get; set; }
  public Dictionary<string, OneElement> DElement { get; set; }
  dynamic? Volume { get; set; }
  OneElement? PreviousElement { get; set; }
  OneElement? NextElement { get; set; }


  private string _strBasa;
  public List<string> Commands { get; set; }
  private readonly string _nameTree = "__#";
  private int _indexCom = 0;
  private CalcElement _calcElement;

  public OneElement(string _commandAri, string name=null)
  {
    DElement = new();

    _strBasa = _commandAri.Replace(" ", "");

    if(_strBasa.IndexOf("=") <0 && name == null)
      return;

    if (_strBasa.IndexOf("=") > 0)
    {
      var s0 = _strBasa.Split("=");
      NameValue = s0[0];
      CommandAri = s0[1];
      Name = "root";
    }
    else
    {
      NameValue = name;
      CommandAri = _strBasa;
      Name = name;
    }
    _calcElement = new(Name);
  }

  public void ConvertScobki(string _commandScobki)
  {
    _calcElement.CaclScobki(_commandScobki);
  }

}
