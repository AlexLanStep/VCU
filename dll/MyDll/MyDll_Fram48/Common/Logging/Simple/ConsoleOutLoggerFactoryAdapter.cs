
using System;
using System.Collections;
using System.Collections.Specialized;

namespace Common.Logging.Simple
{

  public class ConsoleOutLoggerFactoryAdapter : ILoggerFactoryAdapter
  {
    private Hashtable _logs = Hashtable.Synchronized(new Hashtable());
    private Common.Logging.LogLevel _Level = Common.Logging.LogLevel.All;
    private bool _showDateTime = true;
    private bool _showLogName = true;
    private string _dateTimeFormat = string.Empty;

    public ConsoleOutLoggerFactoryAdapter(NameValueCollection properties)
    {
      try
      {
        this._Level = (Common.Logging.LogLevel)Enum.Parse(typeof(Common.Logging.LogLevel), properties["level"], true);
      }
      catch (Exception ex)
      {
        this._Level = Common.Logging.LogLevel.All;
      }
      try
      {
        this._showDateTime = bool.Parse(properties["showDateTime"]);
      }
      catch (Exception ex)
      {
        this._showDateTime = true;
      }
      try
      {
        this._showLogName = bool.Parse(properties["showLogName"]);
      }
      catch (Exception ex)
      {
        this._showLogName = true;
      }
      this._dateTimeFormat = properties["dateTimeFormat"];
    }

    public ILog GetLogger(Type type) => this.GetLogger(type.FullName);

    public ILog GetLogger(string name)
    {
      if (!(this._logs[(object)name] is ILog logger))
      {
        logger = (ILog)new ConsoleOutLogger(name, this._Level, this._showDateTime, this._showLogName, this._dateTimeFormat);
        this._logs.Add((object)name, (object)logger);
      }
      return logger;
    }
  }
}