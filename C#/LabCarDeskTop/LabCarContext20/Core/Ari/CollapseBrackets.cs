using LabCarContext20.Core.Ari.Interface;

namespace LabCarContext20.Core.Ari;
public enum TypeCollapseBrakets { Logical, Matimatic, NotCalc};
public record DanCollapseBraсkets<T>
{
  public List<string> LBrakets = new();
  public Dictionary<string, T> DBrakets=new();
}

public class CollapseBrackets
{
  private TypeCollapseBrakets _tb;
  private readonly AriPattern _aPattern = new();
  public DanCollapseBraсkets<T>? CalcBrakets<T>(string str) where T : ICVariableBase, new()
  {
    _tb = (typeof(T).Name) switch {
      "CVariable" => TypeCollapseBrakets.Matimatic,
      "CVariableLogic" => TypeCollapseBrakets.Logical,
      _ => TypeCollapseBrakets.NotCalc
      };

    DanCollapseBraсkets<T> dcb = new();
    var brec = _aPattern.IsBrakets(str);

    if (brec.Item1) // скобки существуют
      return CalcBraсkets<T>(str);

    // скобки не существуют
    if (_aPattern.IsErrorLogicAndMulti(str))
      return null;

    var nameEnd = "if";
    if (_tb == TypeCollapseBrakets.Matimatic)
    {
      var s = str.Split('=');
      nameEnd = s[0];
      str = s[1];
    }
    dcb.LBrakets.Add(nameEnd);
    var x00 = new T();
    x00.Instal(nameEnd, str);
    dcb.DBrakets.Add(nameEnd, x00);

    return dcb;
  }

  private DanCollapseBraсkets<T>? CalcBraсkets<T>(string str) where T : ICVariableBase, new()
  {
    var nameEnd = "if";

    if(_tb == TypeCollapseBrakets.Matimatic)
    {
      var s = str.Split('=');
      nameEnd = s[0];
      str = s[1];
    }

    DanCollapseBraсkets<T>? dcb = null;
    while (_aPattern.IsBrakets(str).Item1)
    {
      var st0 = str;
      var xScop = Braсkets(str);

      if (xScop.Count <= 0)
        continue;

      var x0 = xScop.ElementAt(0);

      var ssx = str.Substring(x0.Item1, x0.Item2 - x0.Item1 + 1);
      var nameTreeX = Guid.NewGuid().ToString().Replace("-", "_");
      st0 = st0.Replace(ssx, nameTreeX);
      ssx = ssx.Replace("(", "").Replace(")", "");

      dcb ??= new DanCollapseBraсkets<T>();

      dcb.LBrakets.Add(nameTreeX);
      if (dcb.DBrakets.ContainsKey(nameTreeX))
      { 
        var x011 =  dcb.DBrakets[nameTreeX]; 
        x011.StrCommand = ssx;
        dcb.DBrakets[nameTreeX] = x011;
      }
      else
      {
        if (_tb == TypeCollapseBrakets.Logical && _aPattern.IsErrorLogicAndMulti(ssx))
          return null;

        var x01 = new T();
        x01.Instal(nameTreeX, ssx);
        if (dcb.DBrakets.ContainsKey(nameTreeX))
          dcb.DBrakets[nameTreeX] = x01;
        else
          dcb.DBrakets.Add(nameTreeX, x01);
      }

      xScop.RemoveAt(0);
      str = st0;
    }

    if(dcb == null || (_tb == TypeCollapseBrakets.Logical && _aPattern.IsErrorLogicAndMulti(str)))
      return null;

    dcb.LBrakets.Add(nameEnd);
    var x00 = new T();
    x00.Instal(nameEnd, str);
    dcb.DBrakets.Add(nameEnd, x00);

    return dcb;
  }

  private List<(int, int)> Braсkets(string str0)
  {
    List<(int, int)> xBraсkets = new();

    var xl = _aPattern.BraketsLeftCount(str0);
    var xr = _aPattern.BraketsRiteCount(str0);

    while (xl.Count > 0)
    {
      var valL = xl[^1];
      var valR = xr.Where(x => x > valL).Min();
      xBraсkets.Add((valL, valR));
      xl.RemoveAt(xl.Count - 1);
      xr.Remove(valR);
    }
    return xBraсkets;
  }


}

