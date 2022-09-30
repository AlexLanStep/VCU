// Decompiled with JetBrains decompiler
// Type: Common.Logging.ILog
// Assembly: Common.Logging, Version=1.1.0.1, Culture=neutral, PublicKeyToken=65e474d141e25e07
// MVID: F190E754-F88D-465E-80CE-5B5FC84116FB
// Assembly location: E:\LabCar\BasaDll\Common.Logging.dll

using System;

namespace Common.Logging
{
  public interface ILog
  {
    void Debug(object message);

    void Debug(object message, Exception exception);

    void Error(object message);

    void Error(object message, Exception exception);

    void Fatal(object message);

    void Fatal(object message, Exception exception);

    void Info(object message);

    void Info(object message, Exception exception);

    void Warn(object message);

    void Warn(object message, Exception exception);

    bool IsDebugEnabled { get; }

    bool IsErrorEnabled { get; }

    bool IsFatalEnabled { get; }

    bool IsInfoEnabled { get; }

    bool IsWarnEnabled { get; }
  }
}
