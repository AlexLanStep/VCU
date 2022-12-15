﻿
using System.Reactive.Joins;

namespace LabCarContext20.Core.Ari;

public class AriPattern
{
  #region Simvol
  private string _ptSymbol = @"[\+\-\*\/]";
  private string _ptif = @"([\>\<])|([~=]=)";
  private string _ptEq = @"=";

  private string _ptPlusMin = @"[\+\-]";
  private string _ptMultiDiv = @"[\*\/]";
  private string _ptDestv = @"[\+\-\*\/]";
  private string _ptLifet = @"\(";
  private string _ptRite = @"\)";
  private string _ptBrakets = @"[\(\)]";
  private string _ptNoPlusMin = @"[\*\/\(\)]";
  private string _ptAllSim = @"[\+\-\*\/\(\)]";

  #endregion
  public bool? _isSymbol = null;

  private Func<string, string, (bool, int)> _f00 = (s0, s1) =>
  {
    var count = Regex.Matches(s0, s1, RegexOptions.IgnoreCase).Count;
    return (count > 0, count);
  };

  public (bool, int) IsBrakets(string str) => _f00(str, _ptBrakets);

  public (bool, int) IsEq(string str) => _f00(str, _ptEq);

  public (bool, int) IsDestv(string str) => _f00(str, _ptDestv);

  public (bool, int) IsAllSin(string str) => _f00(str, _ptAllSim);
  public (bool, int) IsPlusMin(string str) => _f00(str, _ptPlusMin);
  public (bool, int) IsMultiDiv(string str) => _f00(str, _ptMultiDiv);
  public List<string> ArrayPlusMin(string str)
    => Regex.Matches(str, _ptPlusMin, RegexOptions.IgnoreCase).Select(x => x.Value).ToList();

  public List<string> SplitPlusMin(string str)
    => Regex.Split(str, _ptPlusMin, RegexOptions.IgnoreCase).ToList();
  public (bool, int) IsUmnDiv(string str) => _f00(str, _ptMultiDiv);
  public List<string> SplitMultiDiv(string str)
    => Regex.Split(str, _ptMultiDiv, RegexOptions.IgnoreCase).ToList();
  public List<string> ArrayMultiDiv(string str)
    => Regex.Matches(str, _ptMultiDiv, RegexOptions.IgnoreCase).Select(x => x.Value).ToList();

  public List<string> SplitDigital(string str)
    => Regex.Split(str, _ptDestv, RegexOptions.IgnoreCase).ToList();

  public (bool, int) IsNoPlusMin(string str) => _f00(str, _ptNoPlusMin);

  public List<int> BraketsX(string str, string pattern)=>
      Regex.Matches(str, pattern, RegexOptions.IgnoreCase).Select(x => x.Index).ToList();
  public List<int> BraketsLeftCount(string str) => BraketsX(str, _ptLifet);
  public List<int> BraketsRiteCount(string str) => BraketsX(str, _ptRite);
  public bool IsErrorLogicAndMulti(string str)=>
      _f00(str, _ptif).Item1 && IsMultiDiv(str).Item1;
    
  
  

  public bool? TestInputStr(string str)
  {

    var _scop = IsBrakets(str);
    if (_scop.Item1)
    {
      if (_scop.Item2 % 2 != 0)
        throw new MyException($" В стратегии не соответствует кол-во скобок \n {str}  ", -2);
    }

    if (_f00(str, _ptif).Item1) return false;

    if (_f00(str, _ptEq).Item1) return true;

    return null;
  }
}
