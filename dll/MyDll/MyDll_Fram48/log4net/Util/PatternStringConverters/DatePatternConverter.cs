// Decompiled with JetBrains decompiler
// Type: log4net.Util.PatternStringConverters.DatePatternConverter
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using log4net.Core;
using log4net.DateFormatter;
using System;
using System.Globalization;
using System.IO;

namespace log4net.Util.PatternStringConverters
{
  internal class DatePatternConverter : PatternConverter, IOptionHandler
  {
    protected IDateFormatter m_dateFormatter;

    public void ActivateOptions()
    {
      string str = this.Option ?? "ISO8601";
      if (string.Compare(str, "ISO8601", true, CultureInfo.InvariantCulture) == 0)
        this.m_dateFormatter = (IDateFormatter) new Iso8601DateFormatter();
      else if (string.Compare(str, "ABSOLUTE", true, CultureInfo.InvariantCulture) == 0)
        this.m_dateFormatter = (IDateFormatter) new AbsoluteTimeDateFormatter();
      else if (string.Compare(str, "DATE", true, CultureInfo.InvariantCulture) == 0)
      {
        this.m_dateFormatter = (IDateFormatter) new DateTimeDateFormatter();
      }
      else
      {
        try
        {
          this.m_dateFormatter = (IDateFormatter) new SimpleDateFormatter(str);
        }
        catch (Exception ex)
        {
          LogLog.Error("DatePatternConverter: Could not instantiate SimpleDateFormatter with [" + str + "]", ex);
          this.m_dateFormatter = (IDateFormatter) new Iso8601DateFormatter();
        }
      }
    }

    protected override void Convert(TextWriter writer, object state)
    {
      try
      {
        this.m_dateFormatter.FormatDate(DateTime.Now, writer);
      }
      catch (Exception ex)
      {
        LogLog.Error("DatePatternConverter: Error occurred while converting date.", ex);
      }
    }
  }
}
