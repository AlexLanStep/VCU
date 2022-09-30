// Decompiled with JetBrains decompiler
// Type: Common.Logging.LogManager
// Assembly: Common.Logging, Version=1.1.0.1, Culture=neutral, PublicKeyToken=65e474d141e25e07
// MVID: F190E754-F88D-465E-80CE-5B5FC84116FB
// Assembly location: E:\LabCar\BasaDll\Common.Logging.dll

using Common.Logging.Simple;

namespace Common.Logging;

public sealed class LogManager
{
  private static ILoggerFactoryAdapter _adapter = (ILoggerFactoryAdapter)null;
  private static object _loadLock = new object();
  private static readonly string COMMON_SECTION_LOGGING = "common/logging";

  private LogManager()
  {
  }

  public static ILoggerFactoryAdapter Adapter
  {
    get
    {
      if (LogManager._adapter == null)
      {
        lock (LogManager._loadLock)
        {
          if (LogManager._adapter == null)
            LogManager._adapter = LogManager.BuildLoggerFactoryAdapter();
        }
      }
      return LogManager._adapter;
    }
    set
    {
      lock (LogManager._loadLock)
        LogManager._adapter = value;
    }
  }

  public static ILog GetLogger(Type type) => LogManager.Adapter.GetLogger(type);

  public static ILog GetLogger(string name) => LogManager.Adapter.GetLogger(name);

  private static ILoggerFactoryAdapter BuildLoggerFactoryAdapter()
  {
    LogSetting section;
    try
    {
      section = (LogSetting)LogManager.GetSection(LogManager.COMMON_SECTION_LOGGING);
    }
    catch (Exception ex)
    {
      ILoggerFactoryAdapter loggerFactoryAdapter = LogManager.BuildDefaultLoggerFactoryAdapter();
      loggerFactoryAdapter.GetLogger(typeof(LogManager)).Warn((object)"Unable to read configuration. Using default logger.", ex);
      return loggerFactoryAdapter;
    }
    if (section != null && !typeof(ILoggerFactoryAdapter).IsAssignableFrom(section.FactoryAdapterType))
    {
      ILoggerFactoryAdapter loggerFactoryAdapter = LogManager.BuildDefaultLoggerFactoryAdapter();
      loggerFactoryAdapter.GetLogger(typeof(LogManager)).Warn((object)("Type " + section.FactoryAdapterType.FullName + " does not implement ILogFactory. Using default logger"));
      return loggerFactoryAdapter;
    }
    if (section != null)
    {
      ILoggerFactoryAdapter instance;
      if (section.Properties.Count > 0)
      {
        try
        {
          object[] objArray = new object[1]
          {
            (object) section.Properties
          };
          instance = (ILoggerFactoryAdapter)Activator.CreateInstance(section.FactoryAdapterType, objArray);
        }
        catch (Exception ex)
        {
          ILoggerFactoryAdapter loggerFactoryAdapter = LogManager.BuildDefaultLoggerFactoryAdapter();
          loggerFactoryAdapter.GetLogger(typeof(LogManager)).Warn((object)("Unable to create instance of type " + section.FactoryAdapterType.FullName + ". Using default logger."), ex);
          return loggerFactoryAdapter;
        }
      }
      else
      {
        try
        {
          instance = (ILoggerFactoryAdapter)Activator.CreateInstance(section.FactoryAdapterType);
        }
        catch (Exception ex)
        {
          ILoggerFactoryAdapter loggerFactoryAdapter = LogManager.BuildDefaultLoggerFactoryAdapter();
          loggerFactoryAdapter.GetLogger(typeof(LogManager)).Warn((object)("Unable to create instance of type " + section.FactoryAdapterType.FullName + ". Using default logger."), ex);
          return loggerFactoryAdapter;
        }
      }
      return instance;
    }
    ILoggerFactoryAdapter loggerFactoryAdapter1 = LogManager.BuildDefaultLoggerFactoryAdapter();
    loggerFactoryAdapter1.GetLogger(typeof(LogManager)).Warn((object)"Unable to read configuration common/logging. Using default logger (ConsoleLogger).");
    return loggerFactoryAdapter1;
  }

  private static object GetSection(string sectionName) => ConfigurationSettings.GetConfig(sectionName);

  private static ILoggerFactoryAdapter BuildDefaultLoggerFactoryAdapter() => (ILoggerFactoryAdapter)new NoOpLoggerFactoryAdapter();
}
