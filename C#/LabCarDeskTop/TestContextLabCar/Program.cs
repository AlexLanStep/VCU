// #define DataEmulation
//#if DataEmulation
//#else
//#endif

using ContextLabCar.Core;
using ContextLabCar.Core.Config;
using ContextLabCar.Core.Strategies;
using ContextLabCar.Static;
using DryIoc;
using Newtonsoft.Json;
using System;

namespace TestContextLabCar;  // Note: actual namespace depends on the project name.
public class MyArg
{
  public string Path { get; set; }
  public int Repeat { get; set; }
  public string LabCarLog { get; set; }

}
internal class Program
{
  private static ContainerManager _container = null!;
  static void Main(string[] args)
  {

    if ((args.Length == 0) || (!File.Exists(args[0])))
    {
      Console.WriteLine("--   ERROR  -- нет json файла");
      return;
    }
    List<MyArg> _myArgs = new List<MyArg>();

    try
    {
      _myArgs = JsonConvert.DeserializeObject<List<MyArg>>(File.ReadAllText(args[0]));
    }
    catch (Exception)
    {
      Console.WriteLine("--   ERROR  -- проблема с заргузкой файла jason ");
      return;
    }

  
    int i = 0;
//    Config = JsonConvert.DeserializeObject<GlobalConfigLabCar>(File.ReadAllText(_pathFileConfig));

    _container = ContainerManager.GetInstance();
    _container.LabCar.Resolve<IConnectLabCar>();
//    _container.LabCar.Resolve<IStrategyDanJson>();
    var isRezulta = true;
    string _txtReport = "";
    string _dirRepost = "";
    foreach (var it in _myArgs)
    {
      if (!Directory.Exists(it.Path))
        continue;

      int Repeat = it.Repeat > 0? it.Repeat:1;
      bool loggerCar = it.LabCarLog != null;
      string _txtReportLoc = "";

      Console.WriteLine($"=========   Stratedy {i}   =======");
      for (int j = 0; j < Repeat; j++)
      {
        var testLab = _container.LabCar.Resolve<IBaseContext>();
        testLab.Initialization(it.Path, loggerCar);
        testLab.RunTest();
        isRezulta = testLab.IsResult;
        _txtReportLoc = isRezulta 
              ? testLab.DConfig["Excellent"] 
              : testLab.DConfig["Badly"];
        _dirRepost= testLab.DConfig["DirReport"];
        if (!isRezulta)
        {
          Console.WriteLine($" Error в стратегии {i+1} на {j+1} повторении ");
          _txtReport += _txtReportLoc;
          File.WriteAllText(_dirRepost+"\\error_"+DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"), _txtReport);
          Environment.Exit(-10);
        }
//        DConfig["Excellent"] = ss[0];
//        DConfig["Badly"] = ss[1];
      }
      _txtReport += _txtReportLoc;
      i += 1;
    }
    File.WriteAllText(_dirRepost + "\\good_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"), _txtReport);



    //    const int nStartStrategy = 0;
    //    const int nEndStrategy = 1;
    //    var i = nStartStrategy;
    //while (isRezulta && i< nEndStrategy)
    //{
    //  Console.WriteLine($"=========   Stratedy {i}   =======");
    //  var testLab = _container.LabCar.Resolve<IBaseContext>();
    //  testLab.Initialization(listDir.ElementAt(i));
    //  testLab.RunTest();
    //  isRezulta = testLab.IsResult;
    //  i += 1;
    //}


    Console.WriteLine("==== END === !!!! ");
  }
}


//var listDir = new List<string>()
//      { @"D:\TestSystem\Moto\Strategies\St0",
//        @"D:\TestSystem\Moto\Strategies\St1",
//        @"D:\TestSystem\Moto\Strategies\St2",
//        @"D:\TestSystem\Moto\Strategies\St3",
//        @"D:\TestSystem\Moto\Strategies\St4",
//        @"D:\TestSystem\Moto\Strategies\St5",
//        @"D:\TestSystem\Moto\Strategies\St6",
//        @"D:\TestSystem\Moto\Strategies\St7"
//      };


//_myArgs.Add(new MyArg() { Path = @"D:\TestSystem\Moto\Strategies\St0", Repeat = 2, LabCarLog = "log" });
//_myArgs.Add(new MyArg() { Path = @"D:\TestSystem\Moto\Strategies\St1" });
//_myArgs.Add(new MyArg() { Path = @"D:\TestSystem\Moto\Strategies\St2", Repeat = 4 });
//_myArgs.Add(new MyArg() { Path = @"D:\TestSystem\Moto\Strategies\St3", LabCarLog = "log" });
//_myArgs.Add(new MyArg() { Path = @"D:\TestSystem\Moto\Strategies\St4", Repeat = 5, LabCarLog = "log" });
//_myArgs.Add(new MyArg() { Path = @"D:\TestSystem\Moto\Strategies\St5", Repeat = 2});

//var s = JsonConvert.SerializeObject(_myArgs);
//File.WriteAllText("E:\\1.json", s);
