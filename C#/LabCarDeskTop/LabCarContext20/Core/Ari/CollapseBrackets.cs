using DryIoc.FastExpressionCompiler.LightExpression;
using ETAS.EE.Scripting;
using System.IO;
using System.Runtime.Intrinsics.X86;

namespace LabCarContext20.Core.Ari;
public enum TypeCollapseBrakets { Logical, Matimatic, NotCalc};
public record DanCollapseBrakets<T>
{
  public List<string> LBrakets = new();
  public Dictionary<string, T> DBrakets=new();
}

public class CollapseBrackets
{
  private TypeCollapseBrakets _tb;
  private readonly AriPattern _aPattern = new();
  public DanCollapseBrakets<T>? CalcBrakets<T>(string str) where T : ICVariableBasa, new()
  {
    _tb = (typeof(T).Name) switch {
      "CVariable" => TypeCollapseBrakets.Matimatic,
      "CVariableLogic" => TypeCollapseBrakets.Logical,
      _ => TypeCollapseBrakets.NotCalc
      };

    DanCollapseBrakets<T> dcb = new();
    var _brec = _aPattern.IsBrakets(str);

    if (_brec.Item1) // скобки существуют
      return calcBrakets<T>(str);

    // скобки не существуют
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
    var _x00 = new T();
    _x00.Instal(_nameEnd, str);
    dcb.DBrakets.Add(_nameEnd, _x00);

    return dcb;
  }

  private DanCollapseBrakets<T>? calcBrakets<T>(string str) where T : ICVariableBasa, new()
  {
    string _nameEnd = "end";

    if(_tb == TypeCollapseBrakets.Matimatic)
    {
      string[] s = str.Split('=');
      _nameEnd = s[0];
      str = s[1];
    }

    DanCollapseBrakets<T>? dcb = null;
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
      { 
        var _x0 =  dcb.DBrakets[nameTreeX]; 
        ((ICVariableBasa) _x0).StrComand = ssx;
        dcb.DBrakets[nameTreeX] = _x0;
      }
      else
      {
        if ((_tb == TypeCollapseBrakets.Logical) && _aPattern.IsErrorLogicAndMulti(ssx))
          return null;

        var _x01 = new T();
        _x01.Instal(_nameEnd, str);
        dcb.DBrakets.Add(_nameEnd, _x01);
      }

      xScop.RemoveAt(0);
      str = st0;
    }

    if((dcb == null) || ((_tb == TypeCollapseBrakets.Logical) && _aPattern.IsErrorLogicAndMulti(str)))
      return null;

    dcb.LBrakets.Add(_nameEnd);
//    dcb.DBrakets.Add(_nameEnd, str);
    var _x00 = new T();
    _x00.Instal(_nameEnd, str);
    dcb.DBrakets.Add(_nameEnd, _x00);

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

