using ContextLabCar.Static;
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

  public CVariable(string stcomand)
  {
    stcomand = stcomand.Replace(" ", "");
    var ss = stcomand.Split('=');
    if (ss.Length != 2)
      throw new MyException($"Не правильный формат переменных в разборе арефметической строки -> {stcomand}", -5);
    Name = ss[0];
    StValue = ss[1];
    Value = null;
    bool _is = CalcCommand(StValue);
  }

  public bool CalcCommand(string stcomand)
  {
    if (StArithmetic.IsNoPlusMin(stcomand).Item1)
      return false;

    return true;
  }
}
