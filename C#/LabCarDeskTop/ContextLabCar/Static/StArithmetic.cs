using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography;
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
  private static string _patternNoPlusMin = @"[\*\/\(\)]";

  #endregion

  public static ConcurrentDictionary<string, CVariable> DVarCommand = new();

  private static Func<string, string, (bool, int)> _f00 = (s0, s1) =>
  {
    int count = Regex.Matches(s0, s1, RegexOptions.IgnoreCase).Count;
    return (count > 0, count);
  };

  public static (bool, int) IsScobki(string str) => _f00(str, _patternScobki);
  public static (bool, int) IsDestv(string str) => _f00(str, _patternDestv);
  public static (bool, int) IsPlusMin(string str) => _f00(str, _patternPlusMin);

  public static List<string> ArrayPlusMin(string str)
      => Regex.Matches(str, _patternPlusMin, RegexOptions.IgnoreCase).Select(x => x.Value).ToList();

  public static List<string> SplitPlusMin(string str)
    => Regex.Split(str, _patternPlusMin, RegexOptions.IgnoreCase).ToList();

  public static (bool, int) IsUmnDiv(string str) => _f00(str, _patternUmnDiv);
  public static (bool, int) IsNoPlusMin(string str) => _f00(str, _patternNoPlusMin);

  public static bool IsDigitString(string str)
  {
    bool _isDig = true;
    var s0 = str.IndexOf('.') > 0 ? str.Replace(".", "") : str;
    foreach (var x in s0)
      _isDig &= char.IsDigit(x);
    return _isDig;
  }

  public static dynamic? StringToDynamic(string str)
  {
    dynamic? result = null;
    if (IsDigitString(str))
    {
      if (str.IndexOf('.') > 0)
      {
        bool _isR = double.TryParse(str.Replace('.', ','), out double rez);
        result = _isR ? rez : null;
        return result;
      }
      else
      {
        bool _isR = int.TryParse(str, out int rez);
        result = _isR ? rez : null;
        return result;
      }
    }
    return null;
  }

  public static dynamic? ReadDanExperiment(string str)
  {
    dynamic _xd0 = StringToDynamic(str);
    if (_xd0 == null)
    {
      if (DVarCommand.TryGetValue(str, out var x00))
        return x00.Value;

      var _y = LCDan.GetTask(str);
      if (_y != null)
        return _y;
    }
    else 
      return _xd0;

    return null;
  }


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

  public static dynamic? CalcElemrnt(dynamic d0, dynamic d1, string sim)
  {
    return sim.Trim() switch
    {
      "+" => d0 + d1,
      "-" => d0 - d1,
      "*" => d0 * d1,
      "/" => d0 / d1,
      _ => null
    }; 
  }


  public static void TestIniciallDan()
  {
    //ss11; ds1; sas; ss11; ee1
    CVariable _cv0 = new CVariable("ss11", 12);
    DVarCommand.AddOrUpdate(_cv0.Name, _cv0, (_, _) => _cv0);
    CVariable _cv1 = new CVariable("ds1", 212.33);
    DVarCommand.AddOrUpdate(_cv1.Name, _cv1, (_, _) => _cv1);
    CVariable _cv2 = new CVariable("sas", 44);
    DVarCommand.AddOrUpdate(_cv2.Name, _cv2, (_, _) => _cv2);
    CVariable _cv3 = new CVariable("ss11", 777.111);
    DVarCommand.AddOrUpdate(_cv3.Name, _cv3, (_, _) => _cv3);
  }

}
