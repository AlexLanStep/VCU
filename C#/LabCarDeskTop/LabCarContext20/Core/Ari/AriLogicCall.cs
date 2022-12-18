
using System.Runtime.Intrinsics.X86;
using System.Xml.Linq;

namespace LabCarContext20.Core.Ari;

public class AriLogicCall: AriPattern
{
  private readonly IAllDan _iallDan;
  public AriLogicCall(IAllDan iallDan) => _iallDan = iallDan;

  private ConcurrentDictionary<string, CVariable> dVariables = new();

  public virtual bool ResultEq(dynamic x0, dynamic x1) => Math.Abs((double)x0 - (double)x1) < 0.0001;  // ==
  public virtual bool ResultNe(dynamic x0, dynamic x1) => Math.Abs((double)x0 - (double)x1) > 0.0001;  // !=
  public virtual bool ResultGe(dynamic x0, dynamic x1) => x0 >= x1; // >= 
  public virtual bool ResultGt(dynamic x0, dynamic x1) => x0 > x1; // >
  public virtual bool ResultLe(dynamic x0, dynamic x1) => x0 <= x1; // <= 
  public virtual bool ResultLt(dynamic x0, dynamic x1) => x0 < x1; // < 

  public bool IsDigitString(string str)=>
    str.Replace(".", "")
      .Replace(",", "")
      .Aggregate(true, (current, x) => current & char.IsDigit(x));
  


//  {
//    str = str.Replace(".", "").Replace(",", "");
////    var s0 = str.IndexOf('.') > 0 ? str.Replace(".", ",") : str;
//    return str.Aggregate(true, (current, x) => current & char.IsDigit(x));
//  }

  public dynamic? StringToDynamic(string str)
  {
    if (!IsDigitString(str.Replace("-",""))) return null;

    str = str.Replace('.', ',');

    if (str.IndexOf(',') > 0)
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
  public dynamic? CalcElemrnt(dynamic? d0, dynamic? d1, string sim)
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

  public dynamic? MultiDiv(string str)
  {
//    str = str.Replace(",", ".");
    var lsVal = SplitMultiDiv(str);
    var lsZnak = ArrayMultiDiv(str);
    if (lsVal.Count == 2 && lsZnak.Count == 1)
      return CalcElemrnt(StringToDynamic(lsVal[0]), StringToDynamic(lsVal[1]), lsZnak[0]);
    return null;
  }

  public dynamic? FindNonNumbers(string str, string nameX = "")
  {
//    if (string.IsNullOrEmpty(nameX))
//      nameX = Guid.NewGuid().ToString().Replace("-", "_");
    nameX = nameX == "" 
        ? Guid.NewGuid().ToString().Replace("-", "_") 
        : nameX;

    var countMultiDiv = IsMultiDiv(str);
    var countPlusMin = IsPlusMin(str);
    if (!countMultiDiv.Item1 && countPlusMin.Item1) return str;

    var countDigital = SplitDigital(str);
    foreach (var it in countDigital.Where(x => !IsDigitString(x)))
    {
      if (dVariables.TryGetValue(it, out var dan))
      {
        if (!dan.IsValue)
        {
          FindNonNumbers(dan.SValue, it);
          if (dVariables[it].IsValue)
            str = str.Replace(it, dVariables[it].SValue);
        }
        else
          str = str.Replace(it, Convert.ToString(dan.Value));

      }
    }

    string str0 = ReplaseMultiDiv(str);
    dynamic? d1 = DynamicStrPlusMin(str0);

//    str = (nameX == "" ? "root" : nameX) + "=" + str1;

    // ReSharper disable once UnusedVariable
//    var cv = new CVariable(str);
    return d1;

  }

  private string ReplaseMultiDiv(string str)
  {
    var countMultiDiv = IsMultiDiv(str);
    if (!countMultiDiv.Item1) return str;

    str = str.Replace("*-", "@").Replace("/-", "#");

    var dls = SplitPlusMin(str).Where(x => 
                                              x.Contains("*") 
                                           || x.Contains("/") 
                                           || x.Contains("@") 
                                           || x.Contains("#"))
                                        .ToList();

    foreach (var d in dls)
    {
      var d0 = d.Trim().Replace("@","*-").Replace("#","/-");
      var xxx = MultiDiv(d0);
      if (xxx != null)
        str = str.Replace(d, (string)Convert
          .ToString(xxx))
          .Replace("+-","-");
    }

    return str;
  }

  public dynamic? DynamicStrPlusMin(string str)
  {
    var arr = ArrayPlusMin(str);
    if (arr == null) return false;
    var arrSp = SplitPlusMin(str);
    var xd = new dynamic?[arrSp.Count];

    var @is = true;
    var i = 0;
    while (@is && (i < arrSp.Count))
    {
      var _key0 = arrSp[i];
      if (arrSp[i].Length == 0)
      {
        xd[i] = 0;
        i++;
        continue;
      }
      var dx = StringToDynamic(_key0);
      if (dx != null)
      {
        xd[i] = dx;
        i++;
        continue;
      }

      if (dVariables.ContainsKey(_key0) != null)
      {
        xd[i] = dVariables[_key0].Value;
        i++;
        continue;
      }

      xd[i] = _iallDan.Get(_key0);
      @is &= xd[i] != null;
      i++;
    }

    if (!@is) return null;

    var z = xd[0];
    for (var i1 = 1; i1 < xd.Length; i1++)
      z = CalcElemrnt(z, xd[i1], arr[i1 - 1]);

    if (z == null) return null;

    int i111 = 1;
    return z;
  }

  public dynamic? InputStrArifmet(string str, Dictionary<string, CVariable> cvx)
  {
    dVariables = new ConcurrentDictionary<string, CVariable>(cvx);

    if (IsMultiDiv(str).Item1)
    {
      return FindNonNumbers(str);
//      dynamic? xxxx = DynamicStrPlusMin(str11);

//      dynamic? sss = xxxx;
//      return xxxx;

    }

    else if(IsPlusMin(str).Item1)
      return DynamicStrPlusMin(str);

    return null;
  }
}

//var ls01 = SplitMultiDiv(str);
//var ls02 = ArrayMultiDiv(str);
//var la03 =  SplitPlusMin(str);
