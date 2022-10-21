
namespace ContextLabCar.Core.Config;

public interface ICalibrat
{
  dynamic Val { get;}
  string Comment { get; }
  string Text { get; }
}
public class Calibrat : ICalibrat
{
  public dynamic Val { get; }
  public string Comment { get; }
  public string Text { get; }
  public Calibrat(string nameModel, string name, dynamic val, string comment="")
  {
    Val = (val.GetType() == typeof(string))?0.0:val;
    Comment = comment;
    Text = $"FESTWERT {nameModel}/{name}/Value \n  WERT {((string)Val.ToString()).Replace(',', '.')} \nEND\n";
  }
}
