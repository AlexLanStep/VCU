// Decompiled with JetBrains decompiler
// Type: log4net.Core.DefaultRepositorySelector
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using log4net.Config;
using log4net.Plugin;
using log4net.Repository;
using log4net.Util;
using System;
using System.Collections;
using System.Reflection;

namespace log4net.Core
{
  public class DefaultRepositorySelector : IRepositorySelector
  {
    private const string DefaultRepositoryName = "log4net-default-repository";
    private readonly Hashtable m_name2repositoryMap = new Hashtable();
    private readonly Hashtable m_assembly2repositoryMap = new Hashtable();
    private readonly Hashtable m_alias2repositoryMap = new Hashtable();
    private readonly Type m_defaultRepositoryType;

    public event LoggerRepositoryCreationEventHandler LoggerRepositoryCreatedEvent
    {
      add => this.m_loggerRepositoryCreatedEvent += value;
      remove => this.m_loggerRepositoryCreatedEvent -= value;
    }

    public DefaultRepositorySelector(Type defaultRepositoryType)
    {
      if (defaultRepositoryType == null)
        throw new ArgumentNullException(nameof (defaultRepositoryType));
      this.m_defaultRepositoryType = typeof (ILoggerRepository).IsAssignableFrom(defaultRepositoryType) ? defaultRepositoryType : throw SystemInfo.CreateArgumentOutOfRangeException(nameof (defaultRepositoryType), (object) defaultRepositoryType, "Parameter: defaultRepositoryType, Value: [" + (object) defaultRepositoryType + "] out of range. Argument must implement the ILoggerRepository interface");
      LogLog.Debug("DefaultRepositorySelector: defaultRepositoryType [" + (object) this.m_defaultRepositoryType + "]");
    }

    public ILoggerRepository GetRepository(Assembly repositoryAssembly) => repositoryAssembly != null ? this.CreateRepository(repositoryAssembly, this.m_defaultRepositoryType) : throw new ArgumentNullException(nameof (repositoryAssembly));

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
      Assembly repositoryAssembly,
      Type repositoryType)
    {
      return this.CreateRepository(repositoryAssembly, repositoryType, "log4net-default-repository", true);
    }

    public ILoggerRepository CreateRepository(
      Assembly repositoryAssembly,
      Type repositoryType,
      string repositoryName,
      bool readAssemblyAttributes)
    {
      if (repositoryAssembly == null)
        throw new ArgumentNullException(nameof (repositoryAssembly));
      if (repositoryType == null)
        repositoryType = this.m_defaultRepositoryType;
      lock (this)
      {
        if (!(this.m_assembly2repositoryMap[(object) repositoryAssembly] is ILoggerRepository repository1))
        {
          LogLog.Debug("DefaultRepositorySelector: Creating repository for assembly [" + (object) repositoryAssembly + "]");
          string repositoryName1 = repositoryName;
          Type repositoryType1 = repositoryType;
          if (readAssemblyAttributes)
            this.GetInfoForAssembly(repositoryAssembly, ref repositoryName1, ref repositoryType1);
          LogLog.Debug("DefaultRepositorySelector: Assembly [" + (object) repositoryAssembly + "] using repository [" + repositoryName1 + "] and repository type [" + (object) repositoryType1 + "]");
          if (!(this.m_name2repositoryMap[(object) repositoryName1] is ILoggerRepository repository1))
          {
            repository1 = this.CreateRepository(repositoryName1, repositoryType1);
            if (readAssemblyAttributes)
            {
              try
              {
                this.LoadAliases(repositoryAssembly, repository1);
                this.LoadPlugins(repositoryAssembly, repository1);
                this.ConfigureRepository(repositoryAssembly, repository1);
              }
              catch (Exception ex)
              {
                LogLog.Error("DefaultRepositorySelector: Failed to configure repository [" + repositoryName1 + "] from assembly attributes.", ex);
              }
            }
          }
          else
          {
            LogLog.Debug("DefaultRepositorySelector: repository [" + repositoryName1 + "] already exists, using repository type [" + repository1.GetType().FullName + "]");
            if (readAssemblyAttributes)
            {
              try
              {
                this.LoadPlugins(repositoryAssembly, repository1);
              }
              catch (Exception ex)
              {
                LogLog.Error("DefaultRepositorySelector: Failed to configure repository [" + repositoryName1 + "] from assembly attributes.", ex);
              }
            }
          }
          this.m_assembly2repositoryMap[(object) repositoryAssembly] = (object) repository1;
        }
        return repository1;
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
        if (this.m_name2repositoryMap[(object) repositoryName] is ILoggerRepository repository)
          throw new LogException("Repository [" + repositoryName + "] is already defined. Repositories cannot be redefined.");
        if (this.m_alias2repositoryMap[(object) repositoryName] is ILoggerRepository alias2repository)
        {
          if (alias2repository.GetType() == repositoryType)
          {
            LogLog.Debug("DefaultRepositorySelector: Aliasing repository [" + repositoryName + "] to existing repository [" + alias2repository.Name + "]");
            repository = alias2repository;
            this.m_name2repositoryMap[(object) repositoryName] = (object) repository;
          }
          else
            LogLog.Error("DefaultRepositorySelector: Failed to alias repository [" + repositoryName + "] to existing repository [" + alias2repository.Name + "]. Requested repository type [" + repositoryType.FullName + "] is not compatible with existing type [" + alias2repository.GetType().FullName + "]");
        }
        if (repository == null)
        {
          LogLog.Debug("DefaultRepositorySelector: Creating repository [" + repositoryName + "] using type [" + (object) repositoryType + "]");
          repository = (ILoggerRepository) Activator.CreateInstance(repositoryType);
          repository.Name = repositoryName;
          this.m_name2repositoryMap[(object) repositoryName] = (object) repository;
          this.OnLoggerRepositoryCreatedEvent(repository);
        }
        return repository;
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

    public void AliasRepository(string repositoryAlias, ILoggerRepository repositoryTarget)
    {
      if (repositoryAlias == null)
        throw new ArgumentNullException(nameof (repositoryAlias));
      if (repositoryTarget == null)
        throw new ArgumentNullException(nameof (repositoryTarget));
      lock (this)
      {
        if (this.m_alias2repositoryMap.Contains((object) repositoryAlias))
        {
          if (repositoryTarget != (ILoggerRepository) this.m_alias2repositoryMap[(object) repositoryAlias])
            throw new InvalidOperationException("Repository [" + repositoryAlias + "] is already aliased to repository [" + ((ILoggerRepository) this.m_alias2repositoryMap[(object) repositoryAlias]).Name + "]. Aliases cannot be redefined.");
        }
        else if (this.m_name2repositoryMap.Contains((object) repositoryAlias))
        {
          if (repositoryTarget != (ILoggerRepository) this.m_name2repositoryMap[(object) repositoryAlias])
            throw new InvalidOperationException("Repository [" + repositoryAlias + "] already exists and cannot be aliased to repository [" + repositoryTarget.Name + "].");
        }
        else
          this.m_alias2repositoryMap[(object) repositoryAlias] = (object) repositoryTarget;
      }
    }

    protected virtual void OnLoggerRepositoryCreatedEvent(ILoggerRepository repository)
    {
      LoggerRepositoryCreationEventHandler repositoryCreatedEvent = this.m_loggerRepositoryCreatedEvent;
      if (repositoryCreatedEvent == null)
        return;
      repositoryCreatedEvent((object) this, new LoggerRepositoryCreationEventArgs(repository));
    }

    private void GetInfoForAssembly(
      Assembly assembly,
      ref string repositoryName,
      ref Type repositoryType)
    {
      if (assembly == null)
        throw new ArgumentNullException(nameof (assembly));
      try
      {
        LogLog.Debug("DefaultRepositorySelector: Assembly [" + assembly.FullName + "] Loaded From [" + SystemInfo.AssemblyLocationInfo(assembly) + "]");
      }
      catch
      {
      }
      try
      {
        object[] customAttributes = (object[]) Attribute.GetCustomAttributes(assembly, typeof (RepositoryAttribute), false);
        if (customAttributes == null || customAttributes.Length == 0)
        {
          LogLog.Debug("DefaultRepositorySelector: Assembly [" + (object) assembly + "] does not have a RepositoryAttribute specified.");
        }
        else
        {
          if (customAttributes.Length > 1)
            LogLog.Error("DefaultRepositorySelector: Assembly [" + (object) assembly + "] has multiple log4net.Config.RepositoryAttribute assembly attributes. Only using first occurrence.");
          if (!(customAttributes[0] is RepositoryAttribute repositoryAttribute))
          {
            LogLog.Error("DefaultRepositorySelector: Assembly [" + (object) assembly + "] has a RepositoryAttribute but it does not!.");
          }
          else
          {
            if (repositoryAttribute.Name != null)
              repositoryName = repositoryAttribute.Name;
            if (repositoryAttribute.RepositoryType == null)
              return;
            if (typeof (ILoggerRepository).IsAssignableFrom(repositoryAttribute.RepositoryType))
              repositoryType = repositoryAttribute.RepositoryType;
            else
              LogLog.Error("DefaultRepositorySelector: Repository Type [" + (object) repositoryAttribute.RepositoryType + "] must implement the ILoggerRepository interface.");
          }
        }
      }
      catch (Exception ex)
      {
        LogLog.Error("DefaultRepositorySelector: Unhandled exception in GetInfoForAssembly", ex);
      }
    }

    private void ConfigureRepository(Assembly assembly, ILoggerRepository repository)
    {
      if (assembly == null)
        throw new ArgumentNullException(nameof (assembly));
      if (repository == null)
        throw new ArgumentNullException(nameof (repository));
      object[] customAttributes = (object[]) Attribute.GetCustomAttributes(assembly, typeof (ConfiguratorAttribute), false);
      if (customAttributes != null && customAttributes.Length > 0)
      {
        Array.Sort((Array) customAttributes);
        foreach (ConfiguratorAttribute configuratorAttribute in customAttributes)
        {
          if (configuratorAttribute != null)
          {
            try
            {
              configuratorAttribute.Configure(assembly, repository);
            }
            catch (Exception ex)
            {
              LogLog.Error("DefaultRepositorySelector: Exception calling [" + configuratorAttribute.GetType().FullName + "] .Configure method.", ex);
            }
          }
        }
      }
      if (!(repository.Name == "log4net-default-repository"))
        return;
      string appSetting = SystemInfo.GetAppSetting("log4net.Config");
      if (appSetting == null || appSetting.Length <= 0)
        return;
      string uriString = (string) null;
      try
      {
        uriString = SystemInfo.ApplicationBaseDirectory;
      }
      catch (Exception ex)
      {
        LogLog.Warn("DefaultRepositorySelector: Exception getting ApplicationBaseDirectory. appSettings log4net.Config path [" + appSetting + "] will be treated as an absolute URI", ex);
      }
      Uri configUri = (Uri) null;
      try
      {
        configUri = uriString == null ? new Uri(appSetting) : new Uri(new Uri(uriString), appSetting);
      }
      catch (Exception ex)
      {
        LogLog.Error("DefaultRepositorySelector: Exception while parsing log4net.Config file path [" + appSetting + "]", ex);
      }
      if ((object) configUri == null)
        return;
      LogLog.Debug("DefaultRepositorySelector: Loading configuration for default repository from AppSettings specified Config URI [" + configUri.ToString() + "]");
      try
      {
        XmlConfigurator.Configure(repository, configUri);
      }
      catch (Exception ex)
      {
        LogLog.Error("DefaultRepositorySelector: Exception calling XmlConfigurator.Configure method with ConfigUri [" + (object) configUri + "]", ex);
      }
    }

    private void LoadPlugins(Assembly assembly, ILoggerRepository repository)
    {
      if (assembly == null)
        throw new ArgumentNullException(nameof (assembly));
      if (repository == null)
        throw new ArgumentNullException(nameof (repository));
      object[] customAttributes = (object[]) Attribute.GetCustomAttributes(assembly, typeof (PluginAttribute), false);
      if (customAttributes == null || customAttributes.Length <= 0)
        return;
      foreach (IPluginFactory pluginFactory in customAttributes)
      {
        try
        {
          repository.PluginMap.Add(pluginFactory.CreatePlugin());
        }
        catch (Exception ex)
        {
          LogLog.Error("DefaultRepositorySelector: Failed to create plugin. Attribute [" + pluginFactory.ToString() + "]", ex);
        }
      }
    }

    private void LoadAliases(Assembly assembly, ILoggerRepository repository)
    {
      if (assembly == null)
        throw new ArgumentNullException(nameof (assembly));
      if (repository == null)
        throw new ArgumentNullException(nameof (repository));
      object[] customAttributes = (object[]) Attribute.GetCustomAttributes(assembly, typeof (AliasRepositoryAttribute), false);
      if (customAttributes == null || customAttributes.Length <= 0)
        return;
      foreach (AliasRepositoryAttribute repositoryAttribute in customAttributes)
      {
        try
        {
          this.AliasRepository(repositoryAttribute.Name, repository);
        }
        catch (Exception ex)
        {
          LogLog.Error("DefaultRepositorySelector: Failed to alias repository [" + repositoryAttribute.Name + "]", ex);
        }
      }
    }

    private event LoggerRepositoryCreationEventHandler m_loggerRepositoryCreatedEvent;
  }
}
