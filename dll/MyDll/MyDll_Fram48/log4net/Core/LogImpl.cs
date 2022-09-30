// Decompiled with JetBrains decompiler
// Type: log4net.Core.LogImpl
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using log4net.Repository;
using log4net.Util;
using System;
using System.Globalization;

namespace log4net.Core
{
  public class LogImpl : LoggerWrapperImpl, ILog, ILoggerWrapper
  {
    private static readonly Type ThisDeclaringType = typeof (LogImpl);
    private Level m_levelDebug;
    private Level m_levelInfo;
    private Level m_levelWarn;
    private Level m_levelError;
    private Level m_levelFatal;

    public LogImpl(ILogger logger)
      : base(logger)
    {
      logger.Repository.ConfigurationChanged += new LoggerRepositoryConfigurationChangedEventHandler(this.LoggerRepositoryConfigurationChanged);
      this.ReloadLevels(logger.Repository);
    }

    protected virtual void ReloadLevels(ILoggerRepository repository)
    {
      LevelMap levelMap = repository.LevelMap;
      this.m_levelDebug = levelMap.LookupWithDefault(Level.Debug);
      this.m_levelInfo = levelMap.LookupWithDefault(Level.Info);
      this.m_levelWarn = levelMap.LookupWithDefault(Level.Warn);
      this.m_levelError = levelMap.LookupWithDefault(Level.Error);
      this.m_levelFatal = levelMap.LookupWithDefault(Level.Fatal);
    }

    public virtual void Debug(object message) => this.Logger.Log(LogImpl.ThisDeclaringType, this.m_levelDebug, message, (Exception) null);

    public virtual void Debug(object message, Exception exception) => this.Logger.Log(LogImpl.ThisDeclaringType, this.m_levelDebug, message, exception);

    public virtual void DebugFormat(string format, params object[] args)
    {
      if (!this.IsDebugEnabled)
        return;
      this.Logger.Log(LogImpl.ThisDeclaringType, this.m_levelDebug, (object) new SystemStringFormat((IFormatProvider) CultureInfo.InvariantCulture, format, args), (Exception) null);
    }

    public virtual void DebugFormat(string format, object arg0)
    {
      if (!this.IsDebugEnabled)
        return;
      this.Logger.Log(LogImpl.ThisDeclaringType, this.m_levelDebug, (object) new SystemStringFormat((IFormatProvider) CultureInfo.InvariantCulture, format, new object[1]
      {
        arg0
      }), (Exception) null);
    }

    public virtual void DebugFormat(string format, object arg0, object arg1)
    {
      if (!this.IsDebugEnabled)
        return;
      this.Logger.Log(LogImpl.ThisDeclaringType, this.m_levelDebug, (object) new SystemStringFormat((IFormatProvider) CultureInfo.InvariantCulture, format, new object[2]
      {
        arg0,
        arg1
      }), (Exception) null);
    }

    public virtual void DebugFormat(string format, object arg0, object arg1, object arg2)
    {
      if (!this.IsDebugEnabled)
        return;
      this.Logger.Log(LogImpl.ThisDeclaringType, this.m_levelDebug, (object) new SystemStringFormat((IFormatProvider) CultureInfo.InvariantCulture, format, new object[3]
      {
        arg0,
        arg1,
        arg2
      }), (Exception) null);
    }

    public virtual void DebugFormat(IFormatProvider provider, string format, params object[] args)
    {
      if (!this.IsDebugEnabled)
        return;
      this.Logger.Log(LogImpl.ThisDeclaringType, this.m_levelDebug, (object) new SystemStringFormat(provider, format, args), (Exception) null);
    }

    public virtual void Info(object message) => this.Logger.Log(LogImpl.ThisDeclaringType, this.m_levelInfo, message, (Exception) null);

    public virtual void Info(object message, Exception exception) => this.Logger.Log(LogImpl.ThisDeclaringType, this.m_levelInfo, message, exception);

    public virtual void InfoFormat(string format, params object[] args)
    {
      if (!this.IsInfoEnabled)
        return;
      this.Logger.Log(LogImpl.ThisDeclaringType, this.m_levelInfo, (object) new SystemStringFormat((IFormatProvider) CultureInfo.InvariantCulture, format, args), (Exception) null);
    }

    public virtual void InfoFormat(string format, object arg0)
    {
      if (!this.IsInfoEnabled)
        return;
      this.Logger.Log(LogImpl.ThisDeclaringType, this.m_levelInfo, (object) new SystemStringFormat((IFormatProvider) CultureInfo.InvariantCulture, format, new object[1]
      {
        arg0
      }), (Exception) null);
    }

    public virtual void InfoFormat(string format, object arg0, object arg1)
    {
      if (!this.IsInfoEnabled)
        return;
      this.Logger.Log(LogImpl.ThisDeclaringType, this.m_levelInfo, (object) new SystemStringFormat((IFormatProvider) CultureInfo.InvariantCulture, format, new object[2]
      {
        arg0,
        arg1
      }), (Exception) null);
    }

    public virtual void InfoFormat(string format, object arg0, object arg1, object arg2)
    {
      if (!this.IsInfoEnabled)
        return;
      this.Logger.Log(LogImpl.ThisDeclaringType, this.m_levelInfo, (object) new SystemStringFormat((IFormatProvider) CultureInfo.InvariantCulture, format, new object[3]
      {
        arg0,
        arg1,
        arg2
      }), (Exception) null);
    }

    public virtual void InfoFormat(IFormatProvider provider, string format, params object[] args)
    {
      if (!this.IsInfoEnabled)
        return;
      this.Logger.Log(LogImpl.ThisDeclaringType, this.m_levelInfo, (object) new SystemStringFormat(provider, format, args), (Exception) null);
    }

    public virtual void Warn(object message) => this.Logger.Log(LogImpl.ThisDeclaringType, this.m_levelWarn, message, (Exception) null);

    public virtual void Warn(object message, Exception exception) => this.Logger.Log(LogImpl.ThisDeclaringType, this.m_levelWarn, message, exception);

    public virtual void WarnFormat(string format, params object[] args)
    {
      if (!this.IsWarnEnabled)
        return;
      this.Logger.Log(LogImpl.ThisDeclaringType, this.m_levelWarn, (object) new SystemStringFormat((IFormatProvider) CultureInfo.InvariantCulture, format, args), (Exception) null);
    }

    public virtual void WarnFormat(string format, object arg0)
    {
      if (!this.IsWarnEnabled)
        return;
      this.Logger.Log(LogImpl.ThisDeclaringType, this.m_levelWarn, (object) new SystemStringFormat((IFormatProvider) CultureInfo.InvariantCulture, format, new object[1]
      {
        arg0
      }), (Exception) null);
    }

    public virtual void WarnFormat(string format, object arg0, object arg1)
    {
      if (!this.IsWarnEnabled)
        return;
      this.Logger.Log(LogImpl.ThisDeclaringType, this.m_levelWarn, (object) new SystemStringFormat((IFormatProvider) CultureInfo.InvariantCulture, format, new object[2]
      {
        arg0,
        arg1
      }), (Exception) null);
    }

    public virtual void WarnFormat(string format, object arg0, object arg1, object arg2)
    {
      if (!this.IsWarnEnabled)
        return;
      this.Logger.Log(LogImpl.ThisDeclaringType, this.m_levelWarn, (object) new SystemStringFormat((IFormatProvider) CultureInfo.InvariantCulture, format, new object[3]
      {
        arg0,
        arg1,
        arg2
      }), (Exception) null);
    }

    public virtual void WarnFormat(IFormatProvider provider, string format, params object[] args)
    {
      if (!this.IsWarnEnabled)
        return;
      this.Logger.Log(LogImpl.ThisDeclaringType, this.m_levelWarn, (object) new SystemStringFormat(provider, format, args), (Exception) null);
    }

    public virtual void Error(object message) => this.Logger.Log(LogImpl.ThisDeclaringType, this.m_levelError, message, (Exception) null);

    public virtual void Error(object message, Exception exception) => this.Logger.Log(LogImpl.ThisDeclaringType, this.m_levelError, message, exception);

    public virtual void ErrorFormat(string format, params object[] args)
    {
      if (!this.IsErrorEnabled)
        return;
      this.Logger.Log(LogImpl.ThisDeclaringType, this.m_levelError, (object) new SystemStringFormat((IFormatProvider) CultureInfo.InvariantCulture, format, args), (Exception) null);
    }

    public virtual void ErrorFormat(string format, object arg0)
    {
      if (!this.IsErrorEnabled)
        return;
      this.Logger.Log(LogImpl.ThisDeclaringType, this.m_levelError, (object) new SystemStringFormat((IFormatProvider) CultureInfo.InvariantCulture, format, new object[1]
      {
        arg0
      }), (Exception) null);
    }

    public virtual void ErrorFormat(string format, object arg0, object arg1)
    {
      if (!this.IsErrorEnabled)
        return;
      this.Logger.Log(LogImpl.ThisDeclaringType, this.m_levelError, (object) new SystemStringFormat((IFormatProvider) CultureInfo.InvariantCulture, format, new object[2]
      {
        arg0,
        arg1
      }), (Exception) null);
    }

    public virtual void ErrorFormat(string format, object arg0, object arg1, object arg2)
    {
      if (!this.IsErrorEnabled)
        return;
      this.Logger.Log(LogImpl.ThisDeclaringType, this.m_levelError, (object) new SystemStringFormat((IFormatProvider) CultureInfo.InvariantCulture, format, new object[3]
      {
        arg0,
        arg1,
        arg2
      }), (Exception) null);
    }

    public virtual void ErrorFormat(IFormatProvider provider, string format, params object[] args)
    {
      if (!this.IsErrorEnabled)
        return;
      this.Logger.Log(LogImpl.ThisDeclaringType, this.m_levelError, (object) new SystemStringFormat(provider, format, args), (Exception) null);
    }

    public virtual void Fatal(object message) => this.Logger.Log(LogImpl.ThisDeclaringType, this.m_levelFatal, message, (Exception) null);

    public virtual void Fatal(object message, Exception exception) => this.Logger.Log(LogImpl.ThisDeclaringType, this.m_levelFatal, message, exception);

    public virtual void FatalFormat(string format, params object[] args)
    {
      if (!this.IsFatalEnabled)
        return;
      this.Logger.Log(LogImpl.ThisDeclaringType, this.m_levelFatal, (object) new SystemStringFormat((IFormatProvider) CultureInfo.InvariantCulture, format, args), (Exception) null);
    }

    public virtual void FatalFormat(string format, object arg0)
    {
      if (!this.IsFatalEnabled)
        return;
      this.Logger.Log(LogImpl.ThisDeclaringType, this.m_levelFatal, (object) new SystemStringFormat((IFormatProvider) CultureInfo.InvariantCulture, format, new object[1]
      {
        arg0
      }), (Exception) null);
    }

    public virtual void FatalFormat(string format, object arg0, object arg1)
    {
      if (!this.IsFatalEnabled)
        return;
      this.Logger.Log(LogImpl.ThisDeclaringType, this.m_levelFatal, (object) new SystemStringFormat((IFormatProvider) CultureInfo.InvariantCulture, format, new object[2]
      {
        arg0,
        arg1
      }), (Exception) null);
    }

    public virtual void FatalFormat(string format, object arg0, object arg1, object arg2)
    {
      if (!this.IsFatalEnabled)
        return;
      this.Logger.Log(LogImpl.ThisDeclaringType, this.m_levelFatal, (object) new SystemStringFormat((IFormatProvider) CultureInfo.InvariantCulture, format, new object[3]
      {
        arg0,
        arg1,
        arg2
      }), (Exception) null);
    }

    public virtual void FatalFormat(IFormatProvider provider, string format, params object[] args)
    {
      if (!this.IsFatalEnabled)
        return;
      this.Logger.Log(LogImpl.ThisDeclaringType, this.m_levelFatal, (object) new SystemStringFormat(provider, format, args), (Exception) null);
    }

    public virtual bool IsDebugEnabled => this.Logger.IsEnabledFor(this.m_levelDebug);

    public virtual bool IsInfoEnabled => this.Logger.IsEnabledFor(this.m_levelInfo);

    public virtual bool IsWarnEnabled => this.Logger.IsEnabledFor(this.m_levelWarn);

    public virtual bool IsErrorEnabled => this.Logger.IsEnabledFor(this.m_levelError);

    public virtual bool IsFatalEnabled => this.Logger.IsEnabledFor(this.m_levelFatal);

    private void LoggerRepositoryConfigurationChanged(object sender, EventArgs e)
    {
      if (!(sender is ILoggerRepository repository))
        return;
      this.ReloadLevels(repository);
    }
  }
}
