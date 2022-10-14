namespace ContextLabCar.Core;

public class MyException : Exception
{
  public MyException(string message, int num): base(message)
  {
    myError(message, num);
  }

  private void myError(string message, int num)
  {
    Action<string, int> f0 = (s, i) => Console.WriteLine($"  -Error № {i}  -  {s}");
    switch (num)
    {
      case 0:
        f0(message, num);
        break;
      case -1:
        f0(message, -1);
        //                Environment.Exit(-1);
        break;
      case -2:
        f0(message, num);
        Environment.Exit(num);
        break;

      default:
        break;
    }
  }

}
