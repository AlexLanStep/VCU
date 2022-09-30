// Decompiled with JetBrains decompiler
// Type: log4net.Appender.DebugAppender
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using log4net.Core;
using log4net.Layout;
using System;
using System.Diagnostics;

namespace log4net.Appender
{
  public class DebugAppender : AppenderSkeleton
  {
    private bool m_immediateFlush = true;

    public DebugAppender()
    {
    }

    [Obsolete("Instead use the default constructor and set the Layout property")]
    public DebugAppender(ILayout layout) => this.Layout = layout;

    public bool ImmediateFlush
    {
      get => this.m_immediateFlush;
      set => this.m_immediateFlush = value;
    }

    protected override void Append(LoggingEvent loggingEvent)
    {
      Debug.Write(this.RenderLoggingEvent(loggingEvent), loggingEvent.LoggerName);
      if (!this.m_immediateFlush)
        return;
      Debug.Flush();
    }

    protected override bool RequiresLayout => true;
  }
}
