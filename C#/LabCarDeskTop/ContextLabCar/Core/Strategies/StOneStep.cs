
#nullable enable
using ContextLabCar.Static;

namespace ContextLabCar.Core.Strategies;

public interface IStOneStepNew
{
  string StoneName { get; set; }
  Dictionary<string, dynamic> ParamsStrategy { get; set; }
  List<string> CalibrationsLoad { get; set; }
  List<string> CalibrationsActiv { get; set; }
  List<string> GetPoints { get; set; }
  List<string> LResult { get; set; }
  Dictionary<string, dynamic> SetPoints { get; set; }
  List<string> LoggerNamePole { get; set; }
  int TimeWait { get; set; }
  bool Run(Dictionary<string, string> dConfig, bool islog);
  void LoadInitializationPosition(object d);
  void LoadInitializationIf(List<string> list);
  Dictionary<string, dynamic> StCommand { get; set; }
}



public class StOneStep : IStOneStepNew
{
  private delegate bool Dftest(dynamic x0, dynamic x1);
  private delegate bool DTestOr(string name);
  private Dictionary<string, string> _dConfig;
  private int waitCommand;
  public Dictionary<string, dynamic> ParamsStrategy { get; set; } = new ();
  public string StoneName { get; set; }
  public int TimeWait { get; set; }
  public List<string> GetPoints { get; set; } = new ();
  public Dictionary<string, dynamic> SetPoints { get; set; } = new(); 
  public List<string> LResult { get; set; } = new();
  private List<(string, dynamic, object)> lsIf = new();
  private Dictionary<string, List<(string, dynamic, object)>> lsOr = new();
  public List<string> LoggerNamePole { get; set; } = new();
  public Dictionary<string, dynamic> StCommand { get; set; } = new();
  public List<string> CalibrationsLoad { get; set; } = new();
  public List<string> CalibrationsActiv { get; set; } = new();
  public int MaxWaitRez { get; set; }
  private bool _isLogger;
  public StOneStep()
  {
    _isLogger = false;
  }

  private IConnectLabCar inicialCont()
  {
    ContainerManager _container = ContainerManager.GetInstance();
    return _container.LabCar.Resolve<IConnectLabCar>();
  
  }
  private void restartStrategy()
  {
    IConnectLabCar connectLabCar = inicialCont();
    connectLabCar.StopSimulation();
    connectLabCar.StartSimulation();
  }
  private void simstart()=>  inicialCont().StartSimulation();
  private void simstop()=> inicialCont().StopSimulation();


  public void LoadInitializationPosition(object d)
  {
    if (d == null)
      return; 

    var type1 = Regex.Replace(d.GetType().Name.ToLower(), @"[0-9]", "");

    if (type1.Contains("list"))
    {
      GetPoints.Clear();
      ((List<string>)d).ForEach(x => GetPoints.Add(x));
      return;
    }

    if (!type1.Contains("dict")) return;

    SetPoints.Clear();
    foreach (var it in (Dictionary<string, dynamic>)d)
      SetPoints.Add(it.Key, it.Value);
  }
  public void LoadInitializationIf(List<string> list)
  {
    lsIf.Clear();

    list.ForEach
    (
      x =>
      {
        if (x.Contains("("))
        {
          var nameOr = "or" + lsOr.Count().ToString();
          TestOrLoad(x, nameOr);
          lsIf.Add((nameOr, x, (DTestOr)TestOr));
        }
        else
        {
          lsIf.Add(LogicSignal(x));
        }
      }
    );

  }
  private (string, dynamic, Dftest) LogicSignal(string x)
  {
    (string, dynamic, Dftest) F0(string x, string sim, Dftest f)
    {
      var s = x.Split(sim);
      var name = s[0].Trim();
      var value = (dynamic)s[1].Trim();
      return (name, value, f);
    }
    if (x.Contains("=="))
      return F0(x, "==", ResultEq);
    else if (x.Contains(">="))
      return F0(x, ">=", ResultGe);
    else if (x.Contains("<="))
      return F0(x, "<=", ResultLe);
    else if (x.Contains("<"))
      return F0(x, "<", ResultLt);
    else if (x.Contains(">"))
      return F0(x, ">", ResultGt);
    else if (x.Contains("!="))
      return F0(x, "!=", ResultNe);
    return ("null", null, null);
  }
  public void TestOrLoad(string stOr, string name)
  {
    var list = stOr.Replace("(", "").Replace(")", "").Split(",").Select(x => x.Trim()).ToList();
    List<(string, dynamic, object)> lsRez = new();
    list.ForEach(x =>
      {
        lsRez.Add(LogicSignal(x));
      }
    );
    if (lsOr.ContainsKey(name))
      lsOr[name] = lsRez;
    else
      lsOr.Add(name, lsRez);
  }
  public bool TestOr(string nameOr)
  {
    if (string.IsNullOrEmpty(nameOr) || (!lsOr.ContainsKey(nameOr)) || (lsOr[nameOr].Count() < 2))
      return false;

    var ls = lsOr[nameOr];
    var bResult = false;

    var i = 0;
    while ((!bResult) && (i < ls.Count))
    {

      var x01 = (double) LCDan.GetTask(ls[i].Item1);
      double.TryParse(((string)(ls[i].Item2)).Replace('.', ','), out double x1);
      var f0 = (Dftest)ls[i].Item3;
      bResult = bResult || f0(x01, x1);
      i += 1;
    }
    return bResult;
  }
  public virtual bool TestDan()
  {
    _isLogger = (bool)ParamsStrategy["Logger"];
    var bResult = true;

    int i = 0;
    while (bResult && (i < lsIf.Count)) //  && result.TryGetValue(lsIf[i].Item1, out var x0)
    {
      //      double x01 = x0;
      var stype = lsIf[i].Item3.GetType().Name;
      switch (stype)
      {
        case "DTestOr":
          bResult = bResult && TestOr(lsIf[i].Item1);
          break;
        case "Dftest":
        {
          var x0 = (double) LCDan.GetTask(lsIf[i].Item1);
          double x1;
          double.TryParse(((string)lsIf[i].Item2).Replace('.', ','), out x1);
          bResult = bResult && ((Dftest)lsIf[i].Item3)(x0, x1);
          break;
        }
      }

      i += 1;
    }
    return bResult;
  }

  public virtual bool ResultEq(dynamic x0, dynamic x1) => Math.Abs((double)x0 - (double)x1) < 0.0001;  // ==
  public virtual bool ResultNe(dynamic x0, dynamic x1) => Math.Abs((double)x0 - (double)x1) > 0.0001;  // !=
  public virtual bool ResultGe(dynamic x0, dynamic x1) => x0 >= x1; // >= 
  public virtual bool ResultGt(dynamic x0, dynamic x1) => x0 > x1; // >
  public virtual bool ResultLe(dynamic x0, dynamic x1) => x0 <= x1; // <= 
  public virtual bool ResultLt(dynamic x0, dynamic x1) => x0 < x1; // < 


  private void _getDanLabCar()
  {
    if (GetPoints.Count == 0) return;
    Console.WriteLine(" _ Вывод переменных _");
    GetPoints.ForEach(x => Console.WriteLine($" {x} = {LCDan.GetTask(x)}"));
  }
  private void _setDanLabCar()
  {
    if (SetPoints.Count == 0) return;
    Console.WriteLine(" _ Устанавливаем переменные _");

    foreach (var (keySet, valSet) in SetPoints)
      LCDan.SetParam(keySet, valSet);
  }
  private bool _ifDanLabCar()
  {
    if( lsIf.Count<=0 ) return true;

    var rez = false;
    var dt0 = DateTime.Now;
    while ((!rez) && ((DateTime.Now - dt0).Seconds <= MaxWaitRez))
    {
      if (GetPoints.Count > 0) _getDanLabCar();

      rez = TestDan();
      if (!rez) Thread.Sleep(waitCommand);
    }
    return rez;
  }
  private void _setPathInLogger()
  {
    if ((!_isLogger)|| (LoggerNamePole.Count <= 0))
      return;

    List<string> lsPath = new List<string>();
    List<string> lsTask = new List<string>();
    foreach (var item in LoggerNamePole)
    {
      var task = LCDan.GetTaskInfo(item);
      if (task!=null)
      {
        lsPath.Add(task.PathTask);
        lsTask.Add(task.TimeLabCar);
      }
      else 
      {
        var param = LCDan.GetParamInfo(item);
        if (param != null)
        {
          lsPath.Add(param.Signal);
          lsTask.Add("");
        }
      }
    }
    if (lsPath.Count > 0)
        LCDan.AddLogger(_dConfig["NameDir"], _dConfig["FileLogger"], lsPath.ToArray(), lsTask.ToArray());
  }


  public bool Run(Dictionary<string, string> dConfig, bool islog)
  {
    if (StCommand.TryGetValue("sim", out var val) && (val == "on"))
      simstart();

    if (StCommand.ContainsKey("restart"))
      restartStrategy();
    
    this._dConfig= dConfig;
    MaxWaitRez = (int)ParamsStrategy["Maxwait"];
    waitCommand = 1000;
    if(ParamsStrategy.ContainsKey("Logger"))
        ParamsStrategy["Logger"]=islog;
    else
        ParamsStrategy.Add("Logger", islog);

    _isLogger = ParamsStrategy["Logger"]; 

    bool isRezulta = true;

    Console.WriteLine($"  -  Step -> {StoneName} ");
    Thread.Sleep(TimeWait);
    _getDanLabCar();
    _setDanLabCar();
    _setPathInLogger();

    if (_isLogger && StCommand.ContainsKey("logger") && (StCommand["logger"] == "start"))
      LCDan.GetLogger(dConfig["NameDir"])?.Start();

    var isIf = _ifDanLabCar();
    if (!isIf)
    {
        Console.WriteLine("-- ==>  IF не прошел ((((( ----");
        isRezulta = false;
    }

    if (_isLogger && StCommand.ContainsKey("logger") && (StCommand["logger"] == "end"))
      LCDan.GetLogger(dConfig["NameDir"])?.Stop();
    if(!isRezulta)
    {
        try
        {
            LCDan.GetLogger(dConfig["NameDir"])?.Stop();
        }
        catch (Exception)
        {
          // ignored
        }
    }
    if (StCommand.TryGetValue("sim", out var valsim) && (valsim == "off"))
      simstop();

    return isRezulta;
  }




}

/*
 https://www.tutorialsteacher.com/python/magic-methods-in-python
 __lt__(self, other)	To get called on comparison using < operator.
__le__(self, other)	To get called on comparison using <= operator.
__ne__(self, other)	To get called on comparison using != operator.
__eq__(self, other)	To get called on comparison using == operator.
__ge__(self, other)	To get called on comparison using >= operator.
 
 */