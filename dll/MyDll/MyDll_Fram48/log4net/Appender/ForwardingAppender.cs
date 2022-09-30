// Decompiled with JetBrains decompiler
// Type: log4net.Appender.ForwardingAppender
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using log4net.Core;
using log4net.Util;
using System;

namespace log4net.Appender
{
  public class ForwardingAppender : AppenderSkeleton, IAppenderAttachable
  {
    private AppenderAttachedImpl m_appenderAttachedImpl;

    protected override void OnClose()
    {
      lock (this)
      {
        if (this.m_appenderAttachedImpl == null)
          return;
        this.m_appenderAttachedImpl.RemoveAllAppenders();
      }
    }

    protected override void Append(LoggingEvent loggingEvent)
    {
      if (this.m_appenderAttachedImpl == null)
        return;
      this.m_appenderAttachedImpl.AppendLoopOnAppenders(loggingEvent);
    }

    protected override void Append(LoggingEvent[] loggingEvents)
    {
      if (this.m_appenderAttachedImpl == null)
        return;
      this.m_appenderAttachedImpl.AppendLoopOnAppenders(loggingEvents);
    }

    public virtual void AddAppender(IAppender newAppender)
    {
      if (newAppender == null)
        throw new ArgumentNullException(nameof (newAppender));
      lock (this)
      {
        if (this.m_appenderAttachedImpl == null)
          this.m_appenderAttachedImpl = new AppenderAttachedImpl();
        this.m_appenderAttachedImpl.AddAppender(newAppender);
      }
    }

    public virtual AppenderCollection Appenders
    {
      get
      {
        lock (this)
          return this.m_appenderAttachedImpl == null ? AppenderCollection.EmptyCollection : this.m_appenderAttachedImpl.Appenders;
      }
    }

    public virtual IAppender GetAppender(string name)
    {
      lock (this)
        return this.m_appenderAttachedImpl == null || name == null ? (IAppender) null : this.m_appenderAttachedImpl.GetAppender(name);
    }

    public virtual void RemoveAllAppenders()
    {
      lock (this)
      {
        if (this.m_appenderAttachedImpl == null)
          return;
        this.m_appenderAttachedImpl.RemoveAllAppenders();
        this.m_appenderAttachedImpl = (AppenderAttachedImpl) null;
      }
    }

    public virtual IAppender RemoveAppender(IAppender appender)
    {
      lock (this)
      {
        if (appender != null)
        {
          if (this.m_appenderAttachedImpl != null)
            return this.m_appenderAttachedImpl.RemoveAppender(appender);
        }
      }
      return (IAppender) null;
    }

    public virtual IAppender RemoveAppender(string name)
    {
      lock (this)
      {
        if (name != null)
        {
          if (this.m_appenderAttachedImpl != null)
            return this.m_appenderAttachedImpl.RemoveAppender(name);
        }
      }
      return (IAppender) null;
    }
  }
}
