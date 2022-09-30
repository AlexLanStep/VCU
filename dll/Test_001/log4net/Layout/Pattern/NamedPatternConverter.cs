// Decompiled with JetBrains decompiler
// Type: log4net.Layout.Pattern.NamedPatternConverter
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using log4net.Core;
using log4net.Util;
using System.IO;

namespace log4net.Layout.Pattern
{
  internal abstract class NamedPatternConverter : PatternLayoutConverter, IOptionHandler
  {
    protected int m_precision = 0;

    public void ActivateOptions()
    {
      this.m_precision = 0;
      if (this.Option == null)
        return;
      string s = this.Option.Trim();
      if (s.Length <= 0)
        return;
      int val;
      if (SystemInfo.TryParse(s, out val))
      {
        if (val <= 0)
          LogLog.Error("NamedPatternConverter: Precision option (" + s + ") isn't a positive integer.");
        else
          this.m_precision = val;
      }
      else
        LogLog.Error("NamedPatternConverter: Precision option \"" + s + "\" not a decimal integer.");
    }

    protected abstract string GetFullyQualifiedName(LoggingEvent loggingEvent);

    protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
    {
      string fullyQualifiedName = this.GetFullyQualifiedName(loggingEvent);
      if (this.m_precision <= 0)
      {
        writer.Write(fullyQualifiedName);
      }
      else
      {
        int length = fullyQualifiedName.Length;
        int num = length - 1;
        for (int precision = this.m_precision; precision > 0; --precision)
        {
          num = fullyQualifiedName.LastIndexOf('.', num - 1);
          if (num == -1)
          {
            writer.Write(fullyQualifiedName);
            return;
          }
        }
        writer.Write(fullyQualifiedName.Substring(num + 1, length - num - 1));
      }
    }
  }
}
