// Decompiled with JetBrains decompiler
// Type: log4net.Layout.Pattern.PatternLayoutConverter
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using log4net.Core;
using log4net.Util;
using System;
using System.IO;

namespace log4net.Layout.Pattern
{
  public abstract class PatternLayoutConverter : PatternConverter
  {
    private bool m_ignoresException = true;

    public virtual bool IgnoresException
    {
      get => this.m_ignoresException;
      set => this.m_ignoresException = value;
    }

    protected abstract void Convert(TextWriter writer, LoggingEvent loggingEvent);

    protected override void Convert(TextWriter writer, object state)
    {
      if (!(state is LoggingEvent loggingEvent))
        throw new ArgumentException("state must be of type [" + typeof (LoggingEvent).FullName + "]", nameof (state));
      this.Convert(writer, loggingEvent);
    }
  }
}
