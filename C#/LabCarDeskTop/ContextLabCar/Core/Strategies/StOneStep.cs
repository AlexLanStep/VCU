

using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace ContextLabCar.Core.Strategies;

public class StOneStep : IStOneStep
{
  private delegate bool Dftest(dynamic x0, dynamic x1);
  public int TimeWait { get; set; }
  public Dictionary<string, dynamic> GetPoints { get; set; } = new ();
  public Dictionary<string, dynamic> SetPoints { get; set; } = new(); 
  public List<string> LRezult { get; set; } = new();
  
  public dynamic? ReadSetPoints(string name) => SetPoints.TryGetValue(name, value: out dynamic value) ? value : null;
  public dynamic? ReadGetPoints(string name) => GetPoints.TryGetValue(name, value: out dynamic value) ? value : null;

  public void LoadInicialPosition(object d)
  {
    if(d==null)
      return; 

    string _type1 =(string) Regex.Replace(((string)(string)d.GetType().Name).ToLower(), @"[0-9]", "");

    if (_type1.Contains("list"))
    {
      GetPoints.Clear();
      ((List<string>)d).ForEach(x => GetPoints.Add(x, 0));
      return;
    }

    if (_type1.Contains("dict"))
    {
      SetPoints.Clear();
      foreach (var it in (Dictionary<string, dynamic>)d)
        SetPoints.Add(it.Key, it.Value);
      return;
    }
  }
  private List<(string, dynamic, Dftest)> lsRez = new();

    public void AddGetPoints(string key, dynamic value) 
    { 
        GetPoints.TryAdd(key, value); 
    }
  public void LoadInicialRez(List<string> list) 
  {
    Func<string, string, Dftest, (string, dynamic, Dftest)> f0 = (x, y, f) => 
    {
      string[] s = x.Split("==");
      string name = s[0].Trim();
      dynamic value = (dynamic)s[1].Trim();
      return (name, value, f);
    };

    lsRez.Clear();
    list.ForEach
    (
      x =>
      {
        if (x.Contains("=="))
          lsRez.Add(f0(x, "==", RezultEq));
        else if (x.Contains(">="))
          lsRez.Add(f0(x, ">=", RezultGe));

      }
    ); 

//           var xxx = lsRez.ElementAt(0);
//    bool b = xxx.Item3(3.4, 3.4);

    LRezult = new(list);
  }
  public virtual bool TestDan(Dictionary<string, dynamic> rezul)
  {
    Dictionary<string, dynamic> _rezul = rezul;
    bool _brezult = true;

    if (_rezul.Count == 0)
      return false;

    foreach (var it in lsRez)
    {
      var _name = it.Item1;
      if(_rezul.TryGetValue(_name, out dynamic val1))
      {
        double x0;
        double x01=val1;
        //var b0 = double.TryParse(val1.To, out x0);
        double x1;
        var bb= double.TryParse(((string)(it.Item2)).Replace('.',','), out x1);
        bool _z = it.Item3(x01, x1);

        _brezult = _brezult && _z;
      }
      else
      {
        Console.WriteLine($" Error not {_name} ");
        return false;
      }
      
    }

    return _brezult;
  }
  public virtual bool RezultEq(dynamic x0, dynamic x1) => (double)x0 == (double)x1;  // ==

  public virtual bool RezultNe() // !=
  {
    return false;
  }
  public virtual bool RezultGe(dynamic x0, dynamic x1) => x0 >= x1; // >= 
  public virtual bool RezultGt() // > 
  {
    return false;
  }
  public virtual bool RezultLe() // <= 
  {
    return false;
  }
  public virtual bool RezultLt() // < 
  {
    return false;
  }


}

/*
 https://www.tutorialsteacher.com/python/magic-methods-in-python
 __lt__(self, other)	To get called on comparison using < operator.
__le__(self, other)	To get called on comparison using <= operator.
__eq__(self, other)	To get called on comparison using == operator.
__ne__(self, other)	To get called on comparison using != operator.
__ge__(self, other)	To get called on comparison using >= operator.
 
 */