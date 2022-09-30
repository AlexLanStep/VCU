// Decompiled with JetBrains decompiler
// Type: log4net.Config.XmlConfigurator
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using log4net.Repository;
using log4net.Util;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Xml;

namespace log4net.Config
{
  public sealed class XmlConfigurator
  {
    private XmlConfigurator()
    {
    }

    public static void Configure() => XmlConfigurator.Configure(LogManager.GetRepository(Assembly.GetCallingAssembly()));

    public static void Configure(ILoggerRepository repository)
    {
      LogLog.Debug("XmlConfigurator: configuring repository [" + repository.Name + "] using .config file section");
      try
      {
        LogLog.Debug("XmlConfigurator: Application config file is [" + SystemInfo.ConfigurationFileLocation + "]");
      }
      catch
      {
        LogLog.Debug("XmlConfigurator: Application config file location unknown");
      }
      try
      {
        if (!(ConfigurationSettings.GetConfig("log4net") is XmlElement config))
          LogLog.Error("XmlConfigurator: Failed to find configuration section 'log4net' in the application's .config file. Check your .config file for the <log4net> and <configSections> elements. The configuration section should look like: <section name=\"log4net\" type=\"log4net.Config.Log4NetConfigurationSectionHandler,log4net\" />");
        else
          XmlConfigurator.ConfigureFromXml(repository, config);
      }
      catch (ConfigurationException ex)
      {
        if (ex.BareMessage.IndexOf("Unrecognized element") >= 0)
          LogLog.Error("XmlConfigurator: Failed to parse config file. Check your .config file is well formed XML.", (Exception) ex);
        else
          LogLog.Error("XmlConfigurator: Failed to parse config file. Is the <configSections> specified as: " + ("<section name=\"log4net\" type=\"log4net.Config.Log4NetConfigurationSectionHandler," + Assembly.GetExecutingAssembly().FullName + "\" />"), (Exception) ex);
      }
    }

    public static void Configure(XmlElement element) => XmlConfigurator.ConfigureFromXml(LogManager.GetRepository(Assembly.GetCallingAssembly()), element);

    public static void Configure(ILoggerRepository repository, XmlElement element)
    {
      LogLog.Debug("XmlConfigurator: configuring repository [" + repository.Name + "] using XML element");
      XmlConfigurator.ConfigureFromXml(repository, element);
    }

    public static void Configure(FileInfo configFile) => XmlConfigurator.Configure(LogManager.GetRepository(Assembly.GetCallingAssembly()), configFile);

    public static void Configure(Uri configUri) => XmlConfigurator.Configure(LogManager.GetRepository(Assembly.GetCallingAssembly()), configUri);

    public static void Configure(Stream configStream) => XmlConfigurator.Configure(LogManager.GetRepository(Assembly.GetCallingAssembly()), configStream);

    public static void Configure(ILoggerRepository repository, FileInfo configFile)
    {
      LogLog.Debug("XmlConfigurator: configuring repository [" + repository.Name + "] using file [" + (object) configFile + "]");
      if (configFile == null)
        LogLog.Error("XmlConfigurator: Configure called with null 'configFile' parameter");
      else if (System.IO.File.Exists(configFile.FullName))
      {
        FileStream configStream = (FileStream) null;
        int num = 5;
        while (--num >= 0)
        {
          try
          {
            configStream = configFile.Open(FileMode.Open, FileAccess.Read, FileShare.Read);
            break;
          }
          catch (IOException ex)
          {
            if (num == 0)
            {
              LogLog.Error("XmlConfigurator: Failed to open XML config file [" + configFile.Name + "]", (Exception) ex);
              configStream = (FileStream) null;
            }
            Thread.Sleep(250);
          }
        }
        if (configStream == null)
          return;
        try
        {
          XmlConfigurator.Configure(repository, (Stream) configStream);
        }
        finally
        {
          configStream.Close();
        }
      }
      else
        LogLog.Debug("XmlConfigurator: config file [" + configFile.FullName + "] not found. Configuration unchanged.");
    }

    public static void Configure(ILoggerRepository repository, Uri configUri)
    {
      LogLog.Debug("XmlConfigurator: configuring repository [" + repository.Name + "] using URI [" + (object) configUri + "]");
      if ((object) configUri == null)
        LogLog.Error("XmlConfigurator: Configure called with null 'configUri' parameter");
      else if (configUri.IsFile)
      {
        XmlConfigurator.Configure(repository, new FileInfo(configUri.LocalPath));
      }
      else
      {
        WebRequest webRequest = (WebRequest) null;
        try
        {
          webRequest = WebRequest.Create(configUri);
        }
        catch (Exception ex)
        {
          LogLog.Error("XmlConfigurator: Failed to create WebRequest for URI [" + (object) configUri + "]", ex);
        }
        if (webRequest == null)
          return;
        try
        {
          webRequest.Credentials = CredentialCache.DefaultCredentials;
        }
        catch
        {
        }
        try
        {
          WebResponse response = webRequest.GetResponse();
          if (response == null)
            return;
          try
          {
            using (Stream responseStream = response.GetResponseStream())
              XmlConfigurator.Configure(repository, responseStream);
          }
          finally
          {
            response.Close();
          }
        }
        catch (Exception ex)
        {
          LogLog.Error("XmlConfigurator: Failed to request config from URI [" + (object) configUri + "]", ex);
        }
      }
    }

    public static void Configure(ILoggerRepository repository, Stream configStream)
    {
      LogLog.Debug("XmlConfigurator: configuring repository [" + repository.Name + "] using stream");
      if (configStream == null)
      {
        LogLog.Error("XmlConfigurator: Configure called with null 'configStream' parameter");
      }
      else
      {
        XmlDocument xmlDocument = new XmlDocument();
        try
        {
          xmlDocument.Load((XmlReader) new XmlValidatingReader((XmlReader) new XmlTextReader(configStream))
          {
            ValidationType = ValidationType.None,
            EntityHandling = EntityHandling.ExpandEntities
          });
        }
        catch (Exception ex)
        {
          LogLog.Error("XmlConfigurator: Error while loading XML configuration", ex);
          xmlDocument = (XmlDocument) null;
        }
        if (xmlDocument == null)
          return;
        LogLog.Debug("XmlConfigurator: loading XML configuration");
        XmlNodeList elementsByTagName = xmlDocument.GetElementsByTagName("log4net");
        if (elementsByTagName.Count == 0)
          LogLog.Debug("XmlConfigurator: XML configuration does not contain a <log4net> element. Configuration Aborted.");
        else if (elementsByTagName.Count > 1)
          LogLog.Error("XmlConfigurator: XML configuration contains [" + (object) elementsByTagName.Count + "] <log4net> elements. Only one is allowed. Configuration Aborted.");
        else
          XmlConfigurator.ConfigureFromXml(repository, elementsByTagName[0] as XmlElement);
      }
    }

    public static void ConfigureAndWatch(FileInfo configFile) => XmlConfigurator.ConfigureAndWatch(LogManager.GetRepository(Assembly.GetCallingAssembly()), configFile);

    public static void ConfigureAndWatch(ILoggerRepository repository, FileInfo configFile)
    {
      LogLog.Debug("XmlConfigurator: configuring repository [" + repository.Name + "] using file [" + (object) configFile + "] watching for file updates");
      if (configFile == null)
      {
        LogLog.Error("XmlConfigurator: ConfigureAndWatch called with null 'configFile' parameter");
      }
      else
      {
        XmlConfigurator.Configure(repository, configFile);
        try
        {
          XmlConfigurator.ConfigureAndWatchHandler.StartWatching(repository, configFile);
        }
        catch (Exception ex)
        {
          LogLog.Error("XmlConfigurator: Failed to initialize configuration file watcher for file [" + configFile.FullName + "]", ex);
        }
      }
    }

    private static void ConfigureFromXml(ILoggerRepository repository, XmlElement element)
    {
      if (element == null)
        LogLog.Error("XmlConfigurator: ConfigureFromXml called with null 'element' parameter");
      else if (repository == null)
      {
        LogLog.Error("XmlConfigurator: ConfigureFromXml called with null 'repository' parameter");
      }
      else
      {
        LogLog.Debug("XmlConfigurator: Configuring Repository [" + repository.Name + "]");
        if (!(repository is IXmlRepositoryConfigurator repositoryConfigurator))
        {
          LogLog.Warn("XmlConfigurator: Repository [" + (object) repository + "] does not support the XmlConfigurator");
        }
        else
        {
          XmlDocument xmlDocument = new XmlDocument();
          XmlElement element1 = (XmlElement) xmlDocument.AppendChild(xmlDocument.ImportNode((XmlNode) element, true));
          repositoryConfigurator.Configure(element1);
        }
      }
    }

    private sealed class ConfigureAndWatchHandler
    {
      private const int TimeoutMillis = 500;
      private FileInfo m_configFile;
      private ILoggerRepository m_repository;
      private System.Threading.Timer m_timer;

      internal static void StartWatching(ILoggerRepository repository, FileInfo configFile)
      {
        XmlConfigurator.ConfigureAndWatchHandler configureAndWatchHandler = new XmlConfigurator.ConfigureAndWatchHandler(repository, configFile);
      }

      private ConfigureAndWatchHandler(ILoggerRepository repository, FileInfo configFile)
      {
        this.m_repository = repository;
        this.m_configFile = configFile;
        FileSystemWatcher fileSystemWatcher = new FileSystemWatcher();
        fileSystemWatcher.Path = this.m_configFile.DirectoryName;
        fileSystemWatcher.Filter = this.m_configFile.Name;
        fileSystemWatcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.CreationTime;
        fileSystemWatcher.Changed += new FileSystemEventHandler(this.ConfigureAndWatchHandler_OnChanged);
        fileSystemWatcher.Created += new FileSystemEventHandler(this.ConfigureAndWatchHandler_OnChanged);
        fileSystemWatcher.Deleted += new FileSystemEventHandler(this.ConfigureAndWatchHandler_OnChanged);
        fileSystemWatcher.Renamed += new RenamedEventHandler(this.ConfigureAndWatchHandler_OnRenamed);
        fileSystemWatcher.EnableRaisingEvents = true;
        this.m_timer = new System.Threading.Timer(new TimerCallback(this.OnWatchedFileChange), (object) null, -1, -1);
      }

      private void ConfigureAndWatchHandler_OnChanged(object source, FileSystemEventArgs e)
      {
        LogLog.Debug("ConfigureAndWatchHandler: " + (object) e.ChangeType + " [" + this.m_configFile.FullName + "]");
        this.m_timer.Change(500, -1);
      }

      private void ConfigureAndWatchHandler_OnRenamed(object source, RenamedEventArgs e)
      {
        LogLog.Debug("ConfigureAndWatchHandler: " + (object) e.ChangeType + " [" + this.m_configFile.FullName + "]");
        this.m_timer.Change(500, -1);
      }

      private void OnWatchedFileChange(object state) => XmlConfigurator.Configure(this.m_repository, this.m_configFile);
    }
  }
}
