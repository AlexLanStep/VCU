// Decompiled with JetBrains decompiler
// Type: log4net.Core.CompactRepositorySelector
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using log4net.Repository;
using log4net.Util;
using System;
using System.Collections;
using System.Reflection;

namespace log4net.Core
{
  public class CompactRepositorySelector : IRepositorySelector
  {
    private const string DefaultRepositoryName = "log4net-default-repository";
    private readonly Hashtable m_name2repositoryMap = new Hashtable();
    private readonly Type m_defaultRepositoryType;

    private event LoggerRepositoryCreationEventHandler m_loggerRepositoryCreatedEvent;

    public CompactRepositorySelector(Type defaultRepositoryType)
    {
      if (defaultRepositoryType == null)
        throw new ArgumentNullException(nameof (defaultRepositoryType));
      this.m_defaultRepositoryType = typeof (ILoggerRepository).IsAssignableFrom(defaultRepositoryType) ? defaultRepositoryType : throw SystemInfo.CreateArgumentOutOfRangeException(nameof (defaultRepositoryType), (object) defaultRepositoryType, "Parameter: defaultRepositoryType, Value: [" + (object) defaultRepositoryType + "] out of range. Argument must implement the ILoggerRepository interface");
      LogLog.Debug("CompactRepositorySelector: defaultRepositoryType [" + (object) this.m_defaultRepositoryType + "]");
    }

    public ILoggerRepository GetRepository(Assembly assembly) => this.CreateRepository(assembly, this.m_defaultRepositoryType);

    public ILoggerRepository GetRepository(string repositoryName)
    {
      if (repositoryName == null)
        throw new ArgumentNullException(nameof (repositoryName));
      lock (this)
      {
        if (!(this.m_name2repositoryMap[(object) repositoryName] is ILoggerRepository name2repository))
          throw new LogException("Repository [" + repositoryName + "] is NOT defined.");
        return name2repository;
      }
    }

    public ILoggerRepository CreateRepository(
      Assembly assembly,
      Type repositoryType)
    {
      if (repositoryType == null)
        repositoryType = this.m_defaultRepositoryType;
      lock (this)
      {
        if (!(this.m_name2repositoryMap[(object) "log4net-default-repository"] is ILoggerRepository repository))
          repository = this.CreateRepository("log4net-default-repository", repositoryType);
        return repository;
      }
    }

    public ILoggerRepository CreateRepository(
      string repositoryName,
      Type repositoryType)
    {
      if (repositoryName == null)
        throw new ArgumentNullException(nameof (repositoryName));
      if (repositoryType == null)
        repositoryType = this.m_defaultRepositoryType;
      lock (this)
      {
        if (this.m_name2repositoryMap[(object) repositoryName] is ILoggerRepository)
          throw new LogException("Repository [" + repositoryName + "] is already defined. Repositories cannot be redefined.");
        LogLog.Debug("DefaultRepositorySelector: Creating repository [" + repositoryName + "] using type [" + (object) repositoryType + "]");
        ILoggerRepository instance = (ILoggerRepository) Activator.CreateInstance(repositoryType);
        instance.Name = repositoryName;
        this.m_name2repositoryMap[(object) repositoryName] = (object) instance;
        this.OnLoggerRepositoryCreatedEvent(instance);
        return instance;
      }
    }

    public bool ExistsRepository(string repositoryName)
    {
      lock (this)
        return this.m_name2repositoryMap.ContainsKey((object) repositoryName);
    }

    public ILoggerRepository[] GetAllRepositories()
    {
      lock (this)
      {
        ICollection values = this.m_name2repositoryMap.Values;
        ILoggerRepository[] allRepositories = new ILoggerRepository[values.Count];
        values.CopyTo((Array) allRepositories, 0);
        return allRepositories;
      }
    }

    public event LoggerRepositoryCreationEventHandler LoggerRepositoryCreatedEvent
    {
      add => this.m_loggerRepositoryCreatedEvent += value;
      remove => this.m_loggerRepositoryCreatedEvent -= value;
    }

    protected virtual void OnLoggerRepositoryCreatedEvent(ILoggerRepository repository)
    {
      LoggerRepositoryCreationEventHandler repositoryCreatedEvent = this.m_loggerRepositoryCreatedEvent;
      if (repositoryCreatedEvent == null)
        return;
      repositoryCreatedEvent((object) this, new LoggerRepositoryCreationEventArgs(repository));
    }
  }
}
