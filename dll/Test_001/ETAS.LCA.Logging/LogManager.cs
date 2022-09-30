// Decompiled with JetBrains decompiler
// Type: ETAS.LCA.Logging.LogManager
// Assembly: ETAS.LCA.Logging, Version=1.0.0.0, Culture=neutral, PublicKeyToken=24eada690b1476fd
// MVID: 3E0EA91C-5947-4488-8BAF-799929BC7052
// Assembly location: E:\LabCar\BasaDll\ETAS.LCA.Logging.dll

using log4net.Config;
using log4net.Core;
using System;
using System.IO;
using System.Reflection;

namespace ETAS.LCA.Logging
{
  public class LogManager
  {
    private static readonly WrapperMap s_wrapperMap = new WrapperMap(new log4net.Core.WrapperCreationHandler(LogManager.WrapperCreationHandler));

    private LogManager()
    {
    }

    public static ILog GetLogger(Type type) => LogManager.WrapLogger(LoggerManager.GetLogger(Assembly.GetCallingAssembly(), type));

    public static void Configure() => XmlConfigurator.Configure();

    public static void Configure(string filename) => XmlConfigurator.Configure(new FileInfo(filename));

    public static void Configure(Stream s) => XmlConfigurator.Configure(s);

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
