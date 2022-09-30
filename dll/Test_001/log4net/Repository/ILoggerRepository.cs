// Decompiled with JetBrains decompiler
// Type: log4net.Repository.ILoggerRepository
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using log4net.Appender;
using log4net.Core;
using log4net.ObjectRenderer;
using log4net.Plugin;
using log4net.Util;

namespace log4net.Repository
{
  public interface ILoggerRepository
  {
    string Name { get; set; }

    RendererMap RendererMap { get; }

    PluginMap PluginMap { get; }

    LevelMap LevelMap { get; }

    Level Threshold { get; set; }

    ILogger Exists(string name);

    ILogger[] GetCurrentLoggers();

    ILogger GetLogger(string name);

    void Shutdown();

    void ResetConfiguration();

    void Log(LoggingEvent logEvent);

    bool Configured { get; set; }

    event LoggerRepositoryShutdownEventHandler ShutdownEvent;

    event LoggerRepositoryConfigurationResetEventHandler ConfigurationReset;

    event LoggerRepositoryConfigurationChangedEventHandler ConfigurationChanged;

    PropertiesDictionary Properties { get; }

    IAppender[] GetAppenders();
  }
}
