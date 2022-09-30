// Decompiled with JetBrains decompiler
// Type: log4net.Layout.IRawLayout
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using log4net.Core;
using log4net.Util.TypeConverters;

namespace log4net.Layout
{
  [TypeConverter(typeof (RawLayoutConverter))]
  public interface IRawLayout
  {
    object Format(LoggingEvent loggingEvent);
  }
}
