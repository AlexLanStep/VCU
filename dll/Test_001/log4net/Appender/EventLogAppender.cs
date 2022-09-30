// Decompiled with JetBrains decompiler
// Type: log4net.Appender.EventLogAppender
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using log4net.Core;
using log4net.Layout;
using log4net.Util;
using System;
using System.Diagnostics;
using System.Threading;

namespace log4net.Appender
{
  public class EventLogAppender : AppenderSkeleton
  {
    private string m_logName;
    private string m_applicationName;
    private string m_machineName;
    private LevelMapping m_levelMapping = new LevelMapping();
    private SecurityContext m_securityContext;

    public EventLogAppender()
    {
      this.m_applicationName = Thread.GetDomain().FriendlyName;
      this.m_logName = "Application";
      this.m_machineName = ".";
    }

    [Obsolete("Instead use the default constructor and set the Layout property")]
    public EventLogAppender(ILayout layout)
      : this()
    {
      this.Layout = layout;
    }

    public string LogName
    {
      get => this.m_logName;
      set => this.m_logName = value;
    }

    public string ApplicationName
    {
      get => this.m_applicationName;
      set => this.m_applicationName = value;
    }

    public string MachineName
    {
      get => this.m_machineName;
      set
      {
      }
    }

    public void AddMapping(EventLogAppender.Level2EventLogEntryType mapping) => this.m_levelMapping.Add((LevelMappingEntry) mapping);

    public SecurityContext SecurityContext
    {
      get => this.m_securityContext;
      set => this.m_securityContext = value;
    }

    public override void ActivateOptions()
    {
      base.ActivateOptions();
      if (this.m_securityContext == null)
        this.m_securityContext = SecurityContextProvider.DefaultProvider.CreateSecurityContext((object) this);
      bool flag = false;
      string str1 = (string) null;
      using (this.SecurityContext.Impersonate((object) this))
      {
        flag = EventLog.SourceExists(this.m_applicationName);
        if (flag)
          str1 = EventLog.LogNameFromSourceName(this.m_applicationName, this.m_machineName);
      }
      if (flag && str1 != this.m_logName)
        LogLog.Debug("EventLogAppender: Changing event source [" + this.m_applicationName + "] from log [" + str1 + "] to log [" + this.m_logName + "]");
      else if (!flag)
        LogLog.Debug("EventLogAppender: Creating event source Source [" + this.m_applicationName + "] in log " + this.m_logName + "]");
      string str2 = (string) null;
      using (this.SecurityContext.Impersonate((object) this))
      {
        if (flag && str1 != this.m_logName)
        {
          EventLog.DeleteEventSource(this.m_applicationName, this.m_machineName);
          EventLogAppender.CreateEventSource(this.m_applicationName, this.m_logName, this.m_machineName);
          str2 = EventLog.LogNameFromSourceName(this.m_applicationName, this.m_machineName);
        }
        else if (!flag)
        {
          EventLogAppender.CreateEventSource(this.m_applicationName, this.m_logName, this.m_machineName);
          str2 = EventLog.LogNameFromSourceName(this.m_applicationName, this.m_machineName);
        }
      }
      this.m_levelMapping.ActivateOptions();
      LogLog.Debug("EventLogAppender: Source [" + this.m_applicationName + "] is registered to log [" + str2 + "]");
    }

    private static void CreateEventSource(string source, string logName, string machineName) => EventLog.CreateEventSource(source, logName, machineName);

    protected override void Append(LoggingEvent loggingEvent)
    {
      pattern_1 = 0;
      switch (loggingEvent.LookupProperty("EventID"))
      {
        case string s:
          if (s.Length > 0)
          {
            int val;
            if (SystemInfo.TryParse(s, out val))
            {
              pattern_1 = val;
              break;
            }
            this.ErrorHandler.Error("Unable to parse event ID property [" + s + "].");
            break;
          }
          break;
      }
      try
      {
        string message = this.RenderLoggingEvent(loggingEvent);
        if (message.Length > 32000)
          message = message.Substring(0, 32000);
        EventLogEntryType entryType = this.GetEntryType(loggingEvent.Level);
        using (this.SecurityContext.Impersonate((object) this))
          EventLog.WriteEntry(this.m_applicationName, message, entryType, pattern_1);
      }
      catch (Exception ex)
      {
        this.ErrorHandler.Error("Unable to write to event log [" + this.m_logName + "] using source [" + this.m_applicationName + "]", ex);
      }
    }

    protected override bool RequiresLayout => true;

    protected virtual EventLogEntryType GetEntryType(Level level)
    {
      if (this.m_levelMapping.Lookup(level) is EventLogAppender.Level2EventLogEntryType eventLogEntryType)
        return eventLogEntryType.EventLogEntryType;
      if (level >= Level.Error)
        return EventLogEntryType.Error;
      return level == Level.Warn ? EventLogEntryType.Warning : EventLogEntryType.Information;
    }

    public class Level2EventLogEntryType : LevelMappingEntry
    {
      private EventLogEntryType m_entryType;

      public EventLogEntryType EventLogEntryType
      {
        get => this.m_entryType;
        set => this.m_entryType = value;
      }
    }
  }
}
