
using System;
using System.Globalization;
using System.Text;


namespace Common.Logging.Simple
{

  public class ConsoleOutLogger : ILog
  {
    private bool _showDateTime = false;
    private bool _showLogName = false;
    private string _logName = string.Empty;
    private Common.Logging.LogLevel _currentLogLevel = Common.Logging.LogLevel.All;
    private string _dateTimeFormat = string.Empty;
    private bool _hasDateTimeFormat = false;

    public ConsoleOutLogger(
      string logName,
      Common.Logging.LogLevel logLevel,
      bool showDateTime,
      bool showLogName,
      string dateTimeFormat)
    {
      this._logName = logName;
      this._currentLogLevel = logLevel;
      this._showDateTime = showDateTime;
      this._showLogName = showLogName;
      this._dateTimeFormat = dateTimeFormat;
      if (this._dateTimeFormat == null || this._dateTimeFormat.Length <= 0)
        return;
      this._hasDateTimeFormat = true;
    }

    private void Write(Common.Logging.LogLevel level, object message, Exception e)
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (this._showDateTime)
      {
        if (this._hasDateTimeFormat)
          stringBuilder.Append(DateTime.Now.ToString(this._dateTimeFormat, (IFormatProvider)CultureInfo.InvariantCulture));
        else
          stringBuilder.Append((object)DateTime.Now);
        stringBuilder.Append(" ");
      }
      stringBuilder.Append(string.Format("[{0}]", (object)level.ToString().ToUpper()).PadRight(8));
      if (this._showLogName)
        stringBuilder.Append(this._logName).Append(" - ");
      stringBuilder.Append(message.ToString());
      if (e != null)
        stringBuilder.Append(Environment.NewLine).Append(e.ToString());
      Console.Out.WriteLine(stringBuilder.ToString());
    }

    private bool IsLevelEnabled(Common.Logging.LogLevel level) => level >= this._currentLogLevel;

    public void Debug(object message) => this.Debug(message, (Exception)null);

    public void Debug(object message, Exception e)
    {
      if (!this.IsLevelEnabled(Common.Logging.LogLevel.Debug))
        return;
      this.Write(Common.Logging.LogLevel.Debug, message, e);
    }

    public void Error(object message) => this.Error(message, (Exception)null);

    public void Error(object message, Exception e)
    {
      if (!this.IsLevelEnabled(Common.Logging.LogLevel.Error))
        return;
      this.Write(Common.Logging.LogLevel.Error, message, e);
    }

    public void Fatal(object message) => this.Fatal(message, (Exception)null);

    public void Fatal(object message, Exception e)
    {
      if (!this.IsLevelEnabled(Common.Logging.LogLevel.Fatal))
        return;
      this.Write(Common.Logging.LogLevel.Fatal, message, e);
    }

    public void Info(object message) => this.Info(message, (Exception)null);

    public void Info(object message, Exception e)
    {
      if (!this.IsLevelEnabled(Common.Logging.LogLevel.Info))
        return;
      this.Write(Common.Logging.LogLevel.Info, message, e);
    }

    public void Warn(object message) => this.Warn(message, (Exception)null);

    public void Warn(object message, Exception e)
    {
      if (!this.IsLevelEnabled(Common.Logging.LogLevel.Warn))
        return;
      this.Write(Common.Logging.LogLevel.Warn, message, e);
    }

    public bool IsDebugEnabled => this.IsLevelEnabled(Common.Logging.LogLevel.Debug);

    public bool IsErrorEnabled => this.IsLevelEnabled(Common.Logging.LogLevel.Error);

    public bool IsFatalEnabled => this.IsLevelEnabled(Common.Logging.LogLevel.Fatal);

    public bool IsInfoEnabled => this.IsLevelEnabled(Common.Logging.LogLevel.Info);

    public bool IsWarnEnabled => this.IsLevelEnabled(Common.Logging.LogLevel.Warn);
  }
}