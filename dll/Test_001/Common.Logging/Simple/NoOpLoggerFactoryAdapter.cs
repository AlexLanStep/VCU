// Decompiled with JetBrains decompiler
// Type: Common.Logging.Simple.NoOpLoggerFactoryAdapter
// Assembly: Common.Logging, Version=1.1.0.1, Culture=neutral, PublicKeyToken=65e474d141e25e07
// MVID: F190E754-F88D-465E-80CE-5B5FC84116FB
// Assembly location: E:\LabCar\BasaDll\Common.Logging.dll

using System;
using System.Collections.Specialized;

namespace Common.Logging.Simple
{
  public sealed class NoOpLoggerFactoryAdapter : ILoggerFactoryAdapter
  {
    private ILog _nopLogger = (ILog) null;

    public NoOpLoggerFactoryAdapter() => this._nopLogger = (ILog) new NoOpLogger();

    public NoOpLoggerFactoryAdapter(NameValueCollection properties) => this._nopLogger = (ILog) new NoOpLogger();

    public ILog GetLogger(Type type) => this._nopLogger;

    ILog ILoggerFactoryAdapter.GetLogger(string name) => this._nopLogger;
  }
}
