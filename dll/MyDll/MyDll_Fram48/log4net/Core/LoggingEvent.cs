// Decompiled with JetBrains decompiler
// Type: log4net.Core.LoggingEvent
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using log4net.Repository;
using log4net.Util;
using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;
using System.Security.Principal;
using System.Threading;

namespace log4net.Core
{
  [Serializable]
  public class LoggingEvent : ISerializable
  {
    public const string HostNameProperty = "log4net:HostName";
    public const string IdentityProperty = "log4net:Identity";
    public const string UserNameProperty = "log4net:UserName";
    private LoggingEventData m_data;
    private CompositeProperties m_compositeProperties;
    private PropertiesDictionary m_eventProperties;
    private readonly Type m_callerStackBoundaryDeclaringType;
    private readonly object m_message;
    private readonly Exception m_thrownException;
    private ILoggerRepository m_repository = (ILoggerRepository) null;
    private FixFlags m_fixFlags = FixFlags.None;
    private bool m_cacheUpdatable = true;

    public LoggingEvent(
      Type callerStackBoundaryDeclaringType,
      ILoggerRepository repository,
      string loggerName,
      Level level,
      object message,
      Exception exception)
    {
      this.m_callerStackBoundaryDeclaringType = callerStackBoundaryDeclaringType;
      this.m_message = message;
      this.m_repository = repository;
      this.m_thrownException = exception;
      this.m_data.LoggerName = loggerName;
      this.m_data.Level = level;
      this.m_data.TimeStamp = DateTime.Now;
    }

    public LoggingEvent(
      Type callerStackBoundaryDeclaringType,
      ILoggerRepository repository,
      LoggingEventData data,
      FixFlags fixedData)
    {
      this.m_callerStackBoundaryDeclaringType = callerStackBoundaryDeclaringType;
      this.m_repository = repository;
      this.m_data = data;
      this.m_fixFlags = fixedData;
    }

    public LoggingEvent(
      Type callerStackBoundaryDeclaringType,
      ILoggerRepository repository,
      LoggingEventData data)
      : this(callerStackBoundaryDeclaringType, repository, data, FixFlags.All)
    {
    }

    public LoggingEvent(LoggingEventData data)
      : this((Type) null, (ILoggerRepository) null, data)
    {
    }

    protected LoggingEvent(SerializationInfo info, StreamingContext context)
    {
      this.m_data.LoggerName = info.GetString(nameof (LoggerName));
      this.m_data.Level = (Level) info.GetValue(nameof (Level), typeof (Level));
      this.m_data.Message = info.GetString("Message");
      this.m_data.ThreadName = info.GetString(nameof (ThreadName));
      this.m_data.TimeStamp = info.GetDateTime(nameof (TimeStamp));
      this.m_data.LocationInfo = (LocationInfo) info.GetValue("LocationInfo", typeof (LocationInfo));
      this.m_data.UserName = info.GetString(nameof (UserName));
      this.m_data.ExceptionString = info.GetString("ExceptionString");
      this.m_data.Properties = (PropertiesDictionary) info.GetValue(nameof (Properties), typeof (PropertiesDictionary));
      this.m_data.Domain = info.GetString(nameof (Domain));
      this.m_data.Identity = info.GetString(nameof (Identity));
      this.m_fixFlags = FixFlags.All;
    }

    public static DateTime StartTime => SystemInfo.ProcessStartTime;

    public Level Level => this.m_data.Level;

    public DateTime TimeStamp => this.m_data.TimeStamp;

    public string LoggerName => this.m_data.LoggerName;

    public LocationInfo LocationInformation
    {
      get
      {
        if (this.m_data.LocationInfo == null && this.m_cacheUpdatable)
          this.m_data.LocationInfo = new LocationInfo(this.m_callerStackBoundaryDeclaringType);
        return this.m_data.LocationInfo;
      }
    }

    public object MessageObject => this.m_message;

    public Exception ExceptionObject => this.m_thrownException;

    public ILoggerRepository Repository => this.m_repository;

    internal void EnsureRepository(ILoggerRepository repository)
    {
      if (repository == null)
        return;
      this.m_repository = repository;
    }

    public string RenderedMessage
    {
      get
      {
        if (this.m_data.Message == null && this.m_cacheUpdatable)
          this.m_data.Message = this.m_message != null ? (!(this.m_message is string) ? (this.m_repository == null ? this.m_message.ToString() : this.m_repository.RendererMap.FindAndRender(this.m_message)) : this.m_message as string) : "";
        return this.m_data.Message;
      }
    }

    public void WriteRenderedMessage(TextWriter writer)
    {
      if (this.m_data.Message != null)
      {
        writer.Write(this.m_data.Message);
      }
      else
      {
        if (this.m_message == null)
          return;
        if (this.m_message is string)
          writer.Write(this.m_message as string);
        else if (this.m_repository != null)
          this.m_repository.RendererMap.FindAndRender(this.m_message, writer);
        else
          writer.Write(this.m_message.ToString());
      }
    }

    public string ThreadName
    {
      get
      {
        if (this.m_data.ThreadName == null && this.m_cacheUpdatable)
        {
          this.m_data.ThreadName = Thread.CurrentThread.Name;
          if (this.m_data.ThreadName != null)
          {
            if (this.m_data.ThreadName.Length != 0)
              goto label_5;
          }
          try
          {
            this.m_data.ThreadName = SystemInfo.CurrentThreadId.ToString((IFormatProvider) NumberFormatInfo.InvariantInfo);
          }
          catch (SecurityException ex)
          {
            LogLog.Debug("LoggingEvent: Security exception while trying to get current thread ID. Error Ignored. Empty thread name.");
            this.m_data.ThreadName = Thread.CurrentThread.GetHashCode().ToString((IFormatProvider) CultureInfo.InvariantCulture);
          }
        }
label_5:
        return this.m_data.ThreadName;
      }
    }

    public string UserName
    {
      get
      {
        if (this.m_data.UserName == null)
        {
          if (this.m_cacheUpdatable)
          {
            try
            {
              WindowsIdentity current = WindowsIdentity.GetCurrent();
              this.m_data.UserName = current == null || current.Name == null ? "" : current.Name;
            }
            catch (SecurityException ex)
            {
              LogLog.Debug("LoggingEvent: Security exception while trying to get current windows identity. Error Ignored. Empty user name.");
              this.m_data.UserName = "";
            }
          }
        }
        return this.m_data.UserName;
      }
    }

    public string Identity
    {
      get
      {
        if (this.m_data.Identity == null)
        {
          if (this.m_cacheUpdatable)
          {
            try
            {
              this.m_data.Identity = Thread.CurrentPrincipal == null || Thread.CurrentPrincipal.Identity == null || Thread.CurrentPrincipal.Identity.Name == null ? "" : Thread.CurrentPrincipal.Identity.Name;
            }
            catch (SecurityException ex)
            {
              LogLog.Debug("LoggingEvent: Security exception while trying to get current thread principal. Error Ignored. Empty identity name.");
              this.m_data.Identity = "";
            }
          }
        }
        return this.m_data.Identity;
      }
    }

    public string Domain
    {
      get
      {
        if (this.m_data.Domain == null && this.m_cacheUpdatable)
          this.m_data.Domain = SystemInfo.ApplicationFriendlyName;
        return this.m_data.Domain;
      }
    }

    public PropertiesDictionary Properties
    {
      get
      {
        if (this.m_data.Properties != null)
          return this.m_data.Properties;
        if (this.m_eventProperties == null)
          this.m_eventProperties = new PropertiesDictionary();
        return this.m_eventProperties;
      }
    }

    public FixFlags Fix
    {
      get => this.m_fixFlags;
      set => this.FixVolatileData(value);
    }

    [PermissionSet(SecurityAction.Demand, XML = "<PermissionSet class=\"System.Security.PermissionSet\"\r\n               version=\"1\">\r\n   <IPermission class=\"System.Security.Permissions.SecurityPermission, mscorlib, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"\r\n                version=\"1\"\r\n                Flags=\"SerializationFormatter\"/>\r\n</PermissionSet>\r\n")]
    public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      info.AddValue("LoggerName", (object) this.m_data.LoggerName);
      info.AddValue("Level", (object) this.m_data.Level);
      info.AddValue("Message", (object) this.m_data.Message);
      info.AddValue("ThreadName", (object) this.m_data.ThreadName);
      info.AddValue("TimeStamp", this.m_data.TimeStamp);
      info.AddValue("LocationInfo", (object) this.m_data.LocationInfo);
      info.AddValue("UserName", (object) this.m_data.UserName);
      info.AddValue("ExceptionString", (object) this.m_data.ExceptionString);
      info.AddValue("Properties", (object) this.m_data.Properties);
      info.AddValue("Domain", (object) this.m_data.Domain);
      info.AddValue("Identity", (object) this.m_data.Identity);
    }

    public LoggingEventData GetLoggingEventData() => this.GetLoggingEventData(FixFlags.Partial);

    public LoggingEventData GetLoggingEventData(FixFlags fixFlags)
    {
      this.Fix = fixFlags;
      return this.m_data;
    }

    [Obsolete("Use GetExceptionString instead")]
    public string GetExceptionStrRep() => this.GetExceptionString();

    public string GetExceptionString()
    {
      if (this.m_data.ExceptionString == null && this.m_cacheUpdatable)
        this.m_data.ExceptionString = this.m_thrownException == null ? "" : (this.m_repository == null ? this.m_thrownException.ToString() : this.m_repository.RendererMap.FindAndRender((object) this.m_thrownException));
      return this.m_data.ExceptionString;
    }

    [Obsolete("Use Fix property")]
    public void FixVolatileData() => this.Fix = FixFlags.All;

    [Obsolete("Use Fix property")]
    public void FixVolatileData(bool fastButLoose)
    {
      if (fastButLoose)
        this.Fix = FixFlags.Partial;
      else
        this.Fix = FixFlags.All;
    }

    protected void FixVolatileData(FixFlags flags)
    {
      object obj = (object) null;
      this.m_cacheUpdatable = true;
      FixFlags fixFlags = (flags ^ this.m_fixFlags) & flags;
      if (fixFlags > FixFlags.None)
      {
        if ((fixFlags & FixFlags.Message) != FixFlags.None)
        {
          obj = (object) this.RenderedMessage;
          this.m_fixFlags |= FixFlags.Message;
        }
        if ((fixFlags & FixFlags.ThreadName) != FixFlags.None)
        {
          obj = (object) this.ThreadName;
          this.m_fixFlags |= FixFlags.ThreadName;
        }
        if ((fixFlags & FixFlags.LocationInfo) != FixFlags.None)
        {
          obj = (object) this.LocationInformation;
          this.m_fixFlags |= FixFlags.LocationInfo;
        }
        if ((fixFlags & FixFlags.UserName) != FixFlags.None)
        {
          obj = (object) this.UserName;
          this.m_fixFlags |= FixFlags.UserName;
        }
        if ((fixFlags & FixFlags.Domain) != FixFlags.None)
        {
          obj = (object) this.Domain;
          this.m_fixFlags |= FixFlags.Domain;
        }
        if ((fixFlags & FixFlags.Identity) != FixFlags.None)
        {
          obj = (object) this.Identity;
          this.m_fixFlags |= FixFlags.Identity;
        }
        if ((fixFlags & FixFlags.Exception) != FixFlags.None)
        {
          obj = (object) this.GetExceptionString();
          this.m_fixFlags |= FixFlags.Exception;
        }
        if ((fixFlags & FixFlags.Properties) != FixFlags.None)
        {
          this.CacheProperties();
          this.m_fixFlags |= FixFlags.Properties;
        }
      }
      if (obj == null)
        ;
      this.m_cacheUpdatable = false;
    }

    private void CreateCompositeProperties()
    {
      this.m_compositeProperties = new CompositeProperties();
      if (this.m_eventProperties != null)
        this.m_compositeProperties.Add((ReadOnlyPropertiesDictionary) this.m_eventProperties);
      PropertiesDictionary properties1 = LogicalThreadContext.Properties.GetProperties(false);
      if (properties1 != null)
        this.m_compositeProperties.Add((ReadOnlyPropertiesDictionary) properties1);
      PropertiesDictionary properties2 = ThreadContext.Properties.GetProperties(false);
      if (properties2 != null)
        this.m_compositeProperties.Add((ReadOnlyPropertiesDictionary) properties2);
      this.m_compositeProperties.Add(GlobalContext.Properties.GetReadOnlyProperties());
    }

    private void CacheProperties()
    {
      if (this.m_data.Properties != null || !this.m_cacheUpdatable)
        return;
      if (this.m_compositeProperties == null)
        this.CreateCompositeProperties();
      PropertiesDictionary propertiesDictionary1 = this.m_compositeProperties.Flatten();
      PropertiesDictionary propertiesDictionary2 = new PropertiesDictionary();
      foreach (DictionaryEntry dictionaryEntry in (IEnumerable) propertiesDictionary1)
      {
        if (dictionaryEntry.Key is string key)
        {
          object fixedObject = dictionaryEntry.Value;
          if (fixedObject is IFixingRequired fixingRequired)
            fixedObject = fixingRequired.GetFixedObject();
          if (fixedObject != null)
            propertiesDictionary2[key] = fixedObject;
        }
      }
      this.m_data.Properties = propertiesDictionary2;
    }

    public object LookupProperty(string key)
    {
      if (this.m_data.Properties != null)
        return this.m_data.Properties[key];
      if (this.m_compositeProperties == null)
        this.CreateCompositeProperties();
      return this.m_compositeProperties[key];
    }

    public PropertiesDictionary GetProperties()
    {
      if (this.m_data.Properties != null)
        return this.m_data.Properties;
      if (this.m_compositeProperties == null)
        this.CreateCompositeProperties();
      return this.m_compositeProperties.Flatten();
    }
  }
}
