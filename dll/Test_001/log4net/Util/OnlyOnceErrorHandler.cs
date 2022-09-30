// Decompiled with JetBrains decompiler
// Type: log4net.Util.OnlyOnceErrorHandler
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using log4net.Core;
using System;

namespace log4net.Util
{
  public class OnlyOnceErrorHandler : IErrorHandler
  {
    private bool m_firstTime = true;
    private readonly string m_prefix;

    public OnlyOnceErrorHandler() => this.m_prefix = "";

    public OnlyOnceErrorHandler(string prefix) => this.m_prefix = prefix;

    public void Error(string message, Exception e, ErrorCode errorCode)
    {
      if (!this.IsEnabled)
        return;
      LogLog.Error("[" + this.m_prefix + "] " + message, e);
    }

    public void Error(string message, Exception e)
    {
      if (!this.IsEnabled)
        return;
      LogLog.Error("[" + this.m_prefix + "] " + message, e);
    }

    public void Error(string message)
    {
      if (!this.IsEnabled)
        return;
      LogLog.Error("[" + this.m_prefix + "] " + message);
    }

    private bool IsEnabled
    {
      get
      {
        if (this.m_firstTime)
        {
          this.m_firstTime = false;
          return true;
        }
        return LogLog.InternalDebugging && !LogLog.QuietMode;
      }
    }
  }
}
