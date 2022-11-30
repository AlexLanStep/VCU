using DryIoc.ImTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ContextLabCar.Core;

public interface ICalcElement
{
}
public class CalcElement: ICalcElement
{
  #region pattern
  private readonly string _patternPlusMin = @"[\+\-]";
  private readonly string _patternUmnDiv = @"[\*\/]";
  private readonly string _patternDestv = @"[\+\-\*\/]";
  private readonly string _patternLifet = @"\(";
  private readonly string _patternRite = @"\)";
  private readonly string _patternScobki = @"[\(\)]";
  #endregion

  private List<(int, int)> xScop = new();
  public Dictionary<string, string> DComands { get; set; }
  public OneElement oneElement { get; set; }

  private readonly string _nameTree = "__#";
  private int _indexCom = 0;

  public CalcElement(string name)
  {
    _nameTree = _nameTree+name;

    DComands = new Dictionary<string, string>();
  }

  public void CaclScobki(string scobki)
  {
    DComands.Clear();
    while (IsScobki(scobki).Item1)
    {
      var _st0 = scobki;
      scobkiX(scobki);

      if (xScop.Count <= 0)
        continue;

      var x0 = xScop.ElementAt(0);
  
      string ssx = scobki.Substring(x0.Item1, x0.Item2 - x0.Item1 + 1);
      var _nameTreeX = _nameTree + _indexCom;
      _st0 = _st0.Replace(ssx, _nameTreeX);
      ssx = ssx.Replace("(", "").Replace(")", "");
      DComands.Add(_nameTreeX, ssx);
      xScop.RemoveAt(0);
      scobki = _st0;
      _indexCom++;
    }
  }

  private Func<string, string, (bool, int)> _f00 = (s0, s1) =>
  {
    int count = Regex.Matches(s0, s1, RegexOptions.IgnoreCase).Count;
    return (count > 0, count);
  };

  public (bool, int) IsScobki(string str) => _f00(str, _patternScobki);
  public (bool, int) IsDestv(string str) => _f00(str, _patternDestv);
  public (bool, int) IsPlusMin(string str) => _f00(str, _patternPlusMin);
  public (bool, int) IsUmnDiv(string str) => _f00(str, _patternUmnDiv);
  private void scobkiX(string _str0)
  {
    var _newSig = _str0;
    var xl = Regex.Matches(_newSig, _patternLifet, RegexOptions.IgnoreCase).Select(x => x.Index).ToList();
    var xr = Regex.Matches(_newSig, _patternRite, RegexOptions.IgnoreCase).Select(x => x.Index).ToList();
    var countnewSig = _newSig.Length;

    xScop.Clear();

    while (xr.Count > 0)
    {
      int _valR = xr.ElementAt(0);
      int _valL = xl.Where(x => x < _valR).ToList().Max();
      xScop.Add((_valL, _valR));
      xl.Remove(_valL);
      xr.Remove(_valR);
    }


  }



}
//OneElement
