using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using DryIoc.ImTools;
using ContextLabCar.Static;

namespace ContextLabCar.Core;

public interface IArDeistv
{
  string? Name { get; set; }
  void SetStr(string str);
  string NameValue { get; set; }

}
public class ArDeistv : IArDeistv
{

  public string? Name { get; set; }
  public string NameValue { get; set; }

  private string _str0;
  private string _strBasa;
  public string CommandRoot { get; set; }
  public List<string> CommandList { get; set; }
  private readonly string _nameTree = "__#";
  private int _indexCom = 0;
  public ArElement arElement { get; set; }
  private Dictionary<string, ArElement> _arElements = new Dictionary<string, ArElement>();
  private List<(int, int)> xScop = new();

  public ArDeistv(string name)
  {
    _strBasa = name.Replace(" ", "");
    var s0 = _strBasa.Split("=");
    NameValue = s0[0];
    CommandRoot = s0[1];
    arElement = new ArElement("root", s0[1], null);
    CommandList = new() { CommandRoot };
  }

  public void SetStr(string str)
  {
    _str0 = str;
    
    if (StArithmetic.IsScobki(str).Item1)
    {
      List<(int, int)> xScop = StArithmetic.ScobkiX(_str0);
      int k = 0;
      while (k< xScop.Count - 1)
      {
        var x0 = xScop.ElementAt(k);
        var x1 = xScop.ElementAt(k+1);
        if(x0.Item2< x1.Item2)
        {
          string ssx = str.Substring(x0.Item1, x0.Item2- x0.Item1);
          string _namevolume = _nameTree + _indexCom;
          str = str.Replace(ssx, _namevolume);
          ssx.Replace("(", "").Replace(")","");
          _arElements.Add(_namevolume, new ArElement(_namevolume, ssx, arElement));
          k = xScop.Count;
        }
        k++;
      }
    }
  }


}
