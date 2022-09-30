// Decompiled with JetBrains decompiler
// Type: log4net.Config.BasicConfigurator
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using log4net.Appender;
using log4net.Layout;
using log4net.Repository;
using log4net.Util;
using System.Reflection;

namespace log4net.Config
{
  public sealed class BasicConfigurator
  {
    private BasicConfigurator()
    {
    }

    public static void Configure() => BasicConfigurator.Configure(LogManager.GetRepository(Assembly.GetCallingAssembly()));

    public static void Configure(IAppender appender) => BasicConfigurator.Configure(LogManager.GetRepository(Assembly.GetCallingAssembly()), appender);

    public static void Configure(ILoggerRepository repository)
    {
      PatternLayout patternLayout = new PatternLayout();
      patternLayout.ConversionPattern = "%timestamp [%thread] %level %logger %ndc - %message%newline";
      patternLayout.ActivateOptions();
      ConsoleAppender consoleAppender = new ConsoleAppender();
      consoleAppender.Layout = (ILayout) patternLayout;
      consoleAppender.ActivateOptions();
      BasicConfigurator.Configure(repository, (IAppender) consoleAppender);
    }

    public static void Configure(ILoggerRepository repository, IAppender appender)
    {
      if (repository is IBasicRepositoryConfigurator repositoryConfigurator)
        repositoryConfigurator.Configure(appender);
      else
        LogLog.Warn("BasicConfigurator: Repository [" + (object) repository + "] does not support the BasicConfigurator");
    }
  }
}
