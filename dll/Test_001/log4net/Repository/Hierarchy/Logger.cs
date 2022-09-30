// Decompiled with JetBrains decompiler
// Type: log4net.Repository.Hierarchy.Logger
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using log4net.Appender;
using log4net.Core;
using log4net.Util;
using System;
using System.Security;

namespace log4net.Repository.Hierarchy
{
  public abstract class Logger : IAppenderAttachable, ILogger
  {
    private static readonly Type ThisDeclaringType = typeof (Logger);
    private readonly string m_name;
    private Level m_level;
    private Logger m_parent;
    private log4net.Repository.Hierarchy.Hierarchy m_hierarchy;
    private AppenderAttachedImpl m_appenderAttachedImpl;
    private bool m_additive = true;
    private readonly ReaderWriterLock m_appenderLock = new ReaderWriterLock();

    protected Logger(string name) => this.m_name = string.Intern(name);

    public virtual Logger Parent
    {
      get => this.m_parent;
      set => this.m_parent = value;
    }

    public virtual bool Additivity
    {
      get => this.m_additive;
      set => this.m_additive = value;
    }

    public virtual Level EffectiveLevel
    {
      get
      {
        for (Logger logger = this; logger != null; logger = logger.m_parent)
        {
          Level level = logger.m_level;
          if ((object) level != null)
            return level;
        }
        return (Level) null;
      }
    }

    public virtual log4net.Repository.Hierarchy.Hierarchy Hierarchy
    {
      get => this.m_hierarchy;
      set => this.m_hierarchy = value;
    }

    public virtual Level Level
    {
      get => this.m_level;
      set => this.m_level = value;
    }

    public virtual void AddAppender(IAppender newAppender)
    {
      if (newAppender == null)
        throw new ArgumentNullException(nameof (newAppender));
      this.m_appenderLock.AcquireWriterLock();
      try
      {
        if (this.m_appenderAttachedImpl == null)
          this.m_appenderAttachedImpl = new AppenderAttachedImpl();
        this.m_appenderAttachedImpl.AddAppender(newAppender);
      }
      finally
      {
        this.m_appenderLock.ReleaseWriterLock();
      }
    }

    public virtual AppenderCollection Appenders
    {
      get
      {
        this.m_appenderLock.AcquireReaderLock();
        try
        {
          return this.m_appenderAttachedImpl == null ? AppenderCollection.EmptyCollection : this.m_appenderAttachedImpl.Appenders;
        }
        finally
        {
          this.m_appenderLock.ReleaseReaderLock();
        }
      }
    }

    public virtual IAppender GetAppender(string name)
    {
      this.m_appenderLock.AcquireReaderLock();
      try
      {
        return this.m_appenderAttachedImpl == null || name == null ? (IAppender) null : this.m_appenderAttachedImpl.GetAppender(name);
      }
      finally
      {
        this.m_appenderLock.ReleaseReaderLock();
      }
    }

    public virtual void RemoveAllAppenders()
    {
      this.m_appenderLock.AcquireWriterLock();
      try
      {
        if (this.m_appenderAttachedImpl == null)
          return;
        this.m_appenderAttachedImpl.RemoveAllAppenders();
        this.m_appenderAttachedImpl = (AppenderAttachedImpl) null;
      }
      finally
      {
        this.m_appenderLock.ReleaseWriterLock();
      }
    }

    public virtual IAppender RemoveAppender(IAppender appender)
    {
      this.m_appenderLock.AcquireWriterLock();
      try
      {
        if (appender != null)
        {
          if (this.m_appenderAttachedImpl != null)
            return this.m_appenderAttachedImpl.RemoveAppender(appender);
        }
      }
      finally
      {
        this.m_appenderLock.ReleaseWriterLock();
      }
      return (IAppender) null;
    }

    public virtual IAppender RemoveAppender(string name)
    {
      this.m_appenderLock.AcquireWriterLock();
      try
      {
        if (name != null)
        {
          if (this.m_appenderAttachedImpl != null)
            return this.m_appenderAttachedImpl.RemoveAppender(name);
        }
      }
      finally
      {
        this.m_appenderLock.ReleaseWriterLock();
      }
      return (IAppender) null;
    }

    public virtual string Name => this.m_name;

    public virtual void Log(
      Type callerStackBoundaryDeclaringType,
      Level level,
      object message,
      Exception exception)
    {
      try
      {
        if (!this.IsEnabledFor(level))
          return;
        this.ForcedLog(callerStackBoundaryDeclaringType != null ? callerStackBoundaryDeclaringType : Logger.ThisDeclaringType, level, message, exception);
      }
      catch (Exception ex)
      {
        LogLog.Error("Log: Exception while logging", ex);
      }
      catch
      {
        LogLog.Error("Log: Exception while logging");
      }
    }

    public virtual void Log(LoggingEvent logEvent)
    {
      try
      {
        if (logEvent == null || !this.IsEnabledFor(logEvent.Level))
          return;
        this.ForcedLog(logEvent);
      }
      catch (Exception ex)
      {
        LogLog.Error("Log: Exception while logging", ex);
      }
      catch
      {
        LogLog.Error("Log: Exception while logging");
      }
    }

    public virtual bool IsEnabledFor(Level level)
    {
      try
      {
        if (level != (Level) null)
          return !this.m_hierarchy.IsDisabled(level) && level >= this.EffectiveLevel;
      }
      catch (Exception ex)
      {
        LogLog.Error("Log: Exception while logging", ex);
      }
      catch
      {
        LogLog.Error("Log: Exception while logging");
      }
      return false;
    }

    public ILoggerRepository Repository => (ILoggerRepository) this.m_hierarchy;

    protected virtual void CallAppenders(LoggingEvent loggingEvent)
    {
      if (loggingEvent == null)
        throw new ArgumentNullException(nameof (loggingEvent));
      int num = 0;
      for (Logger logger = this; logger != null; logger = logger.m_parent)
      {
        if (logger.m_appenderAttachedImpl != null)
        {
          logger.m_appenderLock.AcquireReaderLock();
          try
          {
            if (logger.m_appenderAttachedImpl != null)
              num += logger.m_appenderAttachedImpl.AppendLoopOnAppenders(loggingEvent);
          }
          finally
          {
            logger.m_appenderLock.ReleaseReaderLock();
          }
        }
        if (!logger.m_additive)
          break;
      }
      if (this.m_hierarchy.EmittedNoAppenderWarning || num != 0)
        return;
      LogLog.Debug("Logger: No appenders could be found for logger [" + this.Name + "] repository [" + this.Repository.Name + "]");
      LogLog.Debug("Logger: Please initialize the log4net system properly.");
      try
      {
        LogLog.Debug("Logger:    Current AppDomain context information: ");
        LogLog.Debug("Logger:       BaseDirectory   : " + SystemInfo.ApplicationBaseDirectory);
        LogLog.Debug("Logger:       FriendlyName    : " + AppDomain.CurrentDomain.FriendlyName);
        LogLog.Debug("Logger:       DynamicDirectory: " + AppDomain.CurrentDomain.DynamicDirectory);
      }
      catch (SecurityException ex)
      {
      }
      this.m_hierarchy.EmittedNoAppenderWarning = true;
    }

    public virtual void CloseNestedAppenders()
    {
      this.m_appenderLock.AcquireWriterLock();
      try
      {
        if (this.m_appenderAttachedImpl == null)
          return;
        foreach (IAppender appender in this.m_appenderAttachedImpl.Appenders)
        {
          if (appender is IAppenderAttachable)
            appender.Close();
        }
      }
      finally
      {
        this.m_appenderLock.ReleaseWriterLock();
      }
    }

    public virtual void Log(Level level, object message, Exception exception)
    {
      if (!this.IsEnabledFor(level))
        return;
      this.ForcedLog(Logger.ThisDeclaringType, level, message, exception);
    }

    protected virtual void ForcedLog(
      Type callerStackBoundaryDeclaringType,
      Level level,
      object message,
      Exception exception)
    {
      this.CallAppenders(new LoggingEvent(callerStackBoundaryDeclaringType, (ILoggerRepository) this.Hierarchy, this.Name, level, message, exception));
    }

    protected virtual void ForcedLog(LoggingEvent logEvent)
    {
      logEvent.EnsureRepository((ILoggerRepository) this.Hierarchy);
      this.CallAppenders(logEvent);
    }
  }
}
