// Decompiled with JetBrains decompiler
// Type: log4net.Core.LoggerManager
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using log4net.Repository;
using log4net.Util;
using System;
using System.Reflection;
using System.Security;
using System.Text;

namespace log4net.Core
{
  public sealed class LoggerManager
  {
    private static IRepositorySelector s_repositorySelector;

    private LoggerManager()
    {
    }

    static LoggerManager()
    {
      try
      {
        LoggerManager.RegisterAppDomainEvents();
      }
      catch (SecurityException ex)
      {
        LogLog.Debug("LoggerManager: Security Exception (ControlAppDomain LinkDemand) while trying to register Shutdown handler with the AppDomain. LoggerManager.Shutdown() will not be called automatically when the AppDomain exits. It must be called programmatically.");
      }
      LogLog.Debug(LoggerManager.GetVersionInfo());
      string appSetting = SystemInfo.GetAppSetting("log4net.RepositorySelector");
      if (appSetting != null && appSetting.Length > 0)
      {
        Type type = (Type) null;
        try
        {
          type = SystemInfo.GetTypeFromString(appSetting, false, true);
        }
        catch (Exception ex)
        {
          LogLog.Error("LoggerManager: Exception while resolving RepositorySelector Type [" + appSetting + "]", ex);
        }
        if (type != null)
        {
          object obj = (object) null;
          try
          {
            obj = Activator.CreateInstance(type);
          }
          catch (Exception ex)
          {
            LogLog.Error("LoggerManager: Exception while creating RepositorySelector [" + type.FullName + "]", ex);
          }
          if (obj != null && obj is IRepositorySelector)
            LoggerManager.s_repositorySelector = (IRepositorySelector) obj;
          else
            LogLog.Error("LoggerManager: RepositorySelector Type [" + type.FullName + "] is not an IRepositorySelector");
        }
      }
      if (LoggerManager.s_repositorySelector != null)
        return;
      LoggerManager.s_repositorySelector = (IRepositorySelector) new DefaultRepositorySelector(typeof (log4net.Repository.Hierarchy.Hierarchy));
    }

    private static void RegisterAppDomainEvents()
    {
      AppDomain.CurrentDomain.ProcessExit += new EventHandler(LoggerManager.OnProcessExit);
      AppDomain.CurrentDomain.DomainUnload += new EventHandler(LoggerManager.OnDomainUnload);
    }

    [Obsolete("Use GetRepository instead of GetLoggerRepository")]
    public static ILoggerRepository GetLoggerRepository(string repository) => LoggerManager.GetRepository(repository);

    [Obsolete("Use GetRepository instead of GetLoggerRepository")]
    public static ILoggerRepository GetLoggerRepository(Assembly repositoryAssembly) => LoggerManager.GetRepository(repositoryAssembly);

    public static ILoggerRepository GetRepository(string repository) => repository != null ? LoggerManager.RepositorySelector.GetRepository(repository) : throw new ArgumentNullException(nameof (repository));

    public static ILoggerRepository GetRepository(Assembly repositoryAssembly) => repositoryAssembly != null ? LoggerManager.RepositorySelector.GetRepository(repositoryAssembly) : throw new ArgumentNullException(nameof (repositoryAssembly));

    public static ILogger Exists(string repository, string name)
    {
      if (repository == null)
        throw new ArgumentNullException(nameof (repository));
      if (name == null)
        throw new ArgumentNullException(nameof (name));
      return LoggerManager.RepositorySelector.GetRepository(repository).Exists(name);
    }

    public static ILogger Exists(Assembly repositoryAssembly, string name)
    {
      if (repositoryAssembly == null)
        throw new ArgumentNullException(nameof (repositoryAssembly));
      if (name == null)
        throw new ArgumentNullException(nameof (name));
      return LoggerManager.RepositorySelector.GetRepository(repositoryAssembly).Exists(name);
    }

    public static ILogger[] GetCurrentLoggers(string repository) => repository != null ? LoggerManager.RepositorySelector.GetRepository(repository).GetCurrentLoggers() : throw new ArgumentNullException(nameof (repository));

    public static ILogger[] GetCurrentLoggers(Assembly repositoryAssembly) => repositoryAssembly != null ? LoggerManager.RepositorySelector.GetRepository(repositoryAssembly).GetCurrentLoggers() : throw new ArgumentNullException(nameof (repositoryAssembly));

    public static ILogger GetLogger(string repository, string name)
    {
      if (repository == null)
        throw new ArgumentNullException(nameof (repository));
      if (name == null)
        throw new ArgumentNullException(nameof (name));
      return LoggerManager.RepositorySelector.GetRepository(repository).GetLogger(name);
    }

    public static ILogger GetLogger(Assembly repositoryAssembly, string name)
    {
      if (repositoryAssembly == null)
        throw new ArgumentNullException(nameof (repositoryAssembly));
      if (name == null)
        throw new ArgumentNullException(nameof (name));
      return LoggerManager.RepositorySelector.GetRepository(repositoryAssembly).GetLogger(name);
    }

    public static ILogger GetLogger(string repository, Type type)
    {
      if (repository == null)
        throw new ArgumentNullException(nameof (repository));
      if (type == null)
        throw new ArgumentNullException(nameof (type));
      return LoggerManager.RepositorySelector.GetRepository(repository).GetLogger(type.FullName);
    }

    public static ILogger GetLogger(Assembly repositoryAssembly, Type type)
    {
      if (repositoryAssembly == null)
        throw new ArgumentNullException(nameof (repositoryAssembly));
      if (type == null)
        throw new ArgumentNullException(nameof (type));
      return LoggerManager.RepositorySelector.GetRepository(repositoryAssembly).GetLogger(type.FullName);
    }

    public static void Shutdown()
    {
      foreach (ILoggerRepository allRepository in LoggerManager.GetAllRepositories())
        allRepository.Shutdown();
    }

    public static void ShutdownRepository(string repository)
    {
      if (repository == null)
        throw new ArgumentNullException(nameof (repository));
      LoggerManager.RepositorySelector.GetRepository(repository).Shutdown();
    }

    public static void ShutdownRepository(Assembly repositoryAssembly)
    {
      if (repositoryAssembly == null)
        throw new ArgumentNullException(nameof (repositoryAssembly));
      LoggerManager.RepositorySelector.GetRepository(repositoryAssembly).Shutdown();
    }

    public static void ResetConfiguration(string repository)
    {
      if (repository == null)
        throw new ArgumentNullException(nameof (repository));
      LoggerManager.RepositorySelector.GetRepository(repository).ResetConfiguration();
    }

    public static void ResetConfiguration(Assembly repositoryAssembly)
    {
      if (repositoryAssembly == null)
        throw new ArgumentNullException(nameof (repositoryAssembly));
      LoggerManager.RepositorySelector.GetRepository(repositoryAssembly).ResetConfiguration();
    }

    [Obsolete("Use CreateRepository instead of CreateDomain")]
    public static ILoggerRepository CreateDomain(string repository) => LoggerManager.CreateRepository(repository);

    public static ILoggerRepository CreateRepository(string repository) => repository != null ? LoggerManager.RepositorySelector.CreateRepository(repository, (Type) null) : throw new ArgumentNullException(nameof (repository));

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
      if (repository == null)
        throw new ArgumentNullException(nameof (repository));
      if (repositoryType == null)
        throw new ArgumentNullException(nameof (repositoryType));
      return LoggerManager.RepositorySelector.CreateRepository(repository, repositoryType);
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
      if (repositoryAssembly == null)
        throw new ArgumentNullException(nameof (repositoryAssembly));
      if (repositoryType == null)
        throw new ArgumentNullException(nameof (repositoryType));
      return LoggerManager.RepositorySelector.CreateRepository(repositoryAssembly, repositoryType);
    }

    public static ILoggerRepository[] GetAllRepositories() => LoggerManager.RepositorySelector.GetAllRepositories();

    public static IRepositorySelector RepositorySelector
    {
      get => LoggerManager.s_repositorySelector;
      set => LoggerManager.s_repositorySelector = value;
    }

    private static string GetVersionInfo()
    {
      StringBuilder stringBuilder = new StringBuilder();
      Assembly executingAssembly = Assembly.GetExecutingAssembly();
      stringBuilder.Append("log4net assembly [").Append(executingAssembly.FullName).Append("]. ");
      stringBuilder.Append("Loaded from [").Append(SystemInfo.AssemblyLocationInfo(executingAssembly)).Append("]. ");
      stringBuilder.Append("(.NET Runtime [").Append(Environment.Version.ToString()).Append("]");
      stringBuilder.Append(" on ").Append(Environment.OSVersion.ToString());
      stringBuilder.Append(")");
      return stringBuilder.ToString();
    }

    private static void OnDomainUnload(object sender, EventArgs e) => LoggerManager.Shutdown();

    private static void OnProcessExit(object sender, EventArgs e) => LoggerManager.Shutdown();
  }
}
