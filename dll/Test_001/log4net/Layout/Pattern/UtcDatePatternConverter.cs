﻿// Decompiled with JetBrains decompiler
// Type: log4net.Layout.Pattern.UtcDatePatternConverter
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using log4net.Core;
using log4net.Util;
using System;
using System.IO;

namespace log4net.Layout.Pattern
{
  internal class UtcDatePatternConverter : DatePatternConverter
  {
    protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
    {
      try
      {
        this.m_dateFormatter.FormatDate(loggingEvent.TimeStamp.ToUniversalTime(), writer);
      }
      catch (Exception ex)
      {
        LogLog.Error("UtcDatePatternConverter: Error occurred while converting date.", ex);
      }
    }
  }
}
