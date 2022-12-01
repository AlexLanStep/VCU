using DryIoc.ImTools;

namespace ContextLabCar.Core.Arif;

public interface ICalcElement
{
}
public class CalcElement : ICalcElement
{
  public string BasaCommanda { get; set; }

  private readonly string _nameTree = "__#";
  private int _indexCom;
  public CalcElement(string name)
  {
    BasaCommanda = "";
    _indexCom = 0;
    _nameTree = _nameTree + name;
  }

  public void CaclScobki(string scobki)
  {
    BasaCommanda = scobki;
    while (StArithmetic.IsScobki(scobki).Item1)
    {
      var st0 = scobki;
      var xScop = StArithmetic.ScobkiX(scobki);

      if (xScop.Count <= 0)
        continue;

      var x0 = xScop.ElementAt(0);

      var ssx = scobki.Substring(x0.Item1, x0.Item2 - x0.Item1 + 1);
      var nameTreeX = _nameTree + _indexCom;
      st0 = st0.Replace(ssx, nameTreeX);
      ssx = ssx.Replace("(", "").Replace(")", "");

      var cv = new CVariable(nameTreeX + "=" + ssx);
      if (!cv.IsValue)
        StArithmetic.DVarCommand.AddOrUpdate(nameTreeX, cv, (_, _) => cv);

      xScop.RemoveAt(0);
      scobki = st0;
      _indexCom++;
    }
    BasaCommanda = scobki;
  }

  public string ReplaseSimvolDan(string str)
  {
    var keyVal = StArithmetic.DVarCommand.Where(z => z.Value.IsValue)
      .Select(x => new { x.Key, x.Value.Value })
      .ToDictionary(item => item.Key, item => ((string)Convert.ToString(item.Value)).Replace(',', '.'));

    foreach (var it in keyVal
               .Where(it => str.Contains(it.Key) && it.Key.Contains(_nameTree)))
    {
      str = str.Replace(it.Key, it.Value);
      _ = StArithmetic.DVarCommand.TryRemove(it.Key, out var xx);
    }
    return str;
  }

  public string ReplaseMultiDiv(string str)
  {
    var countMultiDiv = StArithmetic.IsUmnDiv(str);
    if (!countMultiDiv.Item1) return str;

    var dls = StArithmetic.SplitPlusMin(str).Where(x => x.Contains("*") || x.Contains("/")).ToList();

    Dictionary<string, dynamic?> dyn0 = new();
    foreach ( var d in dls )
    {
      var xxx = StArithmetic.MultiDiv(d);
      if (xxx != null)
        str = str.Replace(d, (string)Convert.ToString(xxx)); 
    }
    return str;
  }

  public string FindNonNumbers(string str, string nameX="")
  {
    var countMultiDiv = StArithmetic.IsUmnDiv(str);
    var countPlusMin = StArithmetic.IsPlusMin(str);
    if (!countMultiDiv.Item1 && countPlusMin.Item1) return str;

    var countDigital = StArithmetic.SplitDigital(str);
    foreach (var it in countDigital.Where(x=>!StArithmetic.IsDigitString(x)))
    {
      if(StArithmetic.DVarCommand.TryGetValue(it, out var dan))
      {
        if (!dan.IsValue)
        {
          var s0 = FindNonNumbers(dan.StValue, it);
          if (StArithmetic.DVarCommand[it].IsValue)
            str = str.Replace(it, StArithmetic.DVarCommand[it].SValue);
        }
        else
          str = str.Replace(it, Convert.ToString(dan.Value));
        
      }
    }
    str = (nameX==""? "root": nameX) + "=" +ReplaseMultiDiv(str);

    var cv = new CVariable(str);
    return str;

  }

}

/*
 
   //public string ReplasePlusMinus(string str)
  //{
  //  var countPlusMinus = StArithmetic.IsPlusMin(str);
  //  if (!countPlusMinus.Item1) return str;

  //  var dls = StArithmetic.SplitPlusMin(str).ToList();

  //  //Dictionary<string, dynamic?> dyn0 = new();
  //  foreach (var d in dls)
  //  {
  //    var xxx = StArithmetic.MultiDiv(d);
  //    if (xxx != null)
  //      str = str.Replace(d, (string)Convert.ToString(xxx));
  //  }
  //  return str;
  //}

 */