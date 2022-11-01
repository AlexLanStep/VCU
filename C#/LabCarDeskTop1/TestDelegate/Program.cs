#define TestDanLabCar
#define DataEmulation

using System;



namespace TestDelegate // Note: actual namespace depends on the project name.
{
  internal class Program
  {
    static void TestType<T>(T signal)
    {
      string s = typeof(T).Name;
      var d = (T)signal;
      //string name = ((T)d).I
      //(string, string) s001;
      //(string[], string[]) s002;
      //try
      //{
      //  s001 = (string, string)signal;
      //  s002 = ((ValueTuple)signal).Item.GetType().Name.ToString();

      //}
      //catch (Exception)
      //{

      //  throw;
      //}

    }
    static void Main(string[] args)
    {
      string s00 = "000";
      string s01 = "111";

      TestType<(string, string)>((s00, s01));

      string[] s10 = new string[3] { "1000", "2000", "3000" };
      string[] s11 = new string[3] { "1100", "2100", "3100" };
      TestType<(string[], string[])>((s10, s11));

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