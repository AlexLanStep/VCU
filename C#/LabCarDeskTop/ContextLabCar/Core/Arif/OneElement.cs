// ReSharper disable once CheckNamespace
public interface IOneElement
{
}
public class OneElement : IOneElement
{
  // ReSharper disable once UnusedAutoPropertyAccessor.Local
  private string Name { get; set; }
  private string NameValue { get; set; }
  private string CommandAri { get; set; }
  
  public CVariable CVariable { get; set; }

  private string BasaCommanda { get; set; }

  private readonly string _nameTree = "__#";
  private int _indexCom;
  private string strBasa;

#pragma warning disable CS8618
  public OneElement(string commandAri, string name="")
#pragma warning restore CS8618
  {
    strBasa = commandAri.Replace(" ", "").Replace(',', '.');

    if (strBasa.IndexOf("=", StringComparison.Ordinal) < 0 && name == "")
      return;

    if (strBasa.IndexOf("=", StringComparison.Ordinal) > 0)
    {
      var s0 = strBasa.Split("=");
      NameValue = s0[0];
      CommandAri = s0[1];
      Name = "root";
    }
    else
    {
      NameValue = name;
      CommandAri = strBasa;
      Name = name;
    }
    BasaCommanda = CommandAri;
    _indexCom = 0;
    _nameTree = _nameTree + name;
  }

  public CVariable FuncCalc()
  {
    var testStr = StArithmetic.IsAllSin(CommandAri);
    if(!testStr.Item1 && (CommandAri.Length > 0) && StArithmetic.IsDigitString(CommandAri))
    {
      var x = StArithmetic.StringToDynamic(CommandAri);
      if(x != null)
      {
        CVariable = new CVariable(NameValue, x);
        StArithmetic.DVarCommand.AddOrUpdate(CVariable.Name, CVariable, (_, _) => CVariable);

        return CVariable;
      }
    }

    if ((!StArithmetic.IsUmnDiv(CommandAri).Item1) && (!StArithmetic.IsScobki(CommandAri).Item1) && (StArithmetic.IsPlusMin(CommandAri).Item1))
    {
      CVariable = new CVariable(strBasa);
//      StArithmetic.DVarCommand.AddOrUpdate(CVariable.Name, CVariable, (_, _) => CVariable);
      return CVariable;
    }


    CaclScobki(CommandAri);
    BasaCommanda = ReplaseSimvolDan(BasaCommanda);
    BasaCommanda = ReplaseMultiDiv(BasaCommanda);
    BasaCommanda = FindNonNumbers(BasaCommanda, NameValue);

    CVariable = StArithmetic.DVarCommand[NameValue];
    return CVariable;
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
      _ = StArithmetic.DVarCommand.TryRemove(it.Key, out _);
    }
    return str;
  }

  public string ReplaseMultiDiv(string str)
  {
    var countMultiDiv = StArithmetic.IsUmnDiv(str);
    if (!countMultiDiv.Item1) return str;

    var dls = StArithmetic.SplitPlusMin(str).Where(x => x.Contains("*") || x.Contains("/")).ToList();

    foreach (var d in dls)
    {
      var xxx = StArithmetic.MultiDiv(d);
      if (xxx != null)
        str = str.Replace(d, (string)Convert.ToString(xxx));
    }
    return str;
  }

  public string FindNonNumbers(string str, string nameX = "")
  {
    var countMultiDiv = StArithmetic.IsUmnDiv(str);
    var countPlusMin = StArithmetic.IsPlusMin(str);
    if (!countMultiDiv.Item1 && countPlusMin.Item1) return str;

    var countDigital = StArithmetic.SplitDigital(str);
    foreach (var it in countDigital.Where(x => !StArithmetic.IsDigitString(x)))
    {
      if (StArithmetic.DVarCommand.TryGetValue(it, out var dan))
      {
        if (!dan.IsValue)
        {
          FindNonNumbers(dan.StValue, it);
          if (StArithmetic.DVarCommand[it].IsValue)
            str = str.Replace(it, StArithmetic.DVarCommand[it].SValue);
        }
        else
          str = str.Replace(it, Convert.ToString(dan.Value));

      }
    }
    str = (nameX == "" ? "root" : nameX) + "=" + ReplaseMultiDiv(str);

    // ReSharper disable once UnusedVariable
    var cv = new CVariable(str);
    return str;

  }

}

