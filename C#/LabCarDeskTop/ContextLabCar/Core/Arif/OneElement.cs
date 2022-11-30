namespace ContextLabCar.Core.Arif;

public interface IOneElement
{
  string Name { get; set; }
}
public class OneElement : IOneElement
{
  public string Name { get; set; }
  public string? NameValue { get; set; }
  public string? CommandAri { get; set; }
  private readonly CalcElement? _calcElement;

#pragma warning disable CS8618
  public OneElement(string commandAri, string name = "")
#pragma warning restore CS8618
  {
    var strBasa = commandAri.Replace(" ", "");

    if (strBasa.IndexOf("=", StringComparison.Ordinal) < 0 && name == null)
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

  public void ConvertScobki(string commandScobki)
  {
    _calcElement?.CaclScobki(commandScobki);
    _calcElement?.ReplaseSimvolDan();
  }

}
