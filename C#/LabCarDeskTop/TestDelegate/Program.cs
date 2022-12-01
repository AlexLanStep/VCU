#define TestDanLabCar
#define DataEmulation

using ContextLabCar.Core.Arif;
using ContextLabCar.Static;
using DryIoc;
using DryIoc.ImTools;
using System;

using System.Text.RegularExpressions;


namespace TestDelegate // Note: actual namespace depends on the project name.
{

    internal class Program
  {
    private static ContainerManager _container = null!;

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
      //      var str = "В строке есть числа 24 222256 21 243";
      ////      var result = Regex.Matches(str, @"\d+").Cast<Match>().Select(x => int.Parse(x.ToString)).ToArray();
      //      int _tmp;
      //      string[] resultarr = str.Where(x => int.TryParse(x, out _tmp)).ToArray();


      //      ss11; ds1; sas; ss11; ee1
      Console.WriteLine(" ----  ---");
      StArithmetic.TestIniciallDan();
      var _x = new OneElement("ee =(5+ ss11 - ds1 - 21.2)*2.0 - 66 + (4* (sas + (ss11 + 101)*2 ) + sas) / ee1 + (10.2+8)/7.3").FuncCalc();

      _container = ContainerManager.GetInstance();
      //IArifmetic _arifmetic = _container.LabCar.Resolve<IArifmetic>();
      //_arifmetic.LoadStr("ee =(5+ ss11)*2.0 - 66 + (4* (sas + (ss11 + 101)*2 ) + sas) / ee + (10+8)/7");

      string s00 = "000";

    }
  }
}

/*
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
 
 
 */