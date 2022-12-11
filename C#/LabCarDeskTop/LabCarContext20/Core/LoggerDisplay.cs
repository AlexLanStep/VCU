
using DryIoc.ImTools;

namespace LabCarContext20.Core;
public delegate Task AsyncEventHandler(string str);
public class LoggerDisplay: ILoggerDisplay
{
  public bool Is { get; set; } = true;
  public event AsyncEventHandler? AsyncEvent = null;
  public void InitializationConsole() => Is = true;
  public void InitializationWindows() => Is = false;

  public async void Write(string str)
  {
    if(Is) 
      Console.WriteLine($"{str}");
    else
      if(AsyncEvent!=null) 
        await AsyncEvent?.Invoke(str);
  }
}