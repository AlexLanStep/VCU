using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContextLabCar.Static;

public static class StArithmetic
{
  #region pattern
  private static string _patternPlusMin = @"[\+\-]";
  private static string _patternUmnDiv = @"[\*\/]";
  private static string _patternDestv = @"[\+\-\*\/]";
  private static string _patternLifet = @"\(";
  private static string _patternRite = @"\)";
  private static string _patternScobki = @"[\(\)]";
  #endregion

  private static Func<string, string, (bool, int)> _f00 = (s0, s1) =>
  {
    int count = Regex.Matches(s0, s1, RegexOptions.IgnoreCase).Count;
    return (count > 0, count);
  };

  public static (bool, int) IsScobki(string str) => _f00(str, _patternScobki);
  public static (bool, int) IsDestv(string str) => _f00(str, _patternDestv);
  public static (bool, int) IsPlusMin(string str) => _f00(str, _patternPlusMin);
  public static (bool, int) IsUmnDiv(string str) => _f00(str, _patternUmnDiv);
  public static List<(int, int)> ScobkiX(string _str0)
  {
    List<(int, int)> xScop = new();

    var _newSig = _str0;
    var xl = Regex.Matches(_newSig, _patternLifet, RegexOptions.IgnoreCase).Select(x => x.Index).ToList();
    var xr = Regex.Matches(_newSig, _patternRite, RegexOptions.IgnoreCase).Select(x => x.Index).ToList();
    var countnewSig = _newSig.Length;

    int _valR = 0;
    int _valL = 0;

    while (xr.Count > 0)
    {
      _valR = xr.ElementAt(0);
      bool _is = true;
      int kL = 0;
      int kLMax = xl.Count();
      while (_is & xl.Count > 0)
      {
        _valL = xl.ElementAt(kL);

        if ((_valR > _valL) & (_valR < xl.ElementAt(Math.Min(kL + 1, kLMax - 1))) || (xl.Count == 1 && xr.Count == 1))
        {
          xScop.Add((_valL, _valR));
          _is = false;
          xl.RemoveAt(kL);
          xr.RemoveAt(0);
          continue;
        }
        kL++;
      }
    }

    return xScop;
  }



}
