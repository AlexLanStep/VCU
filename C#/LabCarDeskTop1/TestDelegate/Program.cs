using System;

namespace TestDelegate // Note: actual namespace depends on the project name.
{
  internal class Program
  {
    static void Main(string[] args)
    {
      Console.WriteLine("Test Delegate");

      Test01 _test01 = new Test01();
      _test01.Run();

    }
  }
}