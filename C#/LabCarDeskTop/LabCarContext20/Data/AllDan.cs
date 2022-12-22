
namespace LabCarContext20.Data;
public delegate dynamic? GetInfo(string name);
public delegate dynamic? GetTInfo(string name);
public delegate bool SetTInfo(string name, dynamic data);

public class AllDan : IAllDan
{
  private readonly ConcurrentDictionary<string, GetInfo> _dGet = new();
  private readonly ConcurrentDictionary<string, GetTInfo> _dGetT = new();
  private readonly ConcurrentDictionary<string, SetTInfo> _dSet = new();
  private readonly DanReadLc _danReadLc; 
  private readonly DanValue _danValue;
  private readonly DanWriteLc _danWriteLc;
  public AllDan(DanReadLc danReadLc, DanValue danValue, DanWriteLc danWriteLc)
  {
    this._danReadLc = danReadLc;
    this._danValue = danValue;
    this._danWriteLc = danWriteLc;
  }

  public void AddCalc(string name, dynamic d)
  {
    if (_dGet.ContainsKey(name))
    {
      
    }
    else
      Add<dynamic>(name, d);
  }
  public void Add<T>(string name, T dan)
  {
    string nameType = typeof(T).Name.ToLower();
    switch (nameType)
    {
      case "creadlc":
      {
        if (!_danReadLc.Add(name, dan as CReadLc)) //CReadLc
          throw new MyException($" Проблема в Task {name} {dan.ToString()}",-33);

        _dGet.AddOrUpdate(name, _danReadLc.Get, (_, _) => _danReadLc.Get);
        _dGetT.AddOrUpdate(name, _danReadLc.GetT, (_, _) => _danReadLc.GetT);
        break;
      }
      case "object":
      {
        if (dan == null) return;
        _danValue.Add(name, dan);
        _dGet.AddOrUpdate(name, _danValue.Get, (_, _) => _danValue.Get);
        _dGetT.AddOrUpdate(name, _danValue.GetT, (_, _) => _danValue.GetT);
        break;
      }
      case "cwritelc":
      {
        _danWriteLc.Add(name, dan as CWriteLc);
        _dSet.AddOrUpdate(name, _danWriteLc.Set, (_, _) => _danWriteLc.Set);
        break;
      }
    }
  }

#pragma warning disable CS8600
  public object? GetT<T>(string name)
  {
    return typeof(T).Name.ToLower() switch
    {
      "creadlc" => (CReadLc) _dGetT[name].Invoke(name),
      "object" => (DanValue) _dGetT[name].Invoke(name),
      _ => null
    };
  }
#pragma warning restore CS8600

  public dynamic? Get(string name)
  {
    try
    {
      dynamic _d = _dGet[name].Invoke(name);
      return _d;
    }
    catch (Exception e)
    {
      return null;
    }
  }

}



//string nameType = typeof(T).Name.ToLower();
//switch (nameType)
//{
//  case "creadlc":
//    {
//      //danReadLc.Add(name, dan as CReadLc); //CReadLc
//      //                                  //        var _x = (GetInfo) danReadLc.Get(name);
//      //_dGet.AddOrUpdate(name, danReadLc.Get, (s, o) => danReadLc.Get);
//      //var _zz = _dGet[name].Invoke(name);
//      return _dGet[name].Invoke(name);
//      //break;
//    }
//}

//return null;
