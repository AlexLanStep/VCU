﻿
using DryIoc;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Reactive.Joins;
using System.Xml.Xsl;

namespace LabCarContext20.Core.Strategies;

public interface IStContext
{
  void ParserStrateg(List<JToken>? stBasa);
  void SetParams(Dictionary<string, dynamic>? paramsStrategy);
  bool Execute();
  bool ForStartOneStep(string nameStOne);
  bool ForStopOneStep(string nameStOne);


}

public class StContext : IStContext
{
  private delegate bool ProcessCommand(dynamic d);
  private Dictionary<string, ProcessCommand>? _process = new();

  #region Conteiners
  protected ContainerManager? _container = null;
  protected readonly IAllDan _allDan;
  protected readonly ILoggerDisplay _ilogDisplay;
  protected ICDopConfig _icDopConfig;
  protected ILcLoggers _iLogger;
  protected DanValue _danValue;
  #endregion
  //private string _pathStrategy;
  //private string _nameDir;
  //private string FileLogger;
  protected Dictionary<string, dynamic>? ParamsStrategy { get; set; } = new();

  private List<JToken>? _stBasa = new();

  protected List<(string, List<(string, object)>)>? lsSt = new();
  public StContext()
  {
    _container = ContainerManager.GetInstance();
    _allDan = _container.LabCar.Resolve<IAllDan>();
    _ilogDisplay = _container.LabCar.Resolve<ILoggerDisplay>();
    _iLogger = _container.LabCar.Resolve<ILcLoggers>();
    _danValue = _container.LabCar.Resolve<DanValue>();

    #region Разбор строк _process
    _process.Add("t", timewaite);
    _process.Add("loggerset", floggerset);

    #endregion
  }
  public void SetParams(Dictionary<string, dynamic>? paramsStrategy)
  { 
    ParamsStrategy = paramsStrategy;
    foreach (var it in ParamsStrategy)
    {
      if (!it.Value.GetType().Name.ToLower().Contains("string"))
      {
        _danValue.Add(it.Key, it.Value);
      }
    }

  }

  public virtual bool Execute()
  {
    foreach (var it in lsSt)
    {
      string _nameStOne = it.Item1;

      var _paramsStOne = it.Item2;

      bool _is = true;
      foreach (var it2 in _paramsStOne)
      {
        if (!ForStartOneStep(_nameStOne))
          return false;

        var _paramsStTwo1 = it2.Item1;

        if (_process.ContainsKey(it2.Item1))
        {
          _is = _is && _process[it2.Item1].Invoke(it2.Item2);
          if (!_is) 
          { 
            //  Сщщищение об ошибке
            return false; 
          }
        }
        else
        {
          // error ----
//          return false;
        }

        if (!ForStopOneStep(_nameStOne))
          return false;

      }

    }

    return true;
  }

  public virtual bool ForStartOneStep(string nameStOne)
  {
    return true;
  }

  public virtual bool ForStopOneStep(string nameStOne)
  {
    return true;
  }

  public void ParserStrateg(List<JToken>? stBasa)
  {
    _stBasa = stBasa;

    foreach (var it in _stBasa)
    {
      var ee = JsonToDicStDyn(it.ToString());
      if (ee == null)
        continue;

      var StOneName = ee.Keys.ToArray()[0];

      List<(string, object)> _stepSt = new();

      Dictionary<string, object> xx = (Dictionary<string, object>)JsonToDicStDyn((ee?[ee.Keys.ElementAt(0)])?.ToString());

      xx.Keys.ToList().ForEach(x => _stepSt.Add((x, xx[x])));

      lsSt.Add((StOneName, _stepSt));
    }
  }

  #region _  t-> waite __
  private bool timewaite(dynamic d)
  {
    if (d.GetType().Name.ToLower().Contains("int"))
    {
      Thread.Sleep((int)d);
      return true;
    }
    try
    {
      var dx = Convert.ToInt32(d.ToString());
      Thread.Sleep((int)dx);
      return true;
    }
    catch (Exception)
    {
      var val = _allDan.Get(d.ToString());
      if(val != null)
      {
        if (val.GetType().Name.ToLower().Contains("int"))
        {
          Thread.Sleep((int)val);
          return true;
        }
      }
      return false;
    }
    return false;
  }
  #endregion

  #region _  loggerset __
  private bool floggerset(dynamic s) 
  {
    var ss = JsonArrayString(s.ToString() ?? string.Empty);
    
    _iLogger.Add(ss); 
    return true;
  }

  #endregion

  protected Dictionary<string, dynamic>? JsonToDicStDyn(string name) =>
    JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(name);
  protected List<string>? JsonLsString(string name) => JsonConvert.DeserializeObject<List<string>>(name);
  protected string[]? JsonArrayString(string name) => JsonConvert.DeserializeObject<List<string>>(name).ToArray();

}

