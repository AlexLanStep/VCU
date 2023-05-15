namespace LabCarContext20.Core.Ari;

public class AriStrDisassemble : AriPattern
{
  private readonly ILoggerDisplay _iDisplay;
  private readonly DanWriteLc _danWriteLc;
  private readonly DanReadLc _danReadLc;
  private readonly DanValue _danValue;

  private CollapseBrackets collapseBrackets;
  public dynamic? Result { get; set; }
  private string _str="";

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

  private ConcurrentDictionary<string, CVariable> _dVariables = new();

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
//  !!!  проверить nameX
    // ReSharper disable once RedundantAssignment
    nameX = nameX == "" 
        ? Guid.NewGuid().ToString().Replace("-", "_") 
        : nameX;

    var countMultiDiv = IsMultiDiv(str);
    var countPlusMin = IsPlusMin(str);
    if (!countMultiDiv.Item1 && countPlusMin.Item1) return str;

    var countDigital = SplitDigital(str);
    foreach (var it in countDigital.Where(x => !IsDigitString(x)))
    {
      if (_dVariables.TryGetValue(it, out var dan))
      {
        if (!dan.IsValue)
        {
          FindNonNumbers(dan.SValue, it);
          if (_dVariables[it].IsValue)
            str = str.Replace(it, _dVariables[it].SValue);
        }
        else
          str = str.Replace(it, Convert.ToString(dan.Value));
        continue;
      }

      var d = _danReadLc.Get(it);
      d ??=_danValue.Get(it);

      if (d == null) continue;

      str = str.Replace(it, Convert.ToString(d));
    }

    var str0 = ReplaseMultiDiv(str);
    var d1 = DynamicStrPlusMin(str0);
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
      var key0 = arrSp[i];
      if (arrSp[i].Length == 0)
      {
        xd[i] = 0;
        i++;
        continue;
      }
      var dx = StringToDynamic(key0);
      if (dx != null)
      {
        xd[i] = dx;
        i++;
        continue;
      }

      if (_dVariables.ContainsKey(key0))
      {
         xd[i] = _dVariables[key0].Value;
        i++;
        continue;
      }

      var d = _danReadLc.Get(key0);
      if (d == null)
        d = _danValue.Get(key0);

      xd[i] = d;
      @is &= xd[i] != null;
      i++;
    }

    if (!@is) return null;

    var z = xd[0];
    for (var i1 = 1; i1 < xd.Length; i1++)
      z = CalcElemrnt(z, xd[i1], arr[i1 - 1]);

    return z == null ? null : z;
  }

  public dynamic? InputStrArifmet(string str, Dictionary<string, CVariable> cvx)
  {
    _dVariables = new ConcurrentDictionary<string, CVariable>(cvx);

    if (IsMultiDiv(str).Item1)
      return FindNonNumbers(str);

    else if(IsPlusMin(str).Item1)
      return DynamicStrPlusMin(str);

    try
    {
      return str.IndexOf(",", StringComparison.Ordinal) >= 0 ? Convert.ToDouble(str) : Convert.ToInt32(str);
    }
    catch (Exception e)
    {
      _iDisplay.Write(e.ToString());
      throw new MyException($" Error in AriStrDisassemble {str}", -11);
    }
  }

  public bool? InputStrLogic(string str, Dictionary<string, CVariable> cvx)
  {
    _dVariables = new ConcurrentDictionary<string, CVariable>(cvx);

    return null;
  }

  public AriStrDisassemble AriCalcStr(string str)
  {
    Result = null;
    _str = str.Replace(" ", "").Replace('.', ',');
    var isSymbol = TestInputStr(_str);
    if (isSymbol == null)
    {
      throw new MyException($" Проблема в строке (в стратегии)! -> {_str} ", -10);
    }

    if (!isSymbol.Value) return this;

    _iDisplay.Write("Строка вычислений ");
    var collapseBraсkets = collapseBrackets.CalcBrakets<CVariable>(_str);
      
    if (collapseBraсkets?.LBrakets == null) return this;
      
    foreach (var it in collapseBraсkets.LBrakets)
    {
      var sval = collapseBraсkets.DBrakets[it].StrCommand;
      Result = InputStrArifmet(sval, collapseBraсkets.DBrakets);
      if (Result == null)
      {
        throw new MyException($" Error in string, no variable {sval} ", -20);
      }
      else
      {
        var z0 = collapseBraсkets.DBrakets[it];
        z0.Value = Result;
        collapseBraсkets.DBrakets[it] = z0;
        if (!_danWriteLc.Set(it, Result))
          _danValue.Set(it, Result);
      }
    }
    return this;
  }



}

