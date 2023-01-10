
using System.Runtime.Intrinsics.X86;
using System.Xml.Linq;

namespace LabCarContext20.Core.Ari;

public class AriStrDisassemble : AriPattern
{
  private readonly ILoggerDisplay _iDisplay;
  private readonly DanWriteLc _danWriteLc;
  private readonly DanReadLc _danReadLc;
  private readonly DanValue _danValue;

  private CollapseBrackets collapseBrackets;
  public dynamic? Result { get; set; }
  private string _str;

  public AriStrDisassemble(ILoggerDisplay iDisplay, 
                            DanWriteLc danWriteLc, 
                            DanReadLc danReadLc, 
                            DanValue danValue) {
    _iDisplay = iDisplay;
    //    APattern = new AriPattern();
    collapseBrackets = new CollapseBrackets();
    Result = null;
    _danWriteLc = danWriteLc;
    _danReadLc = danReadLc;
    _danValue =danValue; 


    }

  private ConcurrentDictionary<string, CVariable> dVariables = new();

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
        continue;
      }
      dynamic? _d = _iallDan.Get(it);
      if (_d != null)
      {
        str = str.Replace(it, Convert.ToString(_d));
        continue;
      }

    }

    string str0 = ReplaseMultiDiv(str);
    dynamic? d1 = DynamicStrPlusMin(str0);
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
        str = str.Replace(d, (string)Convert.ToString(xxx))
          .Replace("+-","-")
          .Replace("--","+");
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

//      if (dVariables.ContainsKey(_key0) != null)
      if (dVariables.ContainsKey(_key0))
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
      return FindNonNumbers(str);

    else if(IsPlusMin(str).Item1)
      return DynamicStrPlusMin(str);

    return null;
  }

  public bool? InputStrLogic(string str, Dictionary<string, CVariable> cvx)
  {
    dVariables = new ConcurrentDictionary<string, CVariable>(cvx);


    return null;
  }

  public AriStrDisassemble? AriCalcStr(string str)
  {
    Result = null;
    _str = str.Replace(" ", "").Replace('.', ',');
    var _isSymbol = TestInputStr(_str);
    if (_isSymbol == null)
    {
      throw new MyException($" Проблема в строке (в стратегии)! -> {_str} ", -10);
    }

    if (_isSymbol.Value)
    {
      _iDisplay.Write("Строка вычислений ");
      var _collapseBrakets = collapseBrackets.CalcBrakets<CVariable>(_str);
      foreach (var it in _collapseBrakets.LBrakets)
      {
        string _key = it;
        string sval = _collapseBrakets.DBrakets[it].StrCommand;
        Result = InputStrArifmet(sval, _collapseBrakets.DBrakets);
        if (Result == null)
        {
          throw new MyException($" Error in string, no variable {sval} ", -20);
        }
        else
        {
          var _z0 = _collapseBrakets.DBrakets[it];
          _z0.Value = Result;
          _collapseBrakets.DBrakets[it] = _z0;
        }
      }
    }
    return this;
  }



}

/*
 
     else
    {
      _iDisplay.Write("Строка условий ");
      var _collapseBrakets = collapseBrackets.CalcBrakets<CVariableLogic>(_str);
      if (_collapseBrakets == null)
      {
        throw new MyException($" Проблема в строке (в стратегии)! -> {_str} ", -10);
      }



    }

 */

