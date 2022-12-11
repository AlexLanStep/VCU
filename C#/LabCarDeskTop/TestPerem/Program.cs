
using System.Text.RegularExpressions;

namespace TestDelegate // Note: actual namespace depends on the project name.
{
  internal class Program
  {
    public class Aa
    {
      public int x; public int y;
    }

    public static T? foo1<T>(string name)
    {
      string ss0 = "aaaaa";
      dynamic dd = 23.5;
      string s = typeof(T).Name;

      return (T)Convert.ChangeType(dd, typeof(T));
      //if (s == "String")
      //  return (T)Convert.ChangeType(ss0, typeof(T));

      //if (s == "Object")
      //  return (T)Convert.ChangeType(dd, typeof(T));



      //return (T)Convert.ChangeType(null, typeof(T));
    }

    //int result = op switch
    //{
    //  1 => a + b,
    //  2 => a - b,
    //  3 => a * b,
    //  _ => 0
    //};



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

    static void TestType1<T>(string signal)
    {
      string s = typeof(T).Name;
      var d = signal;

      Console.WriteLine($"  {s}  {signal} ");
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
      //TestType1<dynamic>("dynamic");
      //TestType1<string>("string");
      //TestType1<bool>("bool");
      //TestType1<Aa>("Aa");

      string ss = foo1<string>("------");
      dynamic ttt = foo1<dynamic>("-!!!!!!---");
//      byte[]? ttt11 = foo1<byte[]>("-!!!!!!---");


      string s00 = "000";

    }
  }
}

