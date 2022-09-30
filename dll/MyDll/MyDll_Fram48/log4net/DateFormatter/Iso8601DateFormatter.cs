// Decompiled with JetBrains decompiler
// Type: log4net.DateFormatter.Iso8601DateFormatter
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using System;
using System.Text;

namespace log4net.DateFormatter
{
  public class Iso8601DateFormatter : AbsoluteTimeDateFormatter
  {
    protected override void FormatDateWithoutMillis(DateTime dateToFormat, StringBuilder buffer)
    {
      buffer.Append(dateToFormat.Year);
      buffer.Append('-');
      int month = dateToFormat.Month;
      if (month < 10)
        buffer.Append('0');
      buffer.Append(month);
      buffer.Append('-');
      int day = dateToFormat.Day;
      if (day < 10)
        buffer.Append('0');
      buffer.Append(day);
      buffer.Append(' ');
      base.FormatDateWithoutMillis(dateToFormat, buffer);
    }
  }
}
