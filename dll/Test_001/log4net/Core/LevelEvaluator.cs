// Decompiled with JetBrains decompiler
// Type: log4net.Core.LevelEvaluator
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using System;

namespace log4net.Core
{
  public class LevelEvaluator : ITriggeringEventEvaluator
  {
    private Level m_threshold;

    public LevelEvaluator()
      : this(Level.Off)
    {
    }

    public LevelEvaluator(Level threshold) => this.m_threshold = !(threshold == (Level) null) ? threshold : throw new ArgumentNullException(nameof (threshold));

    public Level Threshold
    {
      get => this.m_threshold;
      set => this.m_threshold = value;
    }

    public bool IsTriggeringEvent(LoggingEvent loggingEvent)
    {
      if (loggingEvent == null)
        throw new ArgumentNullException(nameof (loggingEvent));
      return loggingEvent.Level >= this.m_threshold;
    }
  }
}
