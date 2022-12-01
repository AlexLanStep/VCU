
public interface IOneElement
{
}
public class OneElement : IOneElement
{
  public string Name { get; set; }
  public string? NameValue { get; set; }
  public string? CommandAri { get; set; }
  private readonly CalcElement? _calcElement;
  public CVariable CVariable { get; set; }

  public OneElement(string commandAri, string name="")
  {
    var strBasa = commandAri.Replace(" ", "");

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
    _calcElement = new(Name);
  }

  public CVariable FuncCalc()
  {
    if (_calcElement == null) return new CVariable("");

    _calcElement.CaclScobki(CommandAri);
    _calcElement.BasaCommanda = _calcElement.ReplaseSimvolDan(_calcElement.BasaCommanda);
    _calcElement.BasaCommanda = _calcElement.ReplaseMultiDiv(_calcElement.BasaCommanda);
    _calcElement.BasaCommanda = _calcElement.FindNonNumbers(_calcElement.BasaCommanda, NameValue);

    CVariable = StArithmetic.DVarCommand[NameValue];
    return CVariable;
  }

}
