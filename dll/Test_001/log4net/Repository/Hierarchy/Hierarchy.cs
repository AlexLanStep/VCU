// Decompiled with JetBrains decompiler
// Type: log4net.Repository.Hierarchy.Hierarchy
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using log4net.Appender;
using log4net.Core;
using log4net.Util;
using System;
using System.Collections;
using System.Xml;

namespace log4net.Repository.Hierarchy
{
  public class Hierarchy : 
    LoggerRepositorySkeleton,
    IBasicRepositoryConfigurator,
    IXmlRepositoryConfigurator
  {
    private ILoggerFactory m_defaultFactory;
    private Hashtable m_ht;
    private Logger m_root;
    private bool m_emittedNoAppenderWarning = false;

    public event LoggerCreationEventHandler LoggerCreatedEvent
    {
      add => this.m_loggerCreatedEvent += value;
      remove => this.m_loggerCreatedEvent -= value;
    }

    public Hierarchy()
      : this((ILoggerFactory) new DefaultLoggerFactory())
    {
    }

    public Hierarchy(PropertiesDictionary properties)
      : this(properties, (ILoggerFactory) new DefaultLoggerFactory())
    {
    }

    public Hierarchy(ILoggerFactory loggerFactory)
      : this(new PropertiesDictionary(), loggerFactory)
    {
    }

    public Hierarchy(PropertiesDictionary properties, ILoggerFactory loggerFactory)
      : base(properties)
    {
      this.m_defaultFactory = loggerFactory != null ? loggerFactory : throw new ArgumentNullException(nameof (loggerFactory));
      this.m_ht = Hashtable.Synchronized(new Hashtable());
    }

    public bool EmittedNoAppenderWarning
    {
      get => this.m_emittedNoAppenderWarning;
      set => this.m_emittedNoAppenderWarning = value;
    }

    public Logger Root
    {
      get
      {
        if (this.m_root == null)
        {
          lock (this)
          {
            if (this.m_root == null)
            {
              Logger logger = this.m_defaultFactory.CreateLogger((string) null);
              logger.Hierarchy = this;
              this.m_root = logger;
            }
          }
        }
        return this.m_root;
      }
    }

    public ILoggerFactory LoggerFactory
    {
      get => this.m_defaultFactory;
      set => this.m_defaultFactory = value != null ? value : throw new ArgumentNullException(nameof (value));
    }

    public override ILogger Exists(string name) => name != null ? (ILogger) (this.m_ht[(object) new LoggerKey(name)] as Logger) : throw new ArgumentNullException(nameof (name));

    public override ILogger[] GetCurrentLoggers()
    {
      ArrayList arrayList = new ArrayList(this.m_ht.Count);
      foreach (object obj in (IEnumerable) this.m_ht.Values)
      {
        if (obj is Logger)
          arrayList.Add(obj);
      }
      return (ILogger[]) arrayList.ToArray(typeof (Logger));
    }

    public override ILogger GetLogger(string name) => name != null ? (ILogger) this.GetLogger(name, this.m_defaultFactory) : throw new ArgumentNullException(nameof (name));

    public override void Shutdown()
    {
      LogLog.Debug("Hierarchy: Shutdown called on Hierarchy [" + this.Name + "]");
      this.Root.CloseNestedAppenders();
      lock (this.m_ht)
      {
        ILogger[] currentLoggers = this.GetCurrentLoggers();
        foreach (Logger logger in currentLoggers)
          logger.CloseNestedAppenders();
        this.Root.RemoveAllAppenders();
        foreach (Logger logger in currentLoggers)
          logger.RemoveAllAppenders();
      }
      base.Shutdown();
    }

    public override void ResetConfiguration()
    {
      this.Root.Level = Level.Debug;
      this.Threshold = Level.All;
      lock (this.m_ht)
      {
        this.Shutdown();
        foreach (Logger currentLogger in this.GetCurrentLoggers())
        {
          currentLogger.Level = (Level) null;
          currentLogger.Additivity = true;
        }
      }
      base.ResetConfiguration();
      this.OnConfigurationChanged((EventArgs) null);
    }

    public override void Log(LoggingEvent logEvent)
    {
      if (logEvent == null)
        throw new ArgumentNullException(nameof (logEvent));
      this.GetLogger(logEvent.LoggerName, this.m_defaultFactory).Log(logEvent);
    }

    public override IAppender[] GetAppenders()
    {
      ArrayList appenderList = new ArrayList();
      log4net.Repository.Hierarchy.Hierarchy.CollectAppenders(appenderList, (IAppenderAttachable) this.Root);
      foreach (Logger currentLogger in this.GetCurrentLoggers())
        log4net.Repository.Hierarchy.Hierarchy.CollectAppenders(appenderList, (IAppenderAttachable) currentLogger);
      return (IAppender[]) appenderList.ToArray(typeof (IAppender));
    }

    private static void CollectAppender(ArrayList appenderList, IAppender appender)
    {
      if (appenderList.Contains((object) appender))
        return;
      appenderList.Add((object) appender);
      if (!(appender is IAppenderAttachable container))
        return;
      log4net.Repository.Hierarchy.Hierarchy.CollectAppenders(appenderList, container);
    }

    private static void CollectAppenders(ArrayList appenderList, IAppenderAttachable container)
    {
      foreach (IAppender appender in container.Appenders)
        log4net.Repository.Hierarchy.Hierarchy.CollectAppender(appenderList, appender);
    }

    void IBasicRepositoryConfigurator.Configure(IAppender appender) => this.BasicRepositoryConfigure(appender);

    protected void BasicRepositoryConfigure(IAppender appender)
    {
      this.Root.AddAppender(appender);
      this.Configured = true;
      this.OnConfigurationChanged((EventArgs) null);
    }

    void IXmlRepositoryConfigurator.Configure(XmlElement element) => this.XmlRepositoryConfigure(element);

    protected void XmlRepositoryConfigure(XmlElement element)
    {
      new XmlHierarchyConfigurator(this).Configure(element);
      this.Configured = true;
      this.OnConfigurationChanged((EventArgs) null);
    }

    public bool IsDisabled(Level level)
    {
      if ((object) level == null)
        throw new ArgumentNullException(nameof (level));
      return !this.Configured || this.Threshold > level;
    }

    public void Clear() => this.m_ht.Clear();

    public Logger GetLogger(string name, ILoggerFactory factory)
    {
      if (name == null)
        throw new ArgumentNullException(nameof (name));
      if (factory == null)
        throw new ArgumentNullException(nameof (factory));
      LoggerKey key = new LoggerKey(name);
      lock (this.m_ht)
      {
        switch (this.m_ht[(object) key])
        {
          case null:
            Logger logger1 = factory.CreateLogger(name);
            logger1.Hierarchy = this;
            this.m_ht[(object) key] = (object) logger1;
            this.UpdateParents(logger1);
            this.OnLoggerCreationEvent(logger1);
            return logger1;
          case Logger logger3:
            return logger3;
          case ProvisionNode pn:
            Logger logger2 = factory.CreateLogger(name);
            logger2.Hierarchy = this;
            this.m_ht[(object) key] = (object) logger2;
            this.UpdateChildren(pn, logger2);
            this.UpdateParents(logger2);
            this.OnLoggerCreationEvent(logger2);
            return logger2;
          default:
            return (Logger) null;
        }
      }
    }

    protected virtual void OnLoggerCreationEvent(Logger logger)
    {
      LoggerCreationEventHandler loggerCreatedEvent = this.m_loggerCreatedEvent;
      if (loggerCreatedEvent == null)
        return;
      loggerCreatedEvent((object) this, new LoggerCreationEventArgs(logger));
    }

    private void UpdateParents(Logger log)
    {
      string name = log.Name;
      int length1 = name.Length;
      bool flag = false;
      for (int length2 = name.LastIndexOf('.', length1 - 1); length2 >= 0; length2 = name.LastIndexOf('.', length2 - 1))
      {
        LoggerKey key = new LoggerKey(name.Substring(0, length2));
        object obj = this.m_ht[(object) key];
        switch (obj)
        {
          case null:
            ProvisionNode provisionNode1 = new ProvisionNode(log);
            this.m_ht[(object) key] = (object) provisionNode1;
            break;
          case Logger logger:
            flag = true;
            log.Parent = logger;
            goto label_8;
          case ProvisionNode provisionNode2:
            provisionNode2.Add((object) log);
            break;
          default:
            LogLog.Error("Hierarchy: Unexpected object type [" + (object) obj.GetType() + "] in ht.", (Exception) new LogException());
            break;
        }
      }
label_8:
      if (flag)
        return;
      log.Parent = this.Root;
    }

    private void UpdateChildren(ProvisionNode pn, Logger log)
    {
      for (int index = 0; index < pn.Count; ++index)
      {
        Logger logger = (Logger) pn[index];
        if (!logger.Parent.Name.StartsWith(log.Name))
        {
          log.Parent = logger.Parent;
          logger.Parent = log;
        }
      }
    }

    internal void AddLevel(log4net.Repository.Hierarchy.Hierarchy.LevelEntry levelEntry)
    {
      if (levelEntry == null)
        throw new ArgumentNullException(nameof (levelEntry));
      if (levelEntry.Name == null)
        throw new ArgumentNullException("levelEntry.Name");
      if (levelEntry.Value == -1)
      {
        Level level = this.LevelMap[levelEntry.Name];
        levelEntry.Value = !(level == (Level) null) ? level.Value : throw new InvalidOperationException("Cannot redefine level [" + levelEntry.Name + "] because it is not defined in the LevelMap. To define the level supply the level value.");
      }
      this.LevelMap.Add(levelEntry.Name, levelEntry.Value, levelEntry.DisplayName);
    }

    internal void AddProperty(log4net.Repository.Hierarchy.Hierarchy.PropertyEntry propertyEntry)
    {
      if (propertyEntry == null)
        throw new ArgumentNullException(nameof (propertyEntry));
      this.Properties[propertyEntry.Key] = propertyEntry.Key != null ? propertyEntry.Value : throw new ArgumentNullException("propertyEntry.Key");
    }

    private event LoggerCreationEventHandler m_loggerCreatedEvent;

    internal class LevelEntry
    {
      private int m_levelValue = -1;
      private string m_levelName = (string) null;
      private string m_levelDisplayName = (string) null;

      public int Value
      {
        get => this.m_levelValue;
        set => this.m_levelValue = value;
      }

      public string Name
      {
        get => this.m_levelName;
        set => this.m_levelName = value;
      }

      public string DisplayName
      {
        get => this.m_levelDisplayName;
        set => this.m_levelDisplayName = value;
      }

      public override string ToString() => "LevelEntry(Value=" + (object) this.m_levelValue + ", Name=" + this.m_levelName + ", DisplayName=" + this.m_levelDisplayName + ")";
    }

    internal class PropertyEntry
    {
      private string m_key = (string) null;
      private object m_value = (object) null;

      public string Key
      {
        get => this.m_key;
        set => this.m_key = value;
      }

      public object Value
      {
        get => this.m_value;
        set => this.m_value = value;
      }

      public override string ToString() => "PropertyEntry(Key=" + this.m_key + ", Value=" + this.m_value + ")";
    }
  }
}
