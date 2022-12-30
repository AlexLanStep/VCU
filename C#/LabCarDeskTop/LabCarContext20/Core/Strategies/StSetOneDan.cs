
using DryIoc.ImTools;
using System.Xml.Linq;

namespace LabCarContext20.Core.Strategies;

public interface IStSetOneDan
{
  void SetDan(string name, dynamic dan);
  bool? SetDan(string StrCommand);
  bool? SetDans(List<string> ls);
}


public class StSetOneDan: IStSetOneDan
{
//  private readonly DanValue _danValue;
  private readonly IAllDan _iAllDan;
  private readonly AriStrDisassemble _strDisassemble;
  private readonly ILoggerDisplay _iloggerDisplay;
  private DanValue _danValue;

  public StSetOneDan(ILoggerDisplay iloggerDisplay, IAllDan allDan, DanValue danValue, AriStrDisassemble strDisassemble)
  {
    _iloggerDisplay = iloggerDisplay;
    _danValue = danValue;
    _iAllDan = allDan;
    _strDisassemble = strDisassemble;
  }

  public void SetDan(string name, dynamic dan)=>
    //_danValue.Set(name, dan);
    _danValue.Add(name, dan);

  public bool? SetDan(string StrCommand)
  {
    StrCommand = StrCommand.Trim().Replace(" ","");
    string[] s = StrCommand.Split("=");
    if (s.Length != 2)
    {
      throw new MyException($" Проблема со строкой {StrCommand} ", -3);
      return false; 
    }

    switch (s[1].ToLower())
    {
      case "true":
        _danValue.Add(s[0], true);
        return true;
      case "false":
        _danValue.Add(s[0], false);
        return true;
      default:
        break;
    }

    dynamic? d = _strDisassemble.AriCalcStr(s[1]);
    if (d == null)
    {
      throw new MyException($" Проблема с конвертацией строки в xbckj {s[1]} ", -31);
      return null; 
    }

//    _danValue.Set(s[0], d);

    _danValue.Add(s[0], d);

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

