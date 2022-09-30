// Decompiled with JetBrains decompiler
// Type: ETAS.LCA.Logging.ILog
// Assembly: ETAS.LCA.Logging, Version=1.0.0.0, Culture=neutral, PublicKeyToken=24eada690b1476fd
// MVID: 3E0EA91C-5947-4488-8BAF-799929BC7052
// Assembly location: E:\LabCar\BasaDll\ETAS.LCA.Logging.dll

using System;

namespace ETAS.LCA.Logging
{
  public interface ILog
  {
    bool IsFineEnabled { get; }

    bool IsFinerEnabled { get; }

    bool IsFinestEnabled { get; }

    bool IsFatalEnabled { get; }

    bool IsErrorEnabled { get; }

    bool IsWarnEnabled { get; }

    bool IsInfoEnabled { get; }

    bool IsDebugEnabled { get; }

    void Entering(string method);

    void Exiting(string method);

    void EnteringFiner(string method);

    void ExitingFiner(string method);

    void EnteringFinest(string method);

    void ExitingFinest(string method);

    void Fatal(object message);

    void Fatal(object message, Exception exception);

    void FatalFormat(IFormatProvider provider, string format, params object[] args);

    void FatalFormat(string format, params object[] args);

    void FatalFormat(string format, object arg0);

    void FatalFormat(string format, object arg0, object arg1);

    void FatalFormat(string format, object arg0, object arg1, object arg2);

    void Error(object message);

    void Error(object message, Exception exception);

    void ErrorFormat(IFormatProvider provider, string format, params object[] args);

    void ErrorFormat(string format, params object[] args);

    void ErrorFormat(string format, object arg0);

    void ErrorFormat(string format, object arg0, object arg1);

    void ErrorFormat(string format, object arg0, object arg1, object arg2);

    void Warn(object message);

    void Warn(object message, Exception exception);

    void WarnFormat(IFormatProvider provider, string format, params object[] args);

    void WarnFormat(string format, params object[] args);

    void WarnFormat(string format, object arg0);

    void WarnFormat(string format, object arg0, object arg1);

    void WarnFormat(string format, object arg0, object arg1, object arg2);

    void Info(object message);

    void Info(object message, Exception exception);

    void InfoFormat(IFormatProvider provider, string format, params object[] args);

    void InfoFormat(string format, params object[] args);

    void InfoFormat(string format, object arg0);

    void InfoFormat(string format, object arg0, object arg1);

    void InfoFormat(string format, object arg0, object arg1, object arg2);

    void Debug(object message);

    void Debug(object message, Exception exception);

    void DebugFormat(IFormatProvider provider, string format, params object[] args);

    void DebugFormat(string format, params object[] args);

    void DebugFormat(string format, object arg0);

    void DebugFormat(string format, object arg0, object arg1);

    void DebugFormat(string format, object arg0, object arg1, object arg2);

    void Fine(object message);

    void Fine(object message, Exception exception);

    void FineFormat(IFormatProvider provider, string format, params object[] args);

    void FineFormat(string format, params object[] args);

    void FineFormat(string format, object arg0);

    void FineFormat(string format, object arg0, object arg1);

    void FineFormat(string format, object arg0, object arg1, object arg2);

    void Finer(object message);

    void Finer(object message, Exception exception);

    void FinerFormat(IFormatProvider provider, string format, params object[] args);

    void FinerFormat(string format, params object[] args);

    void FinerFormat(string format, object arg0);

    void FinerFormat(string format, object arg0, object arg1);

    void FinerFormat(string format, object arg0, object arg1, object arg2);

    void Finest(object message);

    void Finest(object message, Exception exception);

    void FinestFormat(IFormatProvider provider, string format, params object[] args);

    void FinestFormat(string format, params object[] args);

    void FinestFormat(string format, object arg0);

    void FinestFormat(string format, object arg0, object arg1);

    void FinestFormat(string format, object arg0, object arg1, object arg2);
  }
}
