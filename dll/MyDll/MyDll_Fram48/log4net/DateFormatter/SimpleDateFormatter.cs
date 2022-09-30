// Decompiled with JetBrains decompiler
// Type: log4net.DateFormatter.SimpleDateFormatter
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using System;
using System.Globalization;
using System.IO;

namespace log4net.DateFormatter
{
  public class SimpleDateFormatter : IDateFormatter
  {
    private readonly string m_formatString;

    public SimpleDateFormatter(string format) => this.m_formatString = format;

    public virtual void FormatDate(DateTime dateToFormat, TextWriter writer) => writer.Write(dateToFormat.ToString(this.m_formatString, (IFormatProvider) DateTimeFormatInfo.InvariantInfo));
  }
}
