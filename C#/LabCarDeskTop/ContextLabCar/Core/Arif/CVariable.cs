using ContextLabCar.Static;
using DryIoc.ImTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContextLabCar.Core.Arif;

public class CVariable
{

  public string Name { get; set; }
  public string StValue { get; set; }
  public dynamic? Value { get; set; }
  public bool IsValue { get; set; } 
  public CVariable(string stcomand)
  {
    stcomand = stcomand.Replace(" ", "");
    var ss = stcomand.Split('=');
    if (ss.Length != 2)
      throw new MyException($"Не правильный формат переменных в разборе арефметической строки -> {stcomand}", -5);
    Name = ss[0];
    StValue = ss[1];
    Value = null;
    IsValue = CalcCommand(StValue);
  }

  public CVariable(string name, dynamic dan)
  {
    Name = name;
    Value = dan;
    StValue = "";
    IsValue = true; 
  }

  
  public bool CalcCommand(string stcomand)
  {
    if (Value != null)
      return true;

    if (StArithmetic.IsNoPlusMin(stcomand).Item1)
      return false;

    var _arr = StArithmetic.ArrayPlusMin(stcomand);
    if (_arr == null) return false;
    var _arrSp = StArithmetic.SplitPlusMin(stcomand);
    dynamic[] _xd = new dynamic[_arrSp.Count];

    bool _is = true;
    int i = 0;
    while (_is && (i < _arrSp.Count))
    {
      _xd[i] = StArithmetic.ReadDanExperiment(_arrSp[i]);
      _is &= _xd[i] != null;
      i++;
    }

    if(!_is) return false;

    dynamic _z = _xd[0];
    for (int i1 = 1; i1 < _xd.Length; i1++)
      _z = StArithmetic.CalcElemrnt(_z, _xd[i1], _arr[i1-1]);

    int kk = 1;
    if (_z != null)
    {
      CVariable _cv = new CVariable(Name, _z);
      StArithmetic.DVarCommand.AddOrUpdate(_cv.Name, _cv, (_, _) => _cv);
    }

    return true;
  }

  
}
