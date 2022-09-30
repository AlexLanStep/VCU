// Decompiled with JetBrains decompiler
// Type: log4net.Appender.AspNetTraceAppender
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using log4net.Core;
using System.Web;

namespace log4net.Appender
{
  public class AspNetTraceAppender : AppenderSkeleton
  {
    protected override void Append(LoggingEvent loggingEvent)
    {
      if (HttpContext.Current == null || !HttpContext.Current.Trace.IsEnabled)
        return;
      if (loggingEvent.Level >= Level.Warn)
        HttpContext.Current.Trace.Warn(loggingEvent.LoggerName, this.RenderLoggingEvent(loggingEvent));
      else
        HttpContext.Current.Trace.Write(loggingEvent.LoggerName, this.RenderLoggingEvent(loggingEvent));
    }

    protected override bool RequiresLayout => true;
  }
}
