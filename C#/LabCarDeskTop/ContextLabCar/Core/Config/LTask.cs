
namespace ContextLabCar.Core.Config;

public interface ILTask
{
  string Signal { get; }
  string TTask { get; }
  string Comment { get; }
}
public class LTask: ILTask
{
  public string Signal { get; }
  public string TTask { get; }
  public string Comment { get; }

  public LTask(string signal, string tTask, string comment="")
  {
    Signal = signal;
    TTask = tTask;
    Comment = comment;
  }

}
