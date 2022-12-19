
using DryIoc.ImTools;
using System.Xml.Linq;

namespace LabCarContext20.Core.Strategies;

public interface IStSetDan
{
  void SetDan(string name, dynamic dan);
  bool? SetDan(string StrCommand);
  bool? SetDans(List<string> ls);
}


public class StSetDan: IStSetDan
{
  private readonly DanValue _danValue;
  private readonly IAllDan _iAllDan;
  private readonly AriPattern _pattern;
  private readonly ILoggerDisplay _iloggerDisplay;

  public StSetDan(ILoggerDisplay iloggerDisplay, DanValue danValue, IAllDan allDan, AriPattern pattern)
  {
    _iloggerDisplay = iloggerDisplay;
    _danValue = danValue;
    _iAllDan = allDan;
    _pattern = pattern;
  }

  public void SetDan(string name, dynamic dan)=>
    //_danValue.Set(name, dan);
    _iAllDan.Add<dynamic>(name, dan);
  public bool? SetDan(string StrCommand)
  {
    StrCommand = StrCommand.Trim().Replace(" ","");
    string[] s = StrCommand.Split("=");
    if (s.Length != 2)
    {
      throw new MyException($" Проблема со строкой {StrCommand} ", -3);
      return false; 
    }

    dynamic? d = _pattern.StringToDynamic(s[1]);
    if (d == null)
    {
      throw new MyException($" Проблема с конвертацией строки в xbckj {s[1]} ", -31);
      return null; 
    }

//    _danValue.Set(s[0], d);
    _iAllDan.Add<dynamic>(s[0], d);

    return true;
  }
  public bool? SetDans(List<string> ls)
  {
    foreach (var it in ls)
    {
      var _isRez = SetDan(it);
      if ((_isRez == null) || (!_isRez.Value))
        return null;
    }

    return true;
  }

}

