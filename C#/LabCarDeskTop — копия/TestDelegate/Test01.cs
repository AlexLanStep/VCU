
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.Intrinsics.Arm;
using System.Xml.Linq;

namespace TestDelegate;

public class Test01
{
  public delegate bool Dftest(dynamic x0, dynamic x1);
  public delegate bool DTestOr(string name);
  private List<(string, dynamic, object)> lsIf = new();
  private Dictionary<string, List<(string, dynamic, object)>> lsOr = new();
  private ConcurrentDictionary<string, dynamic> result = new();
  private Random rand = new Random();

  private List<(string, dynamic, object)> loadStrategy(List<string> list)
  {
    //    List<(string, dynamic, Dftest)> _lsRez = new();
    List<(string, dynamic, object)> _lsRez = new();

    (string, dynamic, Dftest) F0(string x, Dftest f)
    //    (string, dynamic, object) F0(string x, object f)
    {
      var s = x.Split("==");
      var name = s[0].Trim();
      var value = (dynamic)s[1].Trim();
      return (name, value, f);
    }

    _lsRez.Clear();
    list.ForEach
    (
      x =>
      {
        if (x.Contains("("))
        {
          string nameOr = "or" + lsOr.Count().ToString();
          TestOrLoad(x, nameOr);
          _lsRez.Add((nameOr, x, (DTestOr)TestOr));
        }

        else
        {
          _lsRez.Add(logicSignal(x));
          var it = _lsRez.ElementAt(_lsRez.Count() - 1);
          var xx = rand.Next();
          result.AddOrUpdate(it.Item1, xx, (_, _) => xx);
        }
      }
    );
    return _lsRez;
  }
  public bool Run()
  {
    List<string> list = new List<string>() { "Low_xxxBeam_State<15.0", "(VCU_DesInvMode==1.0,  Low_Beam_State==1.0)", "Low_Beam_State==1.0", "Low_Beam11_State>=1.0" };

    lsIf = loadStrategy(list);

    var bResult = true;

    if (result.Count == 0)
      return false;

    int i = 0;

    while (bResult && (i < lsIf.Count)) //  && result.TryGetValue(lsIf[i].Item1, out var x0)
    {
//      double x01 = x0;
      var stype = lsIf[i].Item3.GetType().Name;
      if("DTestOr"==stype)
      {

      }
      if ("Dftest" == stype)
      {

      }

      //      var stype = GetType(lsIf[i].Item3).Name;
      //      double.TryParse(((string)(ls[i].Item2)).Replace('.', ','), out double x1);
      //      bResult = bResult && ls[i].Item3(x01, x1);
      i += 1;
    }
    return bResult;
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

  public dynamic ReadLabCar(string pole)
  {

    return 10;
  }
  public void TestOrLoad(string stOr, string name)
  {
    var list = stOr.Replace("(","").Replace(")", "").Split(",").Select(x=> x.Trim()).ToList();
    List<(string, dynamic, object)> _lsRez = new();
    list.ForEach( x => 
      { 
        _lsRez.Add(logicSignal(x));
        var it = _lsRez.ElementAt(_lsRez.Count() - 1);
        var xx = rand.Next();
        result.AddOrUpdate(it.Item1, xx, (_, _) => xx);
      }
    );
    if (lsOr.ContainsKey(name))
      lsOr[name] = _lsRez;
    else
      lsOr.Add(name, _lsRez);
  }

  public bool TestOr(string nameOr)
  {
    if (string.IsNullOrEmpty(nameOr) || (!lsOr.ContainsKey(nameOr)) || (lsOr[nameOr].Count()<2))
      return false;


    var ls = lsOr[nameOr];
    var bResult = false;

    int i = 0;
    while ((!bResult) && (i < ls.Count) && result.TryGetValue(ls[i].Item1, out var x0))
    {
      double x01 = x0;
      double.TryParse(((string)(ls[i].Item2)).Replace('.', ','), out double x1);
      var _f0 = (Dftest)ls[i].Item3;
      bResult = bResult || _f0(x01, x1);
    }
    return bResult;
  }
  public virtual bool ResultEq(dynamic x0, dynamic x1) => Math.Abs((double)x0 - (double)x1) < 0.000001;  // ==
  public virtual bool ResultNe(dynamic x0, dynamic x1) => Math.Abs((double)x0 - (double)x1) > 0.000001;  // !=
  public virtual bool ResultGe(dynamic x0, dynamic x1) => x0 >= x1; // >= 
  public virtual bool ResultGt(dynamic x0, dynamic x1) => x0 > x1; // >
  public virtual bool ResultLe(dynamic x0, dynamic x1) => x0 <= x1; // <= 
  public virtual bool ResultLt(dynamic x0, dynamic x1) => x0 < x1; // < 



}
