
using log4net.Core;
using log4net.Repository;
using System;
using System.Reflection;

namespace log4net
{
  public sealed class LogManager
  {
    private static readonly WrapperMap s_wrapperMap = new WrapperMap(new log4net.Core.WrapperCreationHandler(LogManager.WrapperCreationHandler));

    private LogManager()
    {
    }

    public static ILog Exists(string name) => LogManager.Exists(Assembly.GetCallingAssembly(), name);

    public static ILog Exists(string repository, string name) => LogManager.WrapLogger(LoggerManager.Exists(repository, name));

    public static ILog Exists(Assembly repositoryAssembly, string name) => LogManager.WrapLogger(LoggerManager.Exists(repositoryAssembly, name));

    public static ILog[] GetCurrentLoggers() => LogManager.GetCurrentLoggers(Assembly.GetCallingAssembly());

    public static ILog[] GetCurrentLoggers(string repository) => LogManager.WrapLoggers(LoggerManager.GetCurrentLoggers(repository));

    public static ILog[] GetCurrentLoggers(Assembly repositoryAssembly) => LogManager.WrapLoggers(LoggerManager.GetCurrentLoggers(repositoryAssembly));

    public static ILog GetLogger(string name) => LogManager.GetLogger(Assembly.GetCallingAssembly(), name);

    public static ILog GetLogger(string repository, string name) => LogManager.WrapLogger(LoggerManager.GetLogger(repository, name));

    public static ILog GetLogger(Assembly repositoryAssembly, string name) => LogManager.WrapLogger(LoggerManager.GetLogger(repositoryAssembly, name));

    public static ILog GetLogger(Type type) => LogManager.GetLogger(Assembly.GetCallingAssembly(), type.FullName);

    public static ILog GetLogger(string repository, Type type) => LogManager.WrapLogger(LoggerManager.GetLogger(repository, type));

    public static ILog GetLogger(Assembly repositoryAssembly, Type type) => LogManager.WrapLogger(LoggerManager.GetLogger(repositoryAssembly, type));

    public static void Shutdown() => LoggerManager.Shutdown();

    public static void ShutdownRepository() => LogManager.ShutdownRepository(Assembly.GetCallingAssembly());

    public static void ShutdownRepository(string repository) => LoggerManager.ShutdownRepository(repository);

    public static void ShutdownRepository(Assembly repositoryAssembly) => LoggerManager.ShutdownRepository(repositoryAssembly);

    public static void ResetConfiguration() => LogManager.ResetConfiguration(Assembly.GetCallingAssembly());

    public static void ResetConfiguration(string repository) => LoggerManager.ResetConfiguration(repository);

    public static void ResetConfiguration(Assembly repositoryAssembly) => LoggerManager.ResetConfiguration(repositoryAssembly);

    [Obsolete("Use GetRepository instead of GetLoggerRepository")]
    public static ILoggerRepository GetLoggerRepository() => LogManager.GetRepository(Assembly.GetCallingAssembly());

    [Obsolete("Use GetRepository instead of GetLoggerRepository")]
    public static ILoggerRepository GetLoggerRepository(string repository) => LogManager.GetRepository(repository);

    [Obsolete("Use GetRepository instead of GetLoggerRepository")]
    public static ILoggerRepository GetLoggerRepository(Assembly repositoryAssembly) => LogManager.GetRepository(repositoryAssembly);

    public static ILoggerRepository GetRepository() => LogManager.GetRepository(Assembly.GetCallingAssembly());

    public static ILoggerRepository GetRepository(string repository) => LoggerManager.GetRepository(repository);

    public static ILoggerRepository GetRepository(Assembly repositoryAssembly) => LoggerManager.GetRepository(repositoryAssembly);

    [Obsolete("Use CreateRepository instead of CreateDomain")]
    public static ILoggerRepository CreateDomain(Type repositoryType) => LogManager.CreateRepository(Assembly.GetCallingAssembly(), repositoryType);

    public static ILoggerRepository CreateRepository(Type repositoryType) => LogManager.CreateRepository(Assembly.GetCallingAssembly(), repositoryType);

    [Obsolete("Use CreateRepository instead of CreateDomain")]
    public static ILoggerRepository CreateDomain(string repository) => LoggerManager.CreateRepository(repository);

    public static ILoggerRepository CreateRepository(string repository) => LoggerManager.CreateRepository(repository);

    [Obsolete("Use CreateRepository instead of CreateDomain")]
    public static ILoggerRepository CreateDomain(
      string repository,
      Type repositoryType)
    {
      return LoggerManager.CreateRepository(repository, repositoryType);
    }

    public static ILoggerRepository CreateRepository(
      string repository,
      Type repositoryType)
    {
      return LoggerManager.CreateRepository(repository, repositoryType);
    }

    [Obsolete("Use CreateRepository instead of CreateDomain")]
    public static ILoggerRepository CreateDomain(
      Assembly repositoryAssembly,
      Type repositoryType)
    {
      return LoggerManager.CreateRepository(repositoryAssembly, repositoryType);
    }

    public static ILoggerRepository CreateRepository(
      Assembly repositoryAssembly,
      Type repositoryType)
    {
      return LoggerManager.CreateRepository(repositoryAssembly, repositoryType);
    }

    public static ILoggerRepository[] GetAllRepositories() => LoggerManager.GetAllRepositories();

    private static ILog WrapLogger(ILogger logger) => (ILog) LogManager.s_wrapperMap.GetWrapper(logger);

    private static ILog[] WrapLoggers(ILogger[] loggers)
    {
      ILog[] logArray = new ILog[loggers.Length];
      for (int index = 0; index < loggers.Length; ++index)
        logArray[index] = LogManager.WrapLogger(loggers[index]);
      return logArray;
    }

    private static ILoggerWrapper WrapperCreationHandler(ILogger logger) => (ILoggerWrapper) new LogImpl(logger);
  }
}
