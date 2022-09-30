// Decompiled with JetBrains decompiler
// Type: log4net.Util.PatternParser
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using log4net.Core;
using System;
using System.Collections;
using System.Globalization;

namespace log4net.Util
{
  public sealed class PatternParser
  {
    private const char ESCAPE_CHAR = '%';
    private PatternConverter m_head;
    private PatternConverter m_tail;
    private string m_pattern;
    private Hashtable m_patternConverters = new Hashtable();

    public PatternParser(string pattern) => this.m_pattern = pattern;

    public PatternConverter Parse()
    {
      this.ParseInternal(this.m_pattern, this.BuildCache());
      return this.m_head;
    }

    public Hashtable PatternConverters => this.m_patternConverters;

    private string[] BuildCache()
    {
      string[] strArray = new string[this.m_patternConverters.Keys.Count];
      this.m_patternConverters.Keys.CopyTo((Array) strArray, 0);
      Array.Sort((Array) strArray, 0, strArray.Length, (IComparer) PatternParser.StringLengthComparer.Instance);
      return strArray;
    }

    private void ParseInternal(string pattern, string[] matches)
    {
      int num1 = 0;
      while (num1 < pattern.Length)
      {
        int num2 = pattern.IndexOf('%', num1);
        if (num2 < 0 || num2 == pattern.Length - 1)
        {
          this.ProcessLiteral(pattern.Substring(num1));
          num1 = pattern.Length;
        }
        else if (pattern[num2 + 1] == '%')
        {
          this.ProcessLiteral(pattern.Substring(num1, num2 - num1 + 1));
          num1 = num2 + 2;
        }
        else
        {
          this.ProcessLiteral(pattern.Substring(num1, num2 - num1));
          num1 = num2 + 1;
          FormattingInfo formattingInfo = new FormattingInfo();
          if (num1 < pattern.Length && pattern[num1] == '-')
          {
            formattingInfo.LeftAlign = true;
            ++num1;
          }
          for (; num1 < pattern.Length && char.IsDigit(pattern[num1]); ++num1)
          {
            if (formattingInfo.Min < 0)
              formattingInfo.Min = 0;
            formattingInfo.Min = formattingInfo.Min * 10 + int.Parse(pattern[num1].ToString((IFormatProvider) CultureInfo.InvariantCulture), (IFormatProvider) NumberFormatInfo.InvariantInfo);
          }
          if (num1 < pattern.Length && pattern[num1] == '.')
            ++num1;
          for (; num1 < pattern.Length && char.IsDigit(pattern[num1]); ++num1)
          {
            if (formattingInfo.Max == int.MaxValue)
              formattingInfo.Max = 0;
            formattingInfo.Max = formattingInfo.Max * 10 + int.Parse(pattern[num1].ToString((IFormatProvider) CultureInfo.InvariantCulture), (IFormatProvider) NumberFormatInfo.InvariantInfo);
          }
          int num3 = pattern.Length - num1;
          for (int index = 0; index < matches.Length; ++index)
          {
            if (matches[index].Length <= num3 && string.Compare(pattern, num1, matches[index], 0, matches[index].Length, false, CultureInfo.InvariantCulture) == 0)
            {
              num1 += matches[index].Length;
              string option = (string) null;
              if (num1 < pattern.Length && pattern[num1] == '{')
              {
                ++num1;
                int num4 = pattern.IndexOf('}', num1);
                if (num4 >= 0)
                {
                  option = pattern.Substring(num1, num4 - num1);
                  num1 = num4 + 1;
                }
              }
              this.ProcessConverter(matches[index], option, formattingInfo);
              break;
            }
          }
        }
      }
    }

    private void ProcessLiteral(string text)
    {
      if (text.Length <= 0)
        return;
      this.ProcessConverter("literal", text, new FormattingInfo());
    }

    private void ProcessConverter(
      string converterName,
      string option,
      FormattingInfo formattingInfo)
    {
      LogLog.Debug("PatternParser: Converter [" + converterName + "] Option [" + option + "] Format [min=" + (object) formattingInfo.Min + ",max=" + (object) formattingInfo.Max + ",leftAlign=" + (object) formattingInfo.LeftAlign + "]");
      Type patternConverter = (Type) this.m_patternConverters[(object) converterName];
      if (patternConverter == null)
      {
        LogLog.Error("PatternParser: Unknown converter name [" + converterName + "] in conversion pattern.");
      }
      else
      {
        PatternConverter pc = (PatternConverter) null;
        try
        {
          pc = (PatternConverter) Activator.CreateInstance(patternConverter);
        }
        catch (Exception ex)
        {
          LogLog.Error("PatternParser: Failed to create instance of Type [" + patternConverter.FullName + "] using default constructor. Exception: " + ex.ToString());
        }
        pc.FormattingInfo = formattingInfo;
        pc.Option = option;
        if (pc is IOptionHandler optionHandler)
          optionHandler.ActivateOptions();
        this.AddConverter(pc);
      }
    }

    private void AddConverter(PatternConverter pc)
    {
      if (this.m_head == null)
        this.m_head = this.m_tail = pc;
      else
        this.m_tail = this.m_tail.SetNext(pc);
    }

    private sealed class StringLengthComparer : IComparer
    {
      public static readonly PatternParser.StringLengthComparer Instance = new PatternParser.StringLengthComparer();

      private StringLengthComparer()
      {
      }

      public int Compare(object x, object y)
      {
        string str1 = x as string;
        string str2 = y as string;
        if (str1 == null && str2 == null)
          return 0;
        if (str1 == null)
          return 1;
        return str2 == null ? -1 : str2.Length.CompareTo((object) str1.Length);
      }
    }
  }
}
