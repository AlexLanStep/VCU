// Decompiled with JetBrains decompiler
// Type: log4net.Appender.OutputDebugStringAppender
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using log4net.Core;
using System.Runtime.InteropServices;

namespace log4net.Appender
{
  public class OutputDebugStringAppender : AppenderSkeleton
  {
    protected override void Append(LoggingEvent loggingEvent) => OutputDebugStringAppender.OutputDebugString(this.RenderLoggingEvent(loggingEvent));

    protected override bool RequiresLayout => true;

    [DllImport("Kernel32.dll")]
    protected static extern void OutputDebugString(string message);
  }
}
