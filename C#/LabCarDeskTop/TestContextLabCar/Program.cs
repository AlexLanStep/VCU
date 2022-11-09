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
  public string ReStart { get; set; }

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
      bool _restart = it.ReStart != null;

      Console.WriteLine($"=========   Stratedy {i+1}   =======");
      for (int j = 0; j < Repeat; j++)
      {
        var testLab = _container.LabCar.Resolve<IBaseContext>();
        if (_restart)
          testLab.ReStart();

        testLab.Initialization(it.Path, loggerCar);
        testLab.RunTest();
        isRezulta = testLab.IsResult;
        _dirRepost = testLab.DConfig["DirReport"];

        _txtReportLoc = isRezulta 
              ? testLab.DConfig["Excellent"] 
              : testLab.DConfig["Badly"];

        if (!isRezulta)
        {
          string ss = $" Error в стратегии {i + 1} на {j + 1} повторении ";
          Console.WriteLine(ss);
          _txtReport += _txtReportLoc + $"\n\r  {ss}  \n\r";
          File.WriteAllText(_dirRepost+"\\error_"+DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".txt", _txtReport);
          Environment.Exit(-10);
        }
      }
      _txtReport += _txtReportLoc;
      i += 1;
    }
    File.WriteAllText(_dirRepost + "\\good_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")+".txt", _txtReport);
    Console.WriteLine("==== END === !!!! ");
  }
}

