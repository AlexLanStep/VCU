// Decompiled with JetBrains decompiler
// Type: log4net.Filter.IFilter
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using log4net.Core;

namespace log4net.Filter
{
  public interface IFilter : IOptionHandler
  {
    FilterDecision Decide(LoggingEvent loggingEvent);

    IFilter Next { get; set; }
  }
}
