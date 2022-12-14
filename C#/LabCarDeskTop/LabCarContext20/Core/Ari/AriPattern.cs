
namespace LabCarContext20.Core.Ari;

public class AriPattern
{
  #region Simvol
  private string _ptSymbol = @"[\+\-\*\/]";
  private string _ptif = @"([\>\<])|([~=]=)";
  private string _ptEq = @"=";
  #endregion
  public bool? _isSymbol = null;

  private Func<string, string, (bool, int)> _f00 = (s0, s1) =>
  {
    var count = Regex.Matches(s0, s1, RegexOptions.IgnoreCase).Count;
    return (count > 0, count);
  };

  public bool? TestInputStr(string str)
  {

    //var symvolx = _f00(str, _ptSymbol);
    
    if (_f00(str, _ptif).Item1) return false;

    if(_f00(str, _ptEq).Item1) return true;

    return null;
  }
}
