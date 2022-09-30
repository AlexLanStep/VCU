// Decompiled with JetBrains decompiler
// Type: log4net.Appender.AppenderSkeleton
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using log4net.Core;
using log4net.Filter;
using log4net.Layout;
using log4net.Util;
using System;
using System.Collections;
using System.Globalization;
using System.IO;

namespace log4net.Appender
{
  public abstract class AppenderSkeleton : IBulkAppender, IAppender, IOptionHandler
  {
    private const int c_renderBufferSize = 256;
    private const int c_renderBufferMaxCapacity = 1024;
    private ILayout m_layout;
    private string m_name;
    private Level m_threshold;
    private IErrorHandler m_errorHandler;
    private IFilter m_headFilter;
    private IFilter m_tailFilter;
    private bool m_closed = false;
    private bool m_recursiveGuard = false;
    private ReusableStringWriter m_renderWriter = (ReusableStringWriter) null;

    protected AppenderSkeleton() => this.m_errorHandler = (IErrorHandler) new OnlyOnceErrorHandler(this.GetType().Name);

    ~AppenderSkeleton()
    {
      if (this.m_closed)
        return;
      LogLog.Debug("AppenderSkeleton: Finalizing appender named [" + this.m_name + "].");
      this.Close();
    }

    public Level Threshold
    {
      get => this.m_threshold;
      set => this.m_threshold = value;
    }

    public virtual IErrorHandler ErrorHandler
    {
      get => this.m_errorHandler;
      set
      {
        lock (this)
        {
          if (value == null)
            LogLog.Warn("AppenderSkeleton: You have tried to set a null error-handler.");
          else
            this.m_errorHandler = value;
        }
      }
    }

    public virtual IFilter FilterHead => this.m_headFilter;

    public virtual ILayout Layout
    {
      get => this.m_layout;
      set => this.m_layout = value;
    }

    public virtual void ActivateOptions()
    {
    }

    public string Name
    {
      get => this.m_name;
      set => this.m_name = value;
    }

    public void Close()
    {
      lock (this)
      {
        if (this.m_closed)
          return;
        this.OnClose();
        this.m_closed = true;
      }
    }

    public void DoAppend(LoggingEvent loggingEvent)
    {
      lock (this)
      {
        if (this.m_closed)
        {
          this.ErrorHandler.Error("Attempted to append to closed appender named [" + this.m_name + "].");
        }
        else
        {
          if (this.m_recursiveGuard)
            return;
          try
          {
            this.m_recursiveGuard = true;
            if (!this.FilterEvent(loggingEvent) || !this.PreAppendCheck())
              return;
            this.Append(loggingEvent);
          }
          catch (Exception ex)
          {
            this.ErrorHandler.Error("Failed in DoAppend", ex);
          }
          catch
          {
            this.ErrorHandler.Error("Failed in DoAppend (unknown exception)");
          }
          finally
          {
            this.m_recursiveGuard = false;
          }
        }
      }
    }

    public void DoAppend(LoggingEvent[] loggingEvents)
    {
      lock (this)
      {
        if (this.m_closed)
        {
          this.ErrorHandler.Error("Attempted to append to closed appender named [" + this.m_name + "].");
        }
        else
        {
          if (this.m_recursiveGuard)
            return;
          try
          {
            this.m_recursiveGuard = true;
            ArrayList arrayList = new ArrayList(loggingEvents.Length);
            foreach (LoggingEvent loggingEvent in loggingEvents)
            {
              if (this.FilterEvent(loggingEvent))
                arrayList.Add((object) loggingEvent);
            }
            if (arrayList.Count <= 0 || !this.PreAppendCheck())
              return;
            this.Append((LoggingEvent[]) arrayList.ToArray(typeof (LoggingEvent)));
          }
          catch (Exception ex)
          {
            this.ErrorHandler.Error("Failed in Bulk DoAppend", ex);
          }
          catch
          {
            this.ErrorHandler.Error("Failed in Bulk DoAppend (unknown exception)");
          }
          finally
          {
            this.m_recursiveGuard = false;
          }
        }
      }
    }

    protected virtual bool FilterEvent(LoggingEvent loggingEvent)
    {
      if (!this.IsAsSevereAsThreshold(loggingEvent.Level))
        return false;
      IFilter filter = this.FilterHead;
      while (filter != null)
      {
        switch (filter.Decide(loggingEvent))
        {
          case FilterDecision.Deny:
            return false;
          case FilterDecision.Neutral:
            filter = filter.Next;
            continue;
          case FilterDecision.Accept:
            filter = (IFilter) null;
            continue;
          default:
            continue;
        }
      }
      return true;
    }

    public virtual void AddFilter(IFilter filter)
    {
      if (filter == null)
        throw new ArgumentNullException("filter param must not be null");
      if (this.m_headFilter == null)
      {
        this.m_headFilter = this.m_tailFilter = filter;
      }
      else
      {
        this.m_tailFilter.Next = filter;
        this.m_tailFilter = filter;
      }
    }

    public virtual void ClearFilters() => this.m_headFilter = this.m_tailFilter = (IFilter) null;

    protected virtual bool IsAsSevereAsThreshold(Level level) => this.m_threshold == (Level) null || level >= this.m_threshold;

    protected virtual void OnClose()
    {
    }

    protected abstract void Append(LoggingEvent loggingEvent);

    protected virtual void Append(LoggingEvent[] loggingEvents)
    {
      foreach (LoggingEvent loggingEvent in loggingEvents)
        this.Append(loggingEvent);
    }

    protected virtual bool PreAppendCheck()
    {
      if (this.m_layout != null || !this.RequiresLayout)
        return true;
      this.ErrorHandler.Error("AppenderSkeleton: No layout set for the appender named [" + this.m_name + "].");
      return false;
    }

    protected string RenderLoggingEvent(LoggingEvent loggingEvent)
    {
      if (this.m_renderWriter == null)
        this.m_renderWriter = new ReusableStringWriter((IFormatProvider) CultureInfo.InvariantCulture);
      this.m_renderWriter.Reset(1024, 256);
      this.RenderLoggingEvent((TextWriter) this.m_renderWriter, loggingEvent);
      return this.m_renderWriter.ToString();
    }

    protected void RenderLoggingEvent(TextWriter writer, LoggingEvent loggingEvent)
    {
      if (this.m_layout == null)
        throw new InvalidOperationException("A layout must be set");
      if (this.m_layout.IgnoresException)
      {
        string exceptionString = loggingEvent.GetExceptionString();
        if (exceptionString != null && exceptionString.Length > 0)
        {
          this.m_layout.Format(writer, loggingEvent);
          writer.WriteLine(exceptionString);
        }
        else
          this.m_layout.Format(writer, loggingEvent);
      }
      else
        this.m_layout.Format(writer, loggingEvent);
    }

    protected virtual bool RequiresLayout => false;
  }
}
