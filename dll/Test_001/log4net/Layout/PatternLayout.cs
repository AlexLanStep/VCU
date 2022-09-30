// Decompiled with JetBrains decompiler
// Type: log4net.Layout.PatternLayout
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using log4net.Core;
using log4net.Layout.Pattern;
using log4net.Util;
using log4net.Util.PatternStringConverters;
using System;
using System.Collections;
using System.IO;

namespace log4net.Layout
{
  public class PatternLayout : LayoutSkeleton
  {
    public const string DefaultConversionPattern = "%message%newline";
    public const string DetailConversionPattern = "%timestamp [%thread] %level %logger %ndc - %message%newline";
    private static Hashtable s_globalRulesRegistry = new Hashtable(45);
    private string m_pattern;
    private PatternConverter m_head;
    private Hashtable m_instanceRulesRegistry = new Hashtable();

    static PatternLayout()
    {
      PatternLayout.s_globalRulesRegistry.Add((object) "literal", (object) typeof (LiteralPatternConverter));
      PatternLayout.s_globalRulesRegistry.Add((object) "newline", (object) typeof (NewLinePatternConverter));
      PatternLayout.s_globalRulesRegistry.Add((object) "n", (object) typeof (NewLinePatternConverter));
      PatternLayout.s_globalRulesRegistry.Add((object) "c", (object) typeof (LoggerPatternConverter));
      PatternLayout.s_globalRulesRegistry.Add((object) "logger", (object) typeof (LoggerPatternConverter));
      PatternLayout.s_globalRulesRegistry.Add((object) "C", (object) typeof (TypeNamePatternConverter));
      PatternLayout.s_globalRulesRegistry.Add((object) "class", (object) typeof (TypeNamePatternConverter));
      PatternLayout.s_globalRulesRegistry.Add((object) "type", (object) typeof (TypeNamePatternConverter));
      PatternLayout.s_globalRulesRegistry.Add((object) "d", (object) typeof (log4net.Layout.Pattern.DatePatternConverter));
      PatternLayout.s_globalRulesRegistry.Add((object) "date", (object) typeof (log4net.Layout.Pattern.DatePatternConverter));
      PatternLayout.s_globalRulesRegistry.Add((object) "exception", (object) typeof (ExceptionPatternConverter));
      PatternLayout.s_globalRulesRegistry.Add((object) "F", (object) typeof (FileLocationPatternConverter));
      PatternLayout.s_globalRulesRegistry.Add((object) "file", (object) typeof (FileLocationPatternConverter));
      PatternLayout.s_globalRulesRegistry.Add((object) "l", (object) typeof (FullLocationPatternConverter));
      PatternLayout.s_globalRulesRegistry.Add((object) "location", (object) typeof (FullLocationPatternConverter));
      PatternLayout.s_globalRulesRegistry.Add((object) "L", (object) typeof (LineLocationPatternConverter));
      PatternLayout.s_globalRulesRegistry.Add((object) "line", (object) typeof (LineLocationPatternConverter));
      PatternLayout.s_globalRulesRegistry.Add((object) "m", (object) typeof (MessagePatternConverter));
      PatternLayout.s_globalRulesRegistry.Add((object) "message", (object) typeof (MessagePatternConverter));
      PatternLayout.s_globalRulesRegistry.Add((object) "M", (object) typeof (MethodLocationPatternConverter));
      PatternLayout.s_globalRulesRegistry.Add((object) "method", (object) typeof (MethodLocationPatternConverter));
      PatternLayout.s_globalRulesRegistry.Add((object) "p", (object) typeof (LevelPatternConverter));
      PatternLayout.s_globalRulesRegistry.Add((object) "level", (object) typeof (LevelPatternConverter));
      PatternLayout.s_globalRulesRegistry.Add((object) "P", (object) typeof (log4net.Layout.Pattern.PropertyPatternConverter));
      PatternLayout.s_globalRulesRegistry.Add((object) "property", (object) typeof (log4net.Layout.Pattern.PropertyPatternConverter));
      PatternLayout.s_globalRulesRegistry.Add((object) "properties", (object) typeof (log4net.Layout.Pattern.PropertyPatternConverter));
      PatternLayout.s_globalRulesRegistry.Add((object) "r", (object) typeof (RelativeTimePatternConverter));
      PatternLayout.s_globalRulesRegistry.Add((object) "timestamp", (object) typeof (RelativeTimePatternConverter));
      PatternLayout.s_globalRulesRegistry.Add((object) "t", (object) typeof (ThreadPatternConverter));
      PatternLayout.s_globalRulesRegistry.Add((object) "thread", (object) typeof (ThreadPatternConverter));
      PatternLayout.s_globalRulesRegistry.Add((object) "x", (object) typeof (NdcPatternConverter));
      PatternLayout.s_globalRulesRegistry.Add((object) "ndc", (object) typeof (NdcPatternConverter));
      PatternLayout.s_globalRulesRegistry.Add((object) "X", (object) typeof (log4net.Layout.Pattern.PropertyPatternConverter));
      PatternLayout.s_globalRulesRegistry.Add((object) "mdc", (object) typeof (log4net.Layout.Pattern.PropertyPatternConverter));
      PatternLayout.s_globalRulesRegistry.Add((object) "a", (object) typeof (log4net.Layout.Pattern.AppDomainPatternConverter));
      PatternLayout.s_globalRulesRegistry.Add((object) "appdomain", (object) typeof (log4net.Layout.Pattern.AppDomainPatternConverter));
      PatternLayout.s_globalRulesRegistry.Add((object) "u", (object) typeof (log4net.Layout.Pattern.IdentityPatternConverter));
      PatternLayout.s_globalRulesRegistry.Add((object) "identity", (object) typeof (log4net.Layout.Pattern.IdentityPatternConverter));
      PatternLayout.s_globalRulesRegistry.Add((object) "utcdate", (object) typeof (log4net.Layout.Pattern.UtcDatePatternConverter));
      PatternLayout.s_globalRulesRegistry.Add((object) "utcDate", (object) typeof (log4net.Layout.Pattern.UtcDatePatternConverter));
      PatternLayout.s_globalRulesRegistry.Add((object) "UtcDate", (object) typeof (log4net.Layout.Pattern.UtcDatePatternConverter));
      PatternLayout.s_globalRulesRegistry.Add((object) "w", (object) typeof (log4net.Layout.Pattern.UserNamePatternConverter));
      PatternLayout.s_globalRulesRegistry.Add((object) "username", (object) typeof (log4net.Layout.Pattern.UserNamePatternConverter));
    }

    public PatternLayout()
      : this("%message%newline")
    {
    }

    public PatternLayout(string pattern)
    {
      this.IgnoresException = true;
      this.m_pattern = pattern;
      if (this.m_pattern == null)
        this.m_pattern = "%message%newline";
      this.ActivateOptions();
    }

    public string ConversionPattern
    {
      get => this.m_pattern;
      set => this.m_pattern = value;
    }

    protected virtual PatternParser CreatePatternParser(string pattern)
    {
      PatternParser patternParser = new PatternParser(pattern);
      foreach (DictionaryEntry dictionaryEntry in PatternLayout.s_globalRulesRegistry)
        patternParser.PatternConverters[dictionaryEntry.Key] = dictionaryEntry.Value;
      foreach (DictionaryEntry dictionaryEntry in this.m_instanceRulesRegistry)
        patternParser.PatternConverters[dictionaryEntry.Key] = dictionaryEntry.Value;
      return patternParser;
    }

    public override void ActivateOptions()
    {
      this.m_head = this.CreatePatternParser(this.m_pattern).Parse();
      for (PatternConverter patternConverter = this.m_head; patternConverter != null; patternConverter = patternConverter.Next)
      {
        if (patternConverter is PatternLayoutConverter patternLayoutConverter && !patternLayoutConverter.IgnoresException)
        {
          this.IgnoresException = false;
          break;
        }
      }
    }

    public override void Format(TextWriter writer, LoggingEvent loggingEvent)
    {
      if (writer == null)
        throw new ArgumentNullException(nameof (writer));
      if (loggingEvent == null)
        throw new ArgumentNullException(nameof (loggingEvent));
      for (PatternConverter patternConverter = this.m_head; patternConverter != null; patternConverter = patternConverter.Next)
        patternConverter.Format(writer, (object) loggingEvent);
    }

    public void AddConverter(PatternLayout.ConverterInfo converterInfo) => this.AddConverter(converterInfo.Name, converterInfo.Type);

    public void AddConverter(string name, Type type)
    {
      if (name == null)
        throw new ArgumentNullException(nameof (name));
      if (type == null)
        throw new ArgumentNullException(nameof (type));
      this.m_instanceRulesRegistry[(object) name] = typeof (PatternConverter).IsAssignableFrom(type) ? (object) type : throw new ArgumentException("The converter type specified [" + (object) type + "] must be a subclass of log4net.Util.PatternConverter", nameof (type));
    }

    public sealed class ConverterInfo
    {
      private string m_name;
      private Type m_type;

      public string Name
      {
        get => this.m_name;
        set => this.m_name = value;
      }

      public Type Type
      {
        get => this.m_type;
        set => this.m_type = value;
      }
    }
  }
}
