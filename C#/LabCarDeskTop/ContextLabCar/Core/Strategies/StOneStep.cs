﻿
#nullable enable
using System;
using System.Collections.Generic;

namespace ContextLabCar.Core.Strategies;

public class StOneStep : IStOneStep
{
  public delegate bool Dftest(dynamic x0, dynamic x1);
  public int TimeWait { get; set; }
  public Dictionary<string, dynamic> GetPoints { get; set; } = new ();
  public Dictionary<string, dynamic> SetPoints { get; set; } = new(); 
  public List<string> LResult { get; set; } = new();
  public List<string> LIf { get; set; } = new();
  public List<string> LIfOr { get; set; } = new();

  private List<(string, dynamic, Dftest)> lsRez = new();
  private List<(string, dynamic, Dftest)> lsIf = new();
  private List<(string, dynamic, Dftest)> lsIfOr = new();

  public dynamic? ReadSetPoints(string name) => SetPoints.TryGetValue(name, value: out var value) ? value : null;
  public dynamic? ReadGetPoints(string name) => GetPoints.TryGetValue(name, value: out var value) ? value : null;

  public List<string> LoggerNamePole { get; set; } = new();
  public Dictionary<string, dynamic> StCommand { get; set; } = new();


  public void LoadInitializationPosition(object d)
  {
    if(d==null)
      return; 

    var type1 = Regex.Replace(d.GetType().Name.ToLower(), @"[0-9]", "");

    if (type1.Contains("list"))
    {
      GetPoints.Clear();
            ((List<string>)d).ForEach(
                x => { if (!GetPoints.ContainsKey(x))
                            GetPoints.Add(x, 0);
                        else
                            GetPoints[x] = 0;
                      }
        );
      return;
    }

    if (!type1.Contains("dict")) return;

    SetPoints.Clear();
    foreach (var it in (Dictionary<string, dynamic>)d)
      SetPoints.Add(it.Key, it.Value);
  }

  public void AddGetPoints(string key, dynamic value)
  {
    GetPoints.TryAdd(key, value);
  }
  public void LoadInitializationRez(List<string> list) 
  {
    (string, dynamic, Dftest) F0(string x, Dftest f)
    {
      var s = x.Split("==");
      var name = s[0].Trim();
      var value = (dynamic) s[1].Trim();
      return (name, value, f);
    }

    lsRez.Clear();
    list.ForEach
    (
      x =>
      {
        if (x.Contains("=="))
          lsRez.Add(F0(x, ResultEq));
        else if (x.Contains(">="))
          lsRez.Add(F0(x, ResultGe));
        else if (x.Contains("<="))
          lsRez.Add(F0(x, ResultLe));
        else if (x.Contains("<"))
          lsRez.Add(F0(x, ResultLt));
        else if (x.Contains(">"))
          lsRez.Add(F0(x, ResultGt));
        else if (x.Contains("!="))
          lsRez.Add(F0(x, ResultNe));
      }
    ); 

    LResult = new List<string>(list);
  }

  public List<string> LoadInitializationLogic(ref List<(string, dynamic, Dftest)> ls, List<string> list)
  {
    List<(string, dynamic, Dftest)> _lsRez = ls;

    (string, dynamic, Dftest) F0(string x, string sim, Dftest f)
      {
        var s = x.Split(sim);
        var name = s[0].Trim();
        var value = (dynamic)s[1].Trim();
        return (name, value, f);
      }

    _lsRez.Clear();
    list.ForEach
    (
      x =>
      {
        if (x.Contains("=="))
          _lsRez.Add(F0(x, "==", ResultEq));
        else if (x.Contains(">="))
          _lsRez.Add(F0(x, ">=", ResultGe));
        else if (x.Contains("<="))
          _lsRez.Add(F0(x, "<=", ResultLe));
        else if (x.Contains("<"))
          _lsRez.Add(F0(x, "<", ResultLt));
        else if (x.Contains(">"))
          _lsRez.Add(F0(x, ">", ResultGt));
        else if (x.Contains("!="))
          _lsRez.Add(F0(x, "!=", ResultNe));
      }
    );

//    LResult = new List<string>(list);
    return new List<string>(list);
  }

  public void LoadInitializationRez1(List<string> list) 
  {
    LResult = LoadInitializationLogic(ref lsRez, list);
  }
  public void LoadInitializationIf1(List<string> list)
  {
    LIf = LoadInitializationLogic(ref lsIf, list);
  }
  public void LoadInitializationIfOr1(List<string> list)
  {
    LIfOr = LoadInitializationLogic(ref lsIfOr, list);
  }

  public virtual bool TestDan(Dictionary<string, dynamic> result)
  {
    var bResult = true;

    if (result.Count == 0)
      return false;

    foreach (var it in lsRez)
    {
      var name = it.Item1;
      if(result.TryGetValue(name, out var x0))
      {
        double x01 = x0;
        double x1;
        double.TryParse(((string)(it.Item2)).Replace('.',','), out x1);
        bResult = bResult && it.Item3(x01, x1);
      }
      else
      {
        Console.WriteLine($" Error not {name} ");
        return false;
      }
    }

    return bResult;
  }
  public virtual bool TestDanNew(Dictionary<string, dynamic> result, List<(string, dynamic, Dftest)> ls)
  {
    var bResult = true;

    if (result.Count == 0)
      return false;

    //foreach (var it in ls)
    //{
    //  var name = it.Item1;
    //  if (result.TryGetValue(name, out var x0))
    //  {
    //    double x01 = x0;
    //    //double x1;
    //    double.TryParse(((string)(it.Item2)).Replace('.', ','), out double x1);
    //    bResult = bResult && it.Item3(x01, x1);
    //    if (!bResult)
    //      return false;
    //  }
    //  else
    //  {
    //    Console.WriteLine($" Error not {name} ");
    //    return false;
    //  }
    //}

    int i = 0;
    //while (bResult && (i < ls.Count))
    //{
    //  if (result.TryGetValue(ls[i].Item1, out var x0))
    //  {
    //    double x01 = x0;
    //    double.TryParse(((string)(ls[i].Item2)).Replace('.', ','), out double x1);
    //    bResult = bResult && ls[i].Item3(x01, x1);
    //  }
    //  else
    //  {
    //    Console.WriteLine($" Error not {ls[i].Item1} ");
    //    return false;
    //  }
    //  i += 1;
    //}
    while (bResult && (i < ls.Count) && result.TryGetValue(ls[i].Item1, out var x0))
    {
//      double x01 = x0;
      double.TryParse(((string)(ls[i].Item2)).Replace('.', ','), out double x1);
            double _x0 =(double) x0;
            double _x1 = (double) x1;
      bResult = bResult && ls[i].Item3(_x0, _x1);
      //else
      //{
      //  Console.WriteLine($" Error not {ls[i].Item1} ");
      //  return false;
      //}
      i += 1;
    }
    return bResult;
  }

  public virtual bool TestDanOr(Dictionary<string, dynamic> result, List<(string, dynamic, Dftest)> ls)
  {
    var bResult = false;

    if (result.Count == 0)
      return false;

    //foreach (var it in ls)
    //{
    //  var name = it.Item1;
    //  if (result.TryGetValue(name, out var x0))
    //  {
    //    double x01 = x0;
    //    double.TryParse(((string)(it.Item2)).Replace('.', ','), out double x1);
    //    bResult = bResult || it.Item3(x01, x1);
    //    if (bResult)
    //      return true;
    //  }
    //  else
    //  {
    //    Console.WriteLine($" Error not {name} ");
    //    return false;
    //  }
    //}

    int i = 0;
    while ((!bResult) && (i < ls.Count) && result.TryGetValue(ls[i].Item1, out var x0))
    {
      double x01 = x0;
      double.TryParse(((string)(ls[i].Item2)).Replace('.', ','), out double x1);
      bResult = bResult || ls[i].Item3(x01, x1);
    }

    return bResult;
  }


  public virtual bool TestIf(Dictionary<string, dynamic> result) => TestDanNew(result, lsIf);
  public virtual bool TestIfOr(Dictionary<string, dynamic> result) => TestDanOr(result, lsIfOr);
  public virtual bool TestRez(Dictionary<string, dynamic> result) => TestDanNew(result, lsRez);

  public virtual bool ResultEq(dynamic x0, dynamic x1) => Math.Abs((double)x0 - (double)x1) < 0.000001;  // ==
  public virtual bool ResultNe(dynamic x0, dynamic x1) => Math.Abs((double)x0 - (double)x1) > 0.000001;  // !=
  public virtual bool ResultGe(dynamic x0, dynamic x1) => x0 >= x1; // >= 
  public virtual bool ResultGt(dynamic x0, dynamic x1) => x0 > x1; // >
  public virtual bool ResultLe(dynamic x0, dynamic x1) => x0 <= x1; // <= 
  public virtual bool ResultLt(dynamic x0, dynamic x1) => x0 < x1; // < 


}

/*
 https://www.tutorialsteacher.com/python/magic-methods-in-python
 __lt__(self, other)	To get called on comparison using < operator.
__le__(self, other)	To get called on comparison using <= operator.
__ne__(self, other)	To get called on comparison using != operator.
__eq__(self, other)	To get called on comparison using == operator.
__ge__(self, other)	To get called on comparison using >= operator.
 
 */