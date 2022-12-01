namespace ContextLabCar.Core.Arif;

public class CVariable
{

  public string Name { get; set; }
  public string StValue { get; set; }
  public dynamic? Value { get; set; }
  public string SValue { get; set; }
  public bool IsValue { get; set; } 
  public CVariable(string stcomand)
  {
    stcomand = stcomand.Replace(" ", "").Replace(",", ".");
    var ss = stcomand.Split('=');
    if (ss.Length != 2)
      throw new MyException($"Не правильный формат переменных в разборе арефметической строки -> {stcomand}", -5);
    Name = ss[0];
    StValue = ss[1];
    Value = null;
    SValue = "";
    IsValue = CalcCommand(StValue);
  }

  public CVariable(string name, dynamic? dan)
  {
    Name = name;
    Value = dan;
    StValue = "";
    IsValue = true;
    SValue = Convert.ToString(Value);
  }

  
  public bool CalcCommand(string stcomand)
  {
    if (Value != null)
      return true;

    if (StArithmetic.IsNoPlusMin(stcomand).Item1)
      return false;

    var arr = StArithmetic.ArrayPlusMin(stcomand);
    if (arr == null) return false;
    var arrSp = StArithmetic.SplitPlusMin(stcomand);
    var xd = new dynamic?[arrSp.Count];

    var @is = true;
    var i = 0;
    while (@is && (i < arrSp.Count))
    {
      xd[i] = StArithmetic.ReadDanExperiment(arrSp[i]);
      @is &= xd[i] != null;
      i++;
    }

    if(!@is) return false;

    var z = xd[0];
    for (var i1 = 1; i1 < xd.Length; i1++)
      z = StArithmetic.CalcElemrnt(z, xd[i1], arr[i1-1]);

    if (z == null) return false;

    var cv = new CVariable(Name, z);
    StArithmetic.DVarCommand.AddOrUpdate(cv.Name, cv, (_, _) => cv);

    return true;
  }
}
