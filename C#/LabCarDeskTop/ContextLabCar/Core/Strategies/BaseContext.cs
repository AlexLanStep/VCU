
using ContextLabCar.Core.Config;
using ETAS.EE.Scripting;
using System.Xml.Linq;

namespace ContextLabCar.Core.Strategies;

public interface IBaseContext
{
  Dictionary<string, string> DConfig { get; set; }
  //  Dictionary<string, dynamic> ParamsStrategy { get; set; }
  bool IsResult { get; set; }
  void Initialization(string pathdir, bool islogLabCar=false);
  void RunTest();
  void ReStart();
  void StartSimul();
  void StopSimult();
}
public class BaseContext: IBaseContext
{
  #region Data
  public bool IsResult { get; set; }
  public Dictionary<string, string> DConfig { get; set; }
  public Dictionary<string, dynamic> ParamsStrategy { get; set; }
  private List<IStOneStepNew> _lsStOneStep = new List<IStOneStepNew>();
  private bool _islogLabCar;
  #endregion
  private IConnectLabCar _iConLabCar;
  public BaseContext(IConnectLabCar iConLabCar)
  {
    _iConLabCar = iConLabCar;
    IsResult = true;
    DConfig = new Dictionary<string, string>();
    _islogLabCar = false;
  }
    
   public void ReStart()
  {
    StopSimult();
    StartSimul();
  }

  public void StartSimul() =>_iConLabCar.StartSimulation();
  public void StopSimult()=>_iConLabCar.StopSimulation();

  public void Initialization(string pathdir, bool islogLabCar = false)
  {
    _islogLabCar = islogLabCar;
    if (!Directory.Exists(pathdir))
      throw new MyException($"Нет каталога - {pathdir}", -1);

    DConfig.Add("StDir", pathdir);
    weFormThePaths();
    var parser = new ParserJson(DConfig, ref _lsStOneStep);

    foreach (var (key, value) in parser.Config.PathLabCar)
    {
      if(DConfig.ContainsKey(key))
        DConfig[key] = value;
      else
        DConfig.Add(key, value);
    }

    parser.ParamsStrategy?.Add("Logger", true);
    ParamsStrategy = new Dictionary<string, dynamic>(parser.ParamsStrategy);


    Console.WriteLine("Грузим переменные для старта LabCar");
    _iConLabCar.Initialization(DConfig["Workspace"], DConfig["Experiment"]);
    Console.WriteLine("Подключение к LabCar");
    _iConLabCar.Connect();
    Console.WriteLine($"Инициализация параметров для стратегии {ParamsStrategy["Name"]}:");

    parser.LoadJsonStrategy();
    if(parser.ParamsStrategy.ContainsKey("Logger")) 
        parser.ParamsStrategy["Logger"]=_islogLabCar;
    else
        parser.ParamsStrategy.Add("Logger", _islogLabCar);

    var reportFile = DConfig["StDir"] + "\\report.txt"; 
    if (File.Exists(reportFile))
    {
      var s = File.ReadAllText(reportFile);
      var ss = s.Split("#Badly#");
      DConfig["Excellent"] = ss[0];
      DConfig["Badly"] = ss[1];
    }
    else
    {
      DConfig.Add("Excellent", $"Стратегия {parser.ParamsStrategy["Name"]} работает! Отлично! \n\r");
      DConfig.Add("Badly", $"Стратегия {parser.ParamsStrategy["Name"]} не работает((! Опять бардак \n\r");
    }

    parser.RunInicialDan();

  }
  private void weFormThePaths()
  {
    var pathdir = DConfig["StDir"];
    var dirs = pathdir.Split('\\');
    string nameDir = dirs[^1];
    DConfig.Add("NameDir", nameDir);

    string pathDan = pathdir + "\\Dan";
    if (!Directory.Exists(pathDan))
      Directory.CreateDirectory(pathDan);
    DConfig.Add("FileLogger", pathDan + "\\" + "Logger" + nameDir + "_.dat");

    var count = dirs.Length;
    var dirs0 = dirs.ToList();
    dirs0.RemoveAt(count - 1);
    dirs0.RemoveAt(count - 2);
    string pathConfigDir = string.Join("\\", dirs0.ToArray());
    DConfig.Add("DirConfig", pathConfigDir);
    DConfig.Add("DirCalibrat", pathConfigDir+ "\\Calibration");
    if (!Directory.Exists(DConfig["DirCalibrat"]))
        Directory.CreateDirectory(DConfig["DirCalibrat"]);

    DConfig.Add("DirReport", pathConfigDir + "\\Report");
    if (!Directory.Exists(DConfig["DirReport"]))
      Directory.CreateDirectory(DConfig["DirReport"]);


  }
  public void RunTest()
  {
    Console.WriteLine($"  -  Start TEST - {ParamsStrategy["Name"]} ");

    var numSten =0; 
    while (IsResult && numSten < _lsStOneStep.Count)
    {
      IsResult = _lsStOneStep[numSten].Run(DConfig, _islogLabCar);
      numSten += 1;

    }
    Console.WriteLine(IsResult ? "-- !!!!  Test Прошел !!!!" : "-- ==>  Test ERROR ((((( ----");
  }
}

