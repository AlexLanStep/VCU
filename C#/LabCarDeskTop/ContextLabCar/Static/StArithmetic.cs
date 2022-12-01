
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
    var count = Regex.Matches(s0, s1, RegexOptions.IgnoreCase).Count;
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
  public static List<string> SplitMultiDiv(string str)
    => Regex.Split(str, _patternUmnDiv, RegexOptions.IgnoreCase).ToList();
  public static List<string> ArrayMultiDiv(string str)
      => Regex.Matches(str, _patternUmnDiv, RegexOptions.IgnoreCase).Select(x => x.Value).ToList();

  public static List<string> SplitDigital(string str)
    => Regex.Split(str, _patternDestv, RegexOptions.IgnoreCase).ToList();

  public static (bool, int) IsNoPlusMin(string str) => _f00(str, _patternNoPlusMin);

  public static bool IsDigitString(string str)
  {
    str = str.Replace(",", ".");
    var s0 = str.IndexOf('.') > 0 ? str.Replace(".", "") : str;
    return s0.Aggregate(true, (current, x) => current & char.IsDigit(x));
  }

  public static dynamic? StringToDynamic(string str)
  {
    if (!IsDigitString(str)) return null;

    if (str.IndexOf('.') > 0)
    {
      var isR = double.TryParse(str.Replace('.', ','), out var rez);
      return isR ? rez : null;
    }
    else
    {
      var isR = int.TryParse(str, out var rez);
      return isR ? rez : null;
    }
  }

  public static dynamic? ReadDanExperiment(string str)
  {
    var xd0 = StringToDynamic(str);
    if (xd0 == null)
    {
      if (DVarCommand.TryGetValue(str, out var x00))
        return x00.Value;

      var y = LcDan.GetTask(str);
      if (y != null)
        return y;
    }
    else 
      return xd0;

    return null;
  }

  public static List<(int, int)> ScobkiX(string str0)
  {
    List<(int, int)> xScop = new();

    var xl = Regex.Matches(str0, _patternLifet, RegexOptions.IgnoreCase).Select(x => x.Index).ToList();
    var xr = Regex.Matches(str0, _patternRite, RegexOptions.IgnoreCase).Select(x => x.Index).ToList();

    while (xr.Count > 0)
    {
      var valR = xr.ElementAt(0);
      var @is = true;
      var kL = 0;
      var kLMax = xl.Count();
      while (@is & xl.Count > 0)
      {
        var valL = xl.ElementAt(kL);

        if ((valR > valL) & (valR < xl.ElementAt(Math.Min(kL + 1, kLMax - 1))) || (xl.Count == 1 && xr.Count == 1))
        {
          xScop.Add((valL, valR));
          @is = false;
          xl.RemoveAt(kL);
          xr.RemoveAt(0);
          continue;
        }
        kL++;
      }
    }

    return xScop;
  }

  public static dynamic? CalcElemrnt(dynamic? d0, dynamic? d1, string sim)
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
    var cv0 = new CVariable("ss11", 12);
    DVarCommand.AddOrUpdate(cv0.Name, cv0, (_, _) => cv0);
    var cv1 = new CVariable("ds1", 212.33);
    DVarCommand.AddOrUpdate(cv1.Name, cv1, (_, _) => cv1);
    var cv2 = new CVariable("sas", 44);
    DVarCommand.AddOrUpdate(cv2.Name, cv2, (_, _) => cv2);
    var cv3 = new CVariable("ss11", 777.111);
    DVarCommand.AddOrUpdate(cv3.Name, cv3, (_, _) => cv3);
    var cv4 = new CVariable("ee1", 22.333);
    DVarCommand.AddOrUpdate(cv4.Name, cv4, (_, _) => cv4);
  }

  public static dynamic? MultiDiv(string str)
  {
    str = str.Replace(",", ".");
    var lsVal = SplitMultiDiv(str);
    var lsZnak = ArrayMultiDiv(str);
    if (lsVal.Count == 2 && lsZnak.Count == 1)
      return CalcElemrnt(StringToDynamic(lsVal[0]), StringToDynamic(lsVal[1]), lsZnak[0]);
    return null;
  }

}
