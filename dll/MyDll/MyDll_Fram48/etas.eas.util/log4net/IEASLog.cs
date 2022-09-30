
using log4net;
using log4net.Core;
using System;
using System.Reflection.Emit;

namespace ETAS.EAS.Util.log4net
{
  public interface IEASLog : ILog, ILoggerWrapper
  {
    void entering(string sourceMethod);

    void entering(string sourceMethod, params object[] arguments);

    void exiting(string sourceMethod);

    void exiting(string sourceMethod, params object[] arguments);

    void throwing(string sourceMethod, Exception t);

    void logp(Level level, string sourceMethod, object msg, params object[] arguments);

    void finest(string sourceMethod, object msg);

    void finest(string sourceMethod, object msg, params object[] arguments);

    void finer(string sourceMethod, object msg);

    void finer(string sourceMethod, object msg, params object[] arguments);

    void fine(string sourceMethod, object msg);

    void fine(string sourceMethod, object msg, params object[] arguments);

    void debug(string sourceMethod, object msg);

    void debug(string sourceMethod, object msg, params object[] arguments);

    void info(string sourceMethod, object msg);

    void info(string sourceMethod, object msg, params object[] arguments);

    void warning(string sourceMethod, object msg);

    void warning(string sourceMethod, object msg, params object[] arguments);

    void severe(string sourceMethod, object msg);

    void severe(string sourceMethod, object msg, params object[] arguments);

    bool isFinestEnabled();

    bool isFinerEnabled();

    bool isFineEnabled();

    bool isSevereEnabled();
  }
}
