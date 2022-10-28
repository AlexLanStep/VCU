
namespace ContextLabCar.Core.Config;
public interface ILTask
{
  string Signal { get; }
  string LabCarTask { get; }
  string NameLabCar { get; }
  string Comment { get; }
}
public class LTask: ILTask
{
  public string Signal { get; }
  public string LabCarTask { get; }
  public string NameLabCar { get; }
  public string Comment { get; }

  public LTask(string signal, string tTask, string nameLabCar, string comment="")
  {
    Signal = signal;
    LabCarTask = tTask;
    NameLabCar = nameLabCar;
    Comment = comment;
  }

}
