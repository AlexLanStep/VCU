
namespace LabCarContext20.Data;

public interface IDanLoggerLc
{

}
public class DanLoggerLc : DanBase<LoggerLc>, IDanLoggerLc
{
  public DanLoggerLc(IConnectLabCar iconnectlc):base(iconnectlc){}

  public bool Add(string nameDir, string nameDirDan, string[] signal, string[] task)
  {
    var logger = new LoggerLc() { Name = nameDir, PathDirDan = nameDirDan };
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

    logger.Datalogger = _iConnectLC.Experiment.DataLoggers.GetDataloggerByName(logger.Name);
    if (logger.Datalogger == null)
      logger.Datalogger = _iConnectLC.Experiment.DataLoggers.CreateDatalogger(logger.Name);
    else
    {
      try
      {
        _iConnectLC.Experiment.DataLoggers.RemoveDatalogger(logger.Datalogger);
        logger.Datalogger = _iConnectLC.Experiment.DataLoggers.CreateDatalogger(logger.Name);
      }
      catch (Exception)
      {
        // ignored
      }
    }

    if (logger.Datalogger == null) return false;

    logger.Datalogger.AddScalarRecordingSignals(signal, task);
    logger.Datalogger = _iConnectLC.Experiment.DataLoggers.GetDataloggerByName(logger.Name);
    logger.Datalogger.ConfigureRecordingFile(logger.PathDirDan, "MDF", true, 3);
    logger.Datalogger.ApplyConfiguration();
    logger.Datalogger.Activate();
    if (logger.Datalogger == null) return false;
    base.Add(nameDir, logger);
    return true;
  }

  public new LoggerLc GetT(string name) => base.GetT(name);

}


