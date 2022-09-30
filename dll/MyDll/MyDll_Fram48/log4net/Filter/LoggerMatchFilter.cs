// Decompiled with JetBrains decompiler
// Type: log4net.Filter.LoggerMatchFilter
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using log4net.Core;
using System;

namespace log4net.Filter
{
  public class LoggerMatchFilter : FilterSkeleton
  {
    private bool m_acceptOnMatch = true;
    private string m_loggerToMatch;

    public bool AcceptOnMatch
    {
      get => this.m_acceptOnMatch;
      set => this.m_acceptOnMatch = value;
    }

    public string LoggerToMatch
    {
      get => this.m_loggerToMatch;
      set => this.m_loggerToMatch = value;
    }

    public override FilterDecision Decide(LoggingEvent loggingEvent)
    {
      if (loggingEvent == null)
        throw new ArgumentNullException(nameof (loggingEvent));
      if (this.m_loggerToMatch == null || this.m_loggerToMatch.Length == 0 || !loggingEvent.LoggerName.StartsWith(this.m_loggerToMatch))
        return FilterDecision.Neutral;
      return this.m_acceptOnMatch ? FilterDecision.Accept : FilterDecision.Deny;
    }
  }
}
