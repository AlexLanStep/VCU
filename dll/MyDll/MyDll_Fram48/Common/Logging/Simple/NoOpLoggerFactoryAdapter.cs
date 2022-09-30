
using System;
using System.Collections.Specialized;

namespace Common.Logging.Simple
{

  public sealed class NoOpLoggerFactoryAdapter : ILoggerFactoryAdapter
  {
    private ILog _nopLogger = (ILog)null;

    public NoOpLoggerFactoryAdapter() => this._nopLogger = (ILog)new NoOpLogger();

    public NoOpLoggerFactoryAdapter(NameValueCollection properties) => this._nopLogger = (ILog)new NoOpLogger();

    public ILog GetLogger(Type type) => this._nopLogger;

    ILog ILoggerFactoryAdapter.GetLogger(string name) => this._nopLogger;
  }
}