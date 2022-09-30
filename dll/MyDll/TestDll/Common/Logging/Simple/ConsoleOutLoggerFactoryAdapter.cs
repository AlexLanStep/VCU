// Decompiled with JetBrains decompiler
// Type: Common.Logging.Simple.ConsoleOutLoggerFactoryAdapter
// Assembly: Common.Logging, Version=1.1.0.1, Culture=neutral, PublicKeyToken=65e474d141e25e07
// MVID: F190E754-F88D-465E-80CE-5B5FC84116FB
// Assembly location: E:\LabCar\BasaDll\Common.Logging.dll


namespace Common.Logging.Simple;

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
