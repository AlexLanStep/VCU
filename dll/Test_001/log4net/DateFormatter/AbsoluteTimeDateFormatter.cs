// Decompiled with JetBrains decompiler
// Type: log4net.DateFormatter.AbsoluteTimeDateFormatter
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using System;
using System.IO;
using System.Text;
using System.Threading;

namespace log4net.DateFormatter
{
  public class AbsoluteTimeDateFormatter : IDateFormatter
  {
    public const string AbsoluteTimeDateFormat = "ABSOLUTE";
    public const string DateAndTimeDateFormat = "DATE";
    public const string Iso8601TimeDateFormat = "ISO8601";
    private static long s_lastTimeToTheSecond = 0;
    private static StringBuilder s_lastTimeBuf = new StringBuilder();
    private static string s_lastTimeString;

    protected virtual void FormatDateWithoutMillis(DateTime dateToFormat, StringBuilder buffer)
    {
      int hour = dateToFormat.Hour;
      if (hour < 10)
        buffer.Append('0');
      buffer.Append(hour);
      buffer.Append(':');
      int minute = dateToFormat.Minute;
      if (minute < 10)
        buffer.Append('0');
      buffer.Append(minute);
      buffer.Append(':');
      int second = dateToFormat.Second;
      if (second < 10)
        buffer.Append('0');
      buffer.Append(second);
    }

    public virtual void FormatDate(DateTime dateToFormat, TextWriter writer)
    {
      long num = dateToFormat.Ticks - dateToFormat.Ticks % 10000000L;
      if (AbsoluteTimeDateFormatter.s_lastTimeToTheSecond != num)
      {
        lock (AbsoluteTimeDateFormatter.s_lastTimeBuf)
        {
          if (AbsoluteTimeDateFormatter.s_lastTimeToTheSecond != num)
          {
            AbsoluteTimeDateFormatter.s_lastTimeBuf.Length = 0;
            this.FormatDateWithoutMillis(dateToFormat, AbsoluteTimeDateFormatter.s_lastTimeBuf);
            string str = AbsoluteTimeDateFormatter.s_lastTimeBuf.ToString();
            Thread.MemoryBarrier();
            AbsoluteTimeDateFormatter.s_lastTimeString = str;
            AbsoluteTimeDateFormatter.s_lastTimeToTheSecond = num;
          }
        }
      }
      writer.Write(AbsoluteTimeDateFormatter.s_lastTimeString);
      writer.Write(',');
      int millisecond = dateToFormat.Millisecond;
      if (millisecond < 100)
        writer.Write('0');
      if (millisecond < 10)
        writer.Write('0');
      writer.Write(millisecond);
    }
  }
}
