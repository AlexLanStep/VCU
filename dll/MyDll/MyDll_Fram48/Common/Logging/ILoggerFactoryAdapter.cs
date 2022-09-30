
using System;


namespace Common.Logging
{

  public interface ILoggerFactoryAdapter
  {
    ILog GetLogger(Type type);

    ILog GetLogger(string name);
  }

}