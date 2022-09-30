// Decompiled with JetBrains decompiler
// Type: log4net.Appender.TextWriterAppender
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using log4net.Core;
using log4net.Layout;
using log4net.Util;
using System;
using System.IO;

namespace log4net.Appender
{
  public class TextWriterAppender : AppenderSkeleton
  {
    private QuietTextWriter m_qtw;
    private bool m_immediateFlush = true;

    public TextWriterAppender()
    {
    }

    [Obsolete("Instead use the default constructor and set the Layout & Writer properties")]
    public TextWriterAppender(ILayout layout, Stream os)
      : this(layout, (TextWriter) new StreamWriter(os))
    {
    }

    [Obsolete("Instead use the default constructor and set the Layout & Writer properties")]
    public TextWriterAppender(ILayout layout, TextWriter writer)
    {
      this.Layout = layout;
      this.Writer = writer;
    }

    public bool ImmediateFlush
    {
      get => this.m_immediateFlush;
      set => this.m_immediateFlush = value;
    }

    public virtual TextWriter Writer
    {
      get => (TextWriter) this.m_qtw;
      set
      {
        lock (this)
        {
          this.Reset();
          if (value == null)
            return;
          this.m_qtw = new QuietTextWriter(value, this.ErrorHandler);
          this.WriteHeader();
        }
      }
    }

    protected override bool PreAppendCheck()
    {
      if (!base.PreAppendCheck())
        return false;
      if (this.m_qtw == null)
      {
        this.PrepareWriter();
        if (this.m_qtw == null)
        {
          this.ErrorHandler.Error("No output stream or file set for the appender named [" + this.Name + "].");
          return false;
        }
      }
      if (!this.m_qtw.Closed)
        return true;
      this.ErrorHandler.Error("Output stream for appender named [" + this.Name + "] has been closed.");
      return false;
    }

    protected override void Append(LoggingEvent loggingEvent)
    {
      this.RenderLoggingEvent((TextWriter) this.m_qtw, loggingEvent);
      if (!this.m_immediateFlush)
        return;
      this.m_qtw.Flush();
    }

    protected override void Append(LoggingEvent[] loggingEvents)
    {
      foreach (LoggingEvent loggingEvent in loggingEvents)
        this.RenderLoggingEvent((TextWriter) this.m_qtw, loggingEvent);
      if (!this.m_immediateFlush)
        return;
      this.m_qtw.Flush();
    }

    protected override void OnClose()
    {
      lock (this)
        this.Reset();
    }

    public override IErrorHandler ErrorHandler
    {
      get => base.ErrorHandler;
      set
      {
        lock (this)
        {
          if (value == null)
          {
            LogLog.Warn("TextWriterAppender: You have tried to set a null error-handler.");
          }
          else
          {
            base.ErrorHandler = value;
            if (this.m_qtw == null)
              return;
            this.m_qtw.ErrorHandler = value;
          }
        }
      }
    }

    protected override bool RequiresLayout => true;

    protected virtual void WriteFooterAndCloseWriter()
    {
      this.WriteFooter();
      this.CloseWriter();
    }

    protected virtual void CloseWriter()
    {
      if (this.m_qtw == null)
        return;
      try
      {
        this.m_qtw.Close();
      }
      catch (Exception ex)
      {
        this.ErrorHandler.Error("Could not close writer [" + (object) this.m_qtw + "]", ex);
      }
    }

    protected virtual void Reset()
    {
      this.WriteFooterAndCloseWriter();
      this.m_qtw = (QuietTextWriter) null;
    }

    protected virtual void WriteFooter()
    {
      if (this.Layout == null || this.m_qtw == null || this.m_qtw.Closed)
        return;
      string footer = this.Layout.Footer;
      if (footer == null)
        return;
      this.m_qtw.Write(footer);
    }

    protected virtual void WriteHeader()
    {
      if (this.Layout == null || this.m_qtw == null || this.m_qtw.Closed)
        return;
      string header = this.Layout.Header;
      if (header == null)
        return;
      this.m_qtw.Write(header);
    }

    protected virtual void PrepareWriter()
    {
    }

    protected QuietTextWriter QuietWriter
    {
      get => this.m_qtw;
      set => this.m_qtw = value;
    }
  }
}
