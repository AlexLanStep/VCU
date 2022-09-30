// Decompiled with JetBrains decompiler
// Type: log4net.Filter.StringMatchFilter
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using log4net.Core;
using System;
using System.Text.RegularExpressions;

namespace log4net.Filter
{
  public class StringMatchFilter : FilterSkeleton
  {
    protected bool m_acceptOnMatch = true;
    protected string m_stringToMatch;
    protected string m_stringRegexToMatch;
    protected Regex m_regexToMatch;

    public override void ActivateOptions()
    {
      if (this.m_stringRegexToMatch == null)
        return;
      this.m_regexToMatch = new Regex(this.m_stringRegexToMatch, RegexOptions.Compiled);
    }

    public bool AcceptOnMatch
    {
      get => this.m_acceptOnMatch;
      set => this.m_acceptOnMatch = value;
    }

    public string StringToMatch
    {
      get => this.m_stringToMatch;
      set => this.m_stringToMatch = value;
    }

    public string RegexToMatch
    {
      get => this.m_stringRegexToMatch;
      set => this.m_stringRegexToMatch = value;
    }

    public override FilterDecision Decide(LoggingEvent loggingEvent)
    {
      string input = loggingEvent != null ? loggingEvent.RenderedMessage : throw new ArgumentNullException(nameof (loggingEvent));
      if (input == null || this.m_stringToMatch == null && this.m_regexToMatch == null)
        return FilterDecision.Neutral;
      if (this.m_regexToMatch != null)
      {
        if (!this.m_regexToMatch.Match(input).Success)
          return FilterDecision.Neutral;
        return this.m_acceptOnMatch ? FilterDecision.Accept : FilterDecision.Deny;
      }
      if (this.m_stringToMatch == null || input.IndexOf(this.m_stringToMatch) == -1)
        return FilterDecision.Neutral;
      return this.m_acceptOnMatch ? FilterDecision.Accept : FilterDecision.Deny;
    }
  }
}
