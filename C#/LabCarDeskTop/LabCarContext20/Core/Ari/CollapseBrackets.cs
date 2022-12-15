using System.IO;
using System.Runtime.Intrinsics.X86;

namespace LabCarContext20.Core.Ari;
public enum TypeCollapseBrakets { Logical, Matimatic};
public record DanCollapseBrakets
{
  public List<string> LBrakets = new();
  public Dictionary<string, string> DBrakets=new();
}

public class CollapseBrackets
{
  private TypeCollapseBrakets _tb;
  private readonly AriPattern _aPattern = new();
  public DanCollapseBrakets? CalcBrakets(TypeCollapseBrakets tb, string str)
  {
    _tb=tb;
    DanCollapseBrakets dcb = new();
    var _brec = _aPattern.IsBrakets(str);

    if (_brec.Item1) // скобки существуют
      return calcBrakets(str);

    // скобки не существуют
//    if ((_tb == TypeCollapseBrakets.Logical) && _aPattern.IsMultiDiv(str).Item1)
    if (_aPattern.IsErrorLogicAndMulti(str))
      return null;

    string _nameEnd = "end";
    if (_tb == TypeCollapseBrakets.Matimatic)
    {
      string[] s = str.Split('=');
      _nameEnd = s[0];
      str = s[1];
    }
    dcb.LBrakets.Add(_nameEnd);
    dcb.DBrakets.Add(_nameEnd, str);
    return dcb;
  }

  private DanCollapseBrakets? calcBrakets(string str)
  {
    string _nameEnd = "end";

    if(_tb == TypeCollapseBrakets.Matimatic)
    {
      string[] s = str.Split('=');
      _nameEnd = s[0];
      str = s[1];
    }

    DanCollapseBrakets? dcb = null;
    while (_aPattern.IsBrakets(str).Item1)
    {
      var st0 = str;
      var xScop = Brakets(str);

      if (xScop.Count <= 0)
        continue;

      var x0 = xScop.ElementAt(0);

      var ssx = str.Substring(x0.Item1, x0.Item2 - x0.Item1 + 1);
      string nameTreeX = Guid.NewGuid().ToString().Replace("-", "_");
      st0 = st0.Replace(ssx, nameTreeX);
      ssx = ssx.Replace("(", "").Replace(")", "");

      if(dcb == null)  dcb = new();

      dcb.LBrakets.Add(nameTreeX);
      if (dcb.DBrakets.ContainsKey(nameTreeX))
        dcb.DBrakets[nameTreeX] = ssx;
      else
      {
        if ((_tb == TypeCollapseBrakets.Logical) && _aPattern.IsErrorLogicAndMulti(str))
          return null;

        dcb.DBrakets.Add(nameTreeX, ssx);
      }

      xScop.RemoveAt(0);
      str = st0;
    }

    if((dcb == null) || ((_tb == TypeCollapseBrakets.Logical) && _aPattern.IsErrorLogicAndMulti(str)))
      return null;

    dcb.LBrakets.Add(_nameEnd);
    dcb.DBrakets.Add(_nameEnd, str);

    return dcb;
  }

  private List<(int, int)> Brakets(string str0)
  {
    List<(int, int)> xBrakets = new();

    var xl = _aPattern.BraketsLeftCount(str0);
    var xr = _aPattern.BraketsRiteCount(str0);

    while (xl.Count > 0)
    {
      var valL = xl[^1];
      var valR = xr.Where(x => x > valL).Min();
      xBrakets.Add((valL, valR));
      xl.RemoveAt(xl.Count - 1);
      xr.Remove(valR);
    }
    return xBrakets;
  }


}

