﻿
#nullable enable
using ContextLabCar.Core.Config;
using ContextLabCar.Core.Interface;
using ETAS.EE.Scripting;
using System;
using System.Collections.Generic;
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
  bool Run();
  bool IsRezulta { get; set; }
  void LoadInitializationPosition(object d);
  void LoadInitializationIf(List<string> list);
  Dictionary<string, dynamic> StCommand { get; set; }
}



public class StOneStepNew : IStOneStepNew
{
  private IBaseContext _iBasaContext;
  private IConnectLabCar _connect;

  private delegate bool Dftest(dynamic x0, dynamic x1);
  private delegate bool DTestOr(string name);
  private Dictionary<string, string> dConfig;
  private int waitCommand;
  public bool IsRezulta { get; set; } = true;
  public Dictionary<string, dynamic> ParamsStrategy { get; set; } = new ();
  public string StoneName { get; set; }
  public int TimeWait { get; set; }
  public List<string> GetPoints { get; set; } = new ();
  public Dictionary<string, dynamic> SetPoints { get; set; } = new(); 
  public List<string> LResult { get; set; } = new();
  private List<string> getListDan = new ();

  private List<(string, dynamic, object)> lsIf = new();
  private Dictionary<string, List<(string, dynamic, object)>> lsOr = new();
  private ConcurrentDictionary<string, dynamic> result = new();

  public dynamic? ReadSetPoints(string name) => SetPoints.TryGetValue(name, value: out var value) ? value : null;

  public List<string> LoggerNamePole { get; set; } = new();
  public Dictionary<string, dynamic> StCommand { get; set; } = new();
  public List<string> CalibrationsLoad { get; set; } = new();
  public List<string> CalibrationsActiv { get; set; } = new();
  public int MaxWaitRez { get; set; }
  private bool _isLogger;
  public StOneStepNew(IConnectLabCar iConLabCar, IBaseContext iBasaContext)
  {
    _connect = iConLabCar;
    _iBasaContext = iBasaContext;
    dConfig = _iBasaContext.DConfig;
    _isLogger = false;
  }

  public void LoadInitializationPosition(object d)
  {
    if(d==null)
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
    List<(string, dynamic, object)> _lsRez = new();

    lsIf.Clear();

    list.ForEach
    (
      x =>
      {

        if (x.Contains("("))
        {
          string nameOr = "or" + lsOr.Count().ToString();
          TestOrLoad(x, nameOr);
          lsIf.Add((nameOr, x, (DTestOr)TestOr));
        }

        else
        {
          lsIf.Add(logicSignal(x));
//          var it = _lsRez.ElementAt(_lsRez.Count() - 1);
//          var xx = rand.Next();
//          result.AddOrUpdate(it.Item1, xx, (_, _) => xx);
        }
      }
    );

  }
  private (string, dynamic, Dftest) logicSignal(string x)
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
    List<(string, dynamic, object)> _lsRez = new();
    list.ForEach(x =>
      {
        _lsRez.Add(logicSignal(x));
        var it = _lsRez.ElementAt(_lsRez.Count() - 1);
//        var xx = rand.Next();
//        result.AddOrUpdate(it.Item1, xx, (_, _) => xx);
      }
    );
    if (lsOr.ContainsKey(name))
      lsOr[name] = _lsRez;
    else
      lsOr.Add(name, _lsRez);
  }
  public bool TestOr(string nameOr)
  {
    if (string.IsNullOrEmpty(nameOr) || (!lsOr.ContainsKey(nameOr)) || (lsOr[nameOr].Count() < 2))
      return false;

    var ls = lsOr[nameOr];
    var bResult = false;

    int i = 0;
    while ((!bResult) && (i < ls.Count))
    {

      double x01 = (double) LCDan.GetTask(ls[i].Item1);
      double.TryParse(((string)(ls[i].Item2)).Replace('.', ','), out double x1);
      var _f0 = (Dftest)ls[i].Item3;
      bResult = bResult || _f0(x01, x1);
      i += 1;
    }
    return bResult;
  }
  public virtual bool TestDan()
  {
    var bResult = true;

    //foreach (var it in lsIf)
    //{
    //  var name = it.Item1;
    //  if (result.TryGetValue(name, out var x0))
    //  {
    //    double x01 = x0;
    //    double x1;
    //    double.TryParse(((string)(it.Item2)).Replace('.', ','), out x1);
    //    //        bResult = bResult && it.Item3(x01, x1);
    //  }
    //  else
    //  {
    //    Console.WriteLine($" Error not {name} ");
    //    return false;
    //  }
    //}

    int i = 0;
    while (bResult && (i < lsIf.Count)) //  && result.TryGetValue(lsIf[i].Item1, out var x0)
    {
      //      double x01 = x0;
      var stype = lsIf[i].Item3.GetType().Name;
      if ("DTestOr" == stype)
      {
        bResult = bResult && TestOr(lsIf[i].Item1);
      }
      if ("Dftest" == stype)
      {
        double x0 = (double) LCDan.GetTask(lsIf[i].Item1);
        double x1;
        double.TryParse(((string)(lsIf[i].Item2)).Replace('.', ','), out x1);
//        var fun =(Dftest) lsIf[i].Item3;
        bResult = bResult && ((Dftest)lsIf[i].Item3)(x0, x1);
//        bResult = bResult && fun(x0, x1);
      }

      //      var stype = GetType(lsIf[i].Item3).Name;
      //      double.TryParse(((string)(ls[i].Item2)).Replace('.', ','), out double x1);
      //      bResult = bResult && ls[i].Item3(x01, x1);
      i += 1;
    }
    return bResult;
  }
  public virtual bool TestDanNew(Dictionary<string, dynamic> result, List<(string, dynamic, object)> ls)
  {
    var bResult = true;

    if (result.Count == 0)
      return false;

    int i = 0;
    while (bResult && (i < ls.Count) && result.TryGetValue(ls[i].Item1, out var x0))
    {
      double.TryParse(((string)(ls[i].Item2)).Replace('.', ','), out double x1);
            double _x0 =(double) x0;
            double _x1 = (double) x1;
      i += 1;
    }
    return bResult;
  }
  public virtual bool TestDanOr(Dictionary<string, dynamic> result, List<(string, dynamic, object)> ls)
  {
    var bResult = false;

    if (result.Count == 0)
      return false;

    int i = 0;
    while ((!bResult) && (i < ls.Count) && result.TryGetValue(ls[i].Item1, out var x0))
    {
      double x01 = x0;
      double.TryParse(((string)(ls[i].Item2)).Replace('.', ','), out double x1);
//      bResult = bResult || ls[i].Item3(x01, x1);
      i += 1;
    }

    return bResult;
  }
  public virtual bool ResultEq(dynamic x0, dynamic x1) => Math.Abs((double)x0 - (double)x1) < 0.000001;  // ==
  public virtual bool ResultNe(dynamic x0, dynamic x1) => Math.Abs((double)x0 - (double)x1) > 0.000001;  // !=
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

    List<string> _lsPath = new List<string>();
    List<string> _lsTask = new List<string>();
    foreach (var item in LoggerNamePole)
    {
      var _task = LCDan.GetTaskInfo(item);
      if (_task!=null)
      {
        _lsPath.Add(_task.PathTask);
        _lsTask.Add(_task.TimeLabCar);
      }
      else 
      {
        var _param = LCDan.GetParamInfo(item);
        if (_param != null)
        {
          _lsPath.Add(_param.Signal);
          _lsTask.Add("");
        }
      }
    }
    if (_lsPath.Count > 0)
      LCDan.AddLogger(dConfig["NameDir"], dConfig["FileLogger"], _lsPath.ToArray(), _lsTask.ToArray());
  }


  public bool Run()
  {
    MaxWaitRez = ParamsStrategy["Maxwait"];
    waitCommand = 1000;
    _isLogger = ParamsStrategy.ContainsKey("Logger")? ParamsStrategy["Logger"]:false; 

    IsRezulta = true;

    Console.WriteLine($"  -  Step -> {StoneName} ");

    _getDanLabCar();
    _setDanLabCar();
    _setPathInLogger();

    if (_isLogger && StCommand.ContainsKey("logger") && (StCommand["logger"] == "start"))
      LCDan.GetLogger(dConfig["NameDir"])?.Start();

    var _isIf = _ifDanLabCar();
    if (!_isIf)
    {
        Console.WriteLine("-- ==>  IF не прошел ((((( ----");
        IsRezulta = false;
    }

    if (_isLogger && StCommand.ContainsKey("logger") && (StCommand["logger"] == "end"))
      LCDan.GetLogger(dConfig["NameDir"])?.Stop();

    return IsRezulta;
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