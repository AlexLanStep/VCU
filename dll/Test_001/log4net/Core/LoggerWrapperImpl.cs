// Decompiled with JetBrains decompiler
// Type: log4net.Core.LoggerWrapperImpl
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

namespace log4net.Core
{
  public abstract class LoggerWrapperImpl : ILoggerWrapper
  {
    private readonly ILogger m_logger;

    protected LoggerWrapperImpl(ILogger logger) => this.m_logger = logger;

    public virtual ILogger Logger => this.m_logger;
  }
}
