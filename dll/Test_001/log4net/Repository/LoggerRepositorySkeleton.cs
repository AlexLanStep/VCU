// Decompiled with JetBrains decompiler
// Type: log4net.Repository.LoggerRepositorySkeleton
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using log4net.Appender;
using log4net.Core;
using log4net.ObjectRenderer;
using log4net.Plugin;
using log4net.Util;
using System;

namespace log4net.Repository
{
  public abstract class LoggerRepositorySkeleton : ILoggerRepository
  {
    private string m_name;
    private RendererMap m_rendererMap;
    private PluginMap m_pluginMap;
    private LevelMap m_levelMap;
    private Level m_threshold;
    private bool m_configured;
    private PropertiesDictionary m_properties;

    private event LoggerRepositoryShutdownEventHandler m_shutdownEvent;

    private event LoggerRepositoryConfigurationResetEventHandler m_configurationResetEvent;

    private event LoggerRepositoryConfigurationChangedEventHandler m_configurationChangedEvent;

    protected LoggerRepositorySkeleton()
      : this(new PropertiesDictionary())
    {
    }

    protected LoggerRepositorySkeleton(PropertiesDictionary properties)
    {
      this.m_properties = properties;
      this.m_rendererMap = new RendererMap();
      this.m_pluginMap = new PluginMap((ILoggerRepository) this);
      this.m_levelMap = new LevelMap();
      this.m_configured = false;
      this.AddBuiltinLevels();
      this.m_threshold = Level.All;
    }

    public virtual string Name
    {
      get => this.m_name;
      set => this.m_name = value;
    }

    public virtual Level Threshold
    {
      get => this.m_threshold;
      set
      {
        if (value != (Level) null)
        {
          this.m_threshold = value;
        }
        else
        {
          LogLog.Warn("LoggerRepositorySkeleton: Threshold cannot be set to null. Setting to ALL");
          this.m_threshold = Level.All;
        }
      }
    }

    public virtual RendererMap RendererMap => this.m_rendererMap;

    public virtual PluginMap PluginMap => this.m_pluginMap;

    public virtual LevelMap LevelMap => this.m_levelMap;

    public abstract ILogger Exists(string name);

    public abstract ILogger[] GetCurrentLoggers();

    public abstract ILogger GetLogger(string name);

    public virtual void Shutdown()
    {
      foreach (IPlugin allPlugin in this.PluginMap.AllPlugins)
        allPlugin.Shutdown();
      this.OnShutdown((EventArgs) null);
    }

    public virtual void ResetConfiguration()
    {
      this.m_rendererMap.Clear();
      this.m_levelMap.Clear();
      this.AddBuiltinLevels();
      this.Configured = false;
      this.OnConfigurationReset((EventArgs) null);
    }

    public abstract void Log(LoggingEvent logEvent);

    public virtual bool Configured
    {
      get => this.m_configured;
      set => this.m_configured = value;
    }

    public event LoggerRepositoryShutdownEventHandler ShutdownEvent
    {
      add => this.m_shutdownEvent += value;
      remove => this.m_shutdownEvent -= value;
    }

    public event LoggerRepositoryConfigurationResetEventHandler ConfigurationReset
    {
      add => this.m_configurationResetEvent += value;
      remove => this.m_configurationResetEvent -= value;
    }

    public event LoggerRepositoryConfigurationChangedEventHandler ConfigurationChanged
    {
      add => this.m_configurationChangedEvent += value;
      remove => this.m_configurationChangedEvent -= value;
    }

    public PropertiesDictionary Properties => this.m_properties;

    public abstract IAppender[] GetAppenders();

    private void AddBuiltinLevels()
    {
      this.m_levelMap.Add(Level.Off);
      this.m_levelMap.Add(Level.Emergency);
      this.m_levelMap.Add(Level.Fatal);
      this.m_levelMap.Add(Level.Alert);
      this.m_levelMap.Add(Level.Critical);
      this.m_levelMap.Add(Level.Severe);
      this.m_levelMap.Add(Level.Error);
      this.m_levelMap.Add(Level.Warn);
      this.m_levelMap.Add(Level.Notice);
      this.m_levelMap.Add(Level.Info);
      this.m_levelMap.Add(Level.Debug);
      this.m_levelMap.Add(Level.Fine);
      this.m_levelMap.Add(Level.Trace);
      this.m_levelMap.Add(Level.Finer);
      this.m_levelMap.Add(Level.Verbose);
      this.m_levelMap.Add(Level.Finest);
      this.m_levelMap.Add(Level.All);
    }

    public virtual void AddRenderer(Type typeToRender, IObjectRenderer rendererInstance)
    {
      if (typeToRender == null)
        throw new ArgumentNullException(nameof (typeToRender));
      if (rendererInstance == null)
        throw new ArgumentNullException(nameof (rendererInstance));
      this.m_rendererMap.Put(typeToRender, rendererInstance);
    }

    protected virtual void OnShutdown(EventArgs e)
    {
      if (e == null)
        e = EventArgs.Empty;
      LoggerRepositoryShutdownEventHandler shutdownEvent = this.m_shutdownEvent;
      if (shutdownEvent == null)
        return;
      shutdownEvent((object) this, e);
    }

    protected virtual void OnConfigurationReset(EventArgs e)
    {
      if (e == null)
        e = EventArgs.Empty;
      LoggerRepositoryConfigurationResetEventHandler configurationResetEvent = this.m_configurationResetEvent;
      if (configurationResetEvent == null)
        return;
      configurationResetEvent((object) this, e);
    }

    protected virtual void OnConfigurationChanged(EventArgs e)
    {
      if (e == null)
        e = EventArgs.Empty;
      LoggerRepositoryConfigurationChangedEventHandler configurationChangedEvent = this.m_configurationChangedEvent;
      if (configurationChangedEvent == null)
        return;
      configurationChangedEvent((object) this, EventArgs.Empty);
    }

    public void RaiseConfigurationChanged(EventArgs e) => this.OnConfigurationChanged(e);
  }
}
