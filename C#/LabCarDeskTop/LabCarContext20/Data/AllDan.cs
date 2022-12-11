using LabCarContext20.Core.Config;
using LabCarContext20.Data.Interface;

namespace LabCarContext20.Data;
public delegate dynamic? GetInfo(string name);
public delegate object GetTInfo(string name);
//public delegate CReadLc? GetTInfo(string name);

//public dynamic? Get(string name)
//public CReadLc? GetT(string name)


public class AllDan : IAllDan
{
  private ConcurrentDictionary<string, GetInfo> _dGet = new();
  private ConcurrentDictionary<string, GetTInfo> _dGetT = new();
  private DanDanReadLc _danReadLc; 
  private DanValue danValue;
  public AllDan(DanDanReadLc danReadLc, DanValue danValue)
  {
    this._danReadLc = danReadLc;
    this.danValue = danValue;
  }

  public void Add<T>(string name, T dan)
  {
    string nameType = typeof(T).Name.ToLower();
    switch (nameType)
    {
      case "creadlc":
      {
        _danReadLc.Add(name, dan as CReadLc); //CReadLc
        _dGet.AddOrUpdate(name, _danReadLc.Get, (s, o) => _danReadLc.Get);
        _dGetT.AddOrUpdate(name, _danReadLc.GetT, (s, o) => _danReadLc.GetT);
        break;
      }
      case "object":
      {
        danValue.Add(name, dan);
        _dGet.AddOrUpdate(name, danValue.Get, (s, o) => danValue.Get);
        var _zz = _dGet[name].Invoke(name);
        _dGetT.AddOrUpdate(name, danValue.GetT, (s, o) => danValue.GetT);
        break;
      }

    }
  }

  public object GetT<T>(string name)
  {
    string nameType = typeof(T).Name.ToLower();
    switch (nameType)
    {
      case "creadlc":
      {
        return (CReadLc)_dGetT[name].Invoke(name); 
        //break;
      }
      case "object":
      {
        var xx = (DanValue) _dGetT[name].Invoke(name);
        return xx;
        break;
      }


    }

    return null;
  }
  public dynamic? Get(string name)
  {
    var xx = _dGet[name].Invoke(name);
    return xx;


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
  }

}