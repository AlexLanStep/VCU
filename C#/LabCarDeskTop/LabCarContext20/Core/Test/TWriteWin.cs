
namespace LabCarContext20.Core.Test;

public interface ITWriteWin
{
  void WriteYes();
  void WriteNo();
}
public class TWriteWin: ITWriteWin
{
  private IDataOutputToDisplay _ioutputToDisplay;
  private bool isWrite;
  public TWriteWin(IDataOutputToDisplay ioutputToDisplay)
  {
    isWrite = false;
    _ioutputToDisplay = ioutputToDisplay;
    _ioutputToDisplay.AsyncEvent += _ioutputToDisplay_AsyncEvent;
  }

  public void WriteYes() => isWrite = true;

  public void WriteNo()=>isWrite = false;

  private Task _ioutputToDisplay_AsyncEvent(string str)
      => isWrite? Task.Run(() => Console.WriteLine($"{str}"))
                : Task.FromResult(0);

}

