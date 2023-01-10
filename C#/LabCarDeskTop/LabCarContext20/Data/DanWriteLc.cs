
using LabCarContext20.Data.Interface;
namespace LabCarContext20.Data;


public interface IDanWriteLc: IGetDan
{
  bool Add(string nameParama, CWriteLcJson param);
  bool Add(string nameParama, CWriteLc? param);
  bool Set(string name, dynamic d);
  CWriteLc? GetT(string name);
}

public class DanWriteLc : DanBase<CWriteLc>, IDanWriteLc
{
  private ContainerManager? _container = null;

  public DanWriteLc(IConnectLabCar iconnectlc) : base(iconnectlc)
  {
    _container = ContainerManager.GetInstance();
  }

  public bool Add(string nameParama, CWriteLcJson param)
  {
    CWriteLc _write = _container.LabCar.Resolve<CWriteLc>().Inicialization(nameParama, param);
    if (_write.SignalParams == null) return false;
    base.Add(nameParama, _write);
    return true;
  }

  public bool Add(string nameParama, CWriteLc? param)
  { 
    base.Add(nameParama, param);
    return true;
  }

  public bool Set(string name, dynamic d)
  { 
    return (d != null) && cDan.ContainsKey(name) ? cDan[name].SetValue(d) : false; 
  }

//  public CWriteLc? GetT(string name) => (CWriteLc)base.GetT(name);
  public object GetT(string name) => (CWriteLc)base.GetT(name);
//  object IGetDan.GetT(string name){return GetT(name);}
}


/*
#region ___ Params ____
public static bool AddParams(string nameParama, ParameterJson param)
{
  var parameter = new ParameterNew(Iconnect, nameParama, param);
  if (parameter.SignalParams == null) return false;
  _dParameters.AddOrUpdate(nameParama, parameter, (_, _) => parameter);
  return true;
}

public static bool SetParam(string nameParama, dynamic param)
{
  if (_dParameters.ContainsKey(nameParama))
    return _dParameters[nameParama].SetValue(param);
  else
    throw new MyException(" Error проблема с установкой переменных ", -4);
}

public static List<string> AddParamsRange(Dictionary<string, ParameterJson> paramsLabCar)
  => (from it in paramsLabCar where !AddParams(it.Key, it.Value) select it.Key).ToList();

public static ParameterNew? GetParamInfo(string nameParama) =>
  _dParameters.ContainsKey(nameParama) ? _dParameters[nameParama] : null;

#endregion
*/