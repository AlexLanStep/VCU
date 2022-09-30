// Decompiled with JetBrains decompiler
// Type: log4net.Util.TypeConverters.PatternLayoutConverter
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using log4net.Layout;
using System;

namespace log4net.Util.TypeConverters
{
  internal class PatternLayoutConverter : IConvertFrom
  {
    public bool CanConvertFrom(Type sourceType) => sourceType == typeof (string);

    public object ConvertFrom(object source) => source is string pattern ? (object) new PatternLayout(pattern) : throw ConversionNotSupportedException.Create(typeof (PatternLayout), source);
  }
}
