// #define DataEmulation
//#if DataEmulation
//#else
//#endif

using ContextLabCar.Core;
using ContextLabCar.Core.Strategies;
using ContextLabCar.Static;
using DryIoc;
using Newtonsoft.Json;

namespace TestContextLabCar;  // Note: actual namespace depends on the project name.
public class MyArg
{
#pragma warning disable CS8618
  public string Path { get; set; }
  public int Repeat { get; set; }
  public string LabCarLog { get; set; }
  public string ReStart { get; set; }
#pragma warning restore CS8618

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
    List<MyArg>? myArgs;

    try
    {
      myArgs = JsonConvert.DeserializeObject<List<MyArg>>(File.ReadAllText(args[0]));
    }
    catch (Exception)
    {
      Console.WriteLine("--   ERROR  -- проблема с заргузкой файла jason ");
      return;
    }

  
    var i = 0;
//    Config = JsonConvert.DeserializeObject<GlobalConfigLabCar>(File.ReadAllText(_pathFileConfig));

    _container = ContainerManager.GetInstance();
    _container.LabCar.Resolve<IConnectLabCar>();
    // ReSharper disable once RedundantAssignment
    var isRezulta = true;
    var txtReport = "";
    var dirRepost = "";

    if (myArgs == null)
      throw new MyException("Нет входных данных ", -6);

    foreach (var it in myArgs)
    {
      if (!Directory.Exists(it.Path))
        continue;

      var repeat = it.Repeat > 0? it.Repeat:1;
      var loggerCar = it.LabCarLog != null;
      var txtReportLoc = "";
      var restart = it.ReStart != null;

      Console.WriteLine($"=========   Stratedy {i+1}   =======");
      for (var j = 0; j < repeat; j++)
      {
        var testLab = _container.LabCar.Resolve<IBaseContext>();
        if (restart)
          testLab.ReStart();

        testLab.Initialization(it.Path, loggerCar);
        testLab.RunTest();
        isRezulta = testLab.IsResult;
        dirRepost = testLab.DConfig["DirReport"];

        txtReportLoc = isRezulta 
              ? testLab.DConfig["Excellent"] 
              : testLab.DConfig["Badly"];

        if (!isRezulta)
        {
          string ss = $" Error в стратегии {i + 1} на {j + 1} повторении ";
          Console.WriteLine(ss);
          txtReport += txtReportLoc + $"\n\r  {ss}  \n\r";
          File.WriteAllText(dirRepost+"\\error_"+DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".txt", txtReport);
          Environment.Exit(-10);
        }
      }
      txtReport += txtReportLoc;
      i += 1;
    }
    File.WriteAllText(dirRepost + "\\good_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")+".txt", txtReport);
    Console.WriteLine("==== END === !!!! ");
  }
}

