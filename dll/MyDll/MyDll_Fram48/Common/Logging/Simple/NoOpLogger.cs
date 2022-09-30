
using System;

namespace Common.Logging.Simple
{
  public sealed class NoOpLogger : ILog
  {
    public void Debug(object message)
    {
    }

    public void Debug(object message, Exception e)
    {
    }

    public void Error(object message)
    {
    }

    public void Error(object message, Exception e)
    {
    }

    public void Fatal(object message)
    {
    }

    public void Fatal(object message, Exception e)
    {
    }

    public void Info(object message)
    {
    }

    public void Info(object message, Exception e)
    {
    }

    public void Warn(object message)
    {
    }

    public void Warn(object message, Exception e)
    {
    }

    public bool IsDebugEnabled => false;

    public bool IsErrorEnabled => false;

    public bool IsFatalEnabled => false;

    public bool IsInfoEnabled => false;

    public bool IsWarnEnabled => false;
  }
}
