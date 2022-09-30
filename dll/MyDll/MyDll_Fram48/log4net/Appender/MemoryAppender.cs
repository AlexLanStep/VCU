// Decompiled with JetBrains decompiler
// Type: log4net.Appender.MemoryAppender
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using log4net.Core;
using System;
using System.Collections;

namespace log4net.Appender
{
  public class MemoryAppender : AppenderSkeleton
  {
    protected ArrayList m_eventsList;
    protected FixFlags m_fixFlags = FixFlags.All;

    public MemoryAppender() => this.m_eventsList = new ArrayList();

    public virtual LoggingEvent[] GetEvents() => (LoggingEvent[]) this.m_eventsList.ToArray(typeof (LoggingEvent));

    [Obsolete("Use Fix property")]
    public virtual bool OnlyFixPartialEventData
    {
      get => this.Fix == FixFlags.Partial;
      set
      {
        if (value)
          this.Fix = FixFlags.Partial;
        else
          this.Fix = FixFlags.All;
      }
    }

    public virtual FixFlags Fix
    {
      get => this.m_fixFlags;
      set => this.m_fixFlags = value;
    }

    protected override void Append(LoggingEvent loggingEvent)
    {
      loggingEvent.Fix = this.Fix;
      this.m_eventsList.Add((object) loggingEvent);
    }

    public virtual void Clear() => this.m_eventsList.Clear();
  }
}
