// Decompiled with JetBrains decompiler
// Type: log4net.Layout.SimpleLayout
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using log4net.Core;
using System;
using System.IO;

namespace log4net.Layout
{
  public class SimpleLayout : LayoutSkeleton
  {
    public SimpleLayout() => this.IgnoresException = true;

    public override void ActivateOptions()
    {
    }

    public override void Format(TextWriter writer, LoggingEvent loggingEvent)
    {
      if (loggingEvent == null)
        throw new ArgumentNullException(nameof (loggingEvent));
      writer.Write(loggingEvent.Level.DisplayName);
      writer.Write(" - ");
      loggingEvent.WriteRenderedMessage(writer);
      writer.WriteLine();
    }
  }
}
