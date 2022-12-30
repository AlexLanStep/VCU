
using ETAS.EE.Scripting;
using LabCarContext20.Data.Interface;
using System.Threading.Tasks;

namespace LabCarContext20.Data;

public interface ILcLoggers
{
  bool Add(string[] names);
  void SetPath(string path);
  void Start();
  void Stop();
  
}
public class LcLoggers: ILcLoggers
{
  #region data
  private string _nameDir  = "";
  private string _pathStrategy = "";
  private string _fileLogger="";
  #endregion
  private readonly IAllDan _iallDan;
  private IDataLogger? _iDatalogger = null;
  private IConnectLabCar _iConnectLC; 

  public LcLoggers(IConnectLabCar iConnectLc, IAllDan allDan)
  {
    _iConnectLC = iConnectLc;
    _iallDan = allDan;


  }
  public void SetPath(string path)
  {

    #region --- Logger -- 
    _pathStrategy = path;
    string pathDan = _pathStrategy + "\\Dan";
    if (!Directory.Exists(pathDan))
      Directory.CreateDirectory(pathDan);
    string[] ss = _pathStrategy.Split("\\");
    _nameDir = ss[ss.Length - 1];
    _fileLogger = pathDan + "\\" + "Logger" + _nameDir + "_.dat";
    #endregion

    _iDatalogger = _iConnectLC.Experiment.DataLoggers.GetDataloggerByName(_nameDir);
    if (_iDatalogger == null)
      _iDatalogger = _iConnectLC.Experiment.DataLoggers.CreateDatalogger(_nameDir);
    else
    {
      try
      {
        _iConnectLC.Experiment.DataLoggers.RemoveDatalogger(_iDatalogger);
        _iDatalogger = _iConnectLC.Experiment.DataLoggers.CreateDatalogger(_nameDir);
      }
      catch (Exception)
      {
        // ignored
      }
    }

  }

  public void Start()
  {
#if MODEL
    return;
#endif

    try
    {
      _iDatalogger?.Start();
    }
    catch (Exception)
    {
      // ignored
    }
  }
  public void Stop()
  {
#if MODEL
    return;
#endif

    try
    {
      _iDatalogger?.Stop();
    }
    catch (Exception)
    {
      // ignored
    }
  }

  public bool Add(string[] names)
  {
    List<string> lsPath = new ();
    List<string> lsTask = new ();

    foreach (string name in names)
    {
      CReadLc? task = (CReadLc?)_iallDan.GetT<CReadLc>(name);  //  LcDan.GetTaskInfo(item);

      if (task != null)
      {
        lsPath.Add(task.PathTask);
        lsTask.Add(task.TimeLabCar);
      }
    }

    if (lsPath.Count > 0)
      return Add(lsPath.ToArray(), lsTask.ToArray());

    return false;
  }
  
  private bool Add(string[] signal, string[] task)
  {

#if MODEL
    return true;
#endif
    try
    {
      // ReSharper disable once RedundantCast
      var idlcol = (IDataLoggerCollection)_iConnectLC.Experiment.DataLoggers;
      idlcol.ClearConfiguration();
    }
    catch (Exception)
    {
      // ignored
    }


    if (_iDatalogger == null) return false;

    _iDatalogger.AddScalarRecordingSignals(signal, task);
    _iDatalogger = _iConnectLC.Experiment.DataLoggers.GetDataloggerByName(_nameDir);
    _iDatalogger.ConfigureRecordingFile(_fileLogger, "MDF", true, 3);
    _iDatalogger.ApplyConfiguration();
    _iDatalogger.Activate();
    
    if (_iDatalogger == null) return false;
    
    return true;
  }


}
