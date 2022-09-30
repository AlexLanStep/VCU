// Decompiled with JetBrains decompiler
// Type: ETAS.EAS.Util.log4net.EASLogManager
// Assembly: etas.eas.util, Version=1.0.0.0, Culture=neutral, PublicKeyToken=24eada690b1476fd
// MVID: C6C1AF2C-DE3A-40A9-BDCC-7E76498937A9
// Assembly location: E:\LabCar\BasaDll\etas.eas.util.dll

using log4net.Core;
using System;
using System.Reflection;

namespace ETAS.EAS.Util.log4net
{
  public class EASLogManager
  {
    private static readonly WrapperMap s_wrapperMap = new WrapperMap(new log4net.Core.WrapperCreationHandler(EASLogManager.WrapperCreationHandler));

    private EASLogManager()
    {
    }

    public static IEASLog Exists(string name) => EASLogManager.Exists(Assembly.GetCallingAssembly(), name);

    public static IEASLog Exists(string domain, string name) => EASLogManager.WrapLogger(LoggerManager.Exists(domain, name));

    public static IEASLog Exists(Assembly assembly, string name) => EASLogManager.WrapLogger(LoggerManager.Exists(assembly, name));

    public static IEASLog[] GetCurrentLoggers() => EASLogManager.GetCurrentLoggers(Assembly.GetCallingAssembly());

    public static IEASLog[] GetCurrentLoggers(string domain) => EASLogManager.WrapLoggers(LoggerManager.GetCurrentLoggers(domain));

    public static IEASLog[] GetCurrentLoggers(Assembly assembly) => EASLogManager.WrapLoggers(LoggerManager.GetCurrentLoggers(assembly));

    public static IEASLog GetLogger(string name) => EASLogManager.GetLogger(Assembly.GetCallingAssembly(), name);

    public static IEASLog GetLogger(string domain, string name) => EASLogManager.WrapLogger(LoggerManager.GetLogger(domain, name));

    public static IEASLog GetLogger(Assembly assembly, string name) => EASLogManager.WrapLogger(LoggerManager.GetLogger(assembly, name));

    public static IEASLog GetLogger(Type type) => EASLogManager.GetLogger(Assembly.GetCallingAssembly(), type.FullName);

    public static IEASLog GetLogger(string domain, Type type) => EASLogManager.WrapLogger(LoggerManager.GetLogger(domain, type));

    public static IEASLog GetLogger(Assembly assembly, Type type) => EASLogManager.WrapLogger(LoggerManager.GetLogger(assembly, type));

    public static IEASLog WrapLogger(ILogger logger) => (IEASLog)EASLogManager.s_wrapperMap.GetWrapper(logger);

    public static IEASLog[] WrapLoggers(ILogger[] loggers)
    {
      IEASLog[] easLogArray = new IEASLog[loggers.Length];
      for (int index = 0; index < loggers.Length; ++index)
        easLogArray[index] = EASLogManager.WrapLogger(loggers[index]);
      return easLogArray;
    }

    private static ILoggerWrapper WrapperCreationHandler(ILogger logger) => (ILoggerWrapper)new EASLogImpl(logger);
  }
}
