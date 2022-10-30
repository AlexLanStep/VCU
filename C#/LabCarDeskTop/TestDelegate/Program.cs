#define TestDanLabCar
#define DataEmulation

using System;



namespace TestDelegate // Note: actual namespace depends on the project name.
{
  internal class Program
  {
    static void Main(string[] args)
    {
      Console.WriteLine("Test Delegate");

      //#if debug
      //    Console.WriteLine("Debug build");
      //            Console.WriteLine(" - Test  данных LabCar");

      //#elif TestDanLabCar
      //      Console.WriteLine(" - Test  данных LabCar");
      //#endif
#if TestDanLabCar
    Console.WriteLine("Debug build");
            Console.WriteLine(" - Test  данных LabCar");

#else
      Console.WriteLine(" - !!!!! !!!");
#endif

#if DataEmulation
      Console.WriteLine(" ---  DataEmulation --");
      Console.WriteLine(" - Test  данных LabCar");

#else 
      Console.WriteLine(" Real dan");
#endif


      Test01 _test01 = new Test01();
      _test01.Run();

    }
  }
}