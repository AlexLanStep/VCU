// Decompiled with JetBrains decompiler
// Type: log4net.Filter.PropertyFilter
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using log4net.Core;
using System;

namespace log4net.Filter
{
  public class PropertyFilter : StringMatchFilter
  {
    private string m_key;

    public string Key
    {
      get => this.m_key;
      set => this.m_key = value;
    }

    public override FilterDecision Decide(LoggingEvent loggingEvent)
    {
      if (loggingEvent == null)
        throw new ArgumentNullException(nameof (loggingEvent));
      if (this.m_key == null)
        return FilterDecision.Neutral;
      object obj = loggingEvent.LookupProperty(this.m_key);
      string andRender = loggingEvent.Repository.RendererMap.FindAndRender(obj);
      if (andRender == null || this.m_stringToMatch == null && this.m_regexToMatch == null)
        return FilterDecision.Neutral;
      if (this.m_regexToMatch != null)
      {
        if (!this.m_regexToMatch.Match(andRender).Success)
          return FilterDecision.Neutral;
        return this.m_acceptOnMatch ? FilterDecision.Accept : FilterDecision.Deny;
      }
      if (this.m_stringToMatch == null || andRender.IndexOf(this.m_stringToMatch) == -1)
        return FilterDecision.Neutral;
      return this.m_acceptOnMatch ? FilterDecision.Accept : FilterDecision.Deny;
    }
  }
}
