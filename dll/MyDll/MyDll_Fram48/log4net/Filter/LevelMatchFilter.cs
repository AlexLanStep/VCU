// Decompiled with JetBrains decompiler
// Type: log4net.Filter.LevelMatchFilter
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using log4net.Core;
using System;

namespace log4net.Filter
{
  public class LevelMatchFilter : FilterSkeleton
  {
    private bool m_acceptOnMatch = true;
    private Level m_levelToMatch;

    public bool AcceptOnMatch
    {
      get => this.m_acceptOnMatch;
      set => this.m_acceptOnMatch = value;
    }

    public Level LevelToMatch
    {
      get => this.m_levelToMatch;
      set => this.m_levelToMatch = value;
    }

    public override FilterDecision Decide(LoggingEvent loggingEvent)
    {
      if (loggingEvent == null)
        throw new ArgumentNullException(nameof (loggingEvent));
      return this.m_levelToMatch != (Level) null && this.m_levelToMatch == loggingEvent.Level ? (this.m_acceptOnMatch ? FilterDecision.Accept : FilterDecision.Deny) : FilterDecision.Neutral;
    }
  }
}
