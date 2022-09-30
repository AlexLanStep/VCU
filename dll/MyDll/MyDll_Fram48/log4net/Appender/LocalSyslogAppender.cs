// Decompiled with JetBrains decompiler
// Type: log4net.Appender.LocalSyslogAppender
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using log4net.Core;
using log4net.Util;
using System;
using System.Runtime.InteropServices;

namespace log4net.Appender
{
  public class LocalSyslogAppender : AppenderSkeleton
  {
    private LocalSyslogAppender.SyslogFacility m_facility = LocalSyslogAppender.SyslogFacility.User;
    private string m_identity;
    private IntPtr m_handleToIdentity = IntPtr.Zero;
    private LevelMapping m_levelMapping = new LevelMapping();

    public string Identity
    {
      get => this.m_identity;
      set => this.m_identity = value;
    }

    public LocalSyslogAppender.SyslogFacility Facility
    {
      get => this.m_facility;
      set => this.m_facility = value;
    }

    public void AddMapping(LocalSyslogAppender.LevelSeverity mapping) => this.m_levelMapping.Add((LevelMappingEntry) mapping);

    public override void ActivateOptions()
    {
      base.ActivateOptions();
      this.m_levelMapping.ActivateOptions();
      this.m_handleToIdentity = Marshal.StringToHGlobalAnsi(this.m_identity ?? SystemInfo.ApplicationFriendlyName);
      LocalSyslogAppender.openlog(this.m_handleToIdentity, 1, this.m_facility);
    }

    protected override void Append(LoggingEvent loggingEvent) => LocalSyslogAppender.syslog(LocalSyslogAppender.GeneratePriority(this.m_facility, this.GetSeverity(loggingEvent.Level)), "%s", this.RenderLoggingEvent(loggingEvent));

    protected override void OnClose()
    {
      base.OnClose();
      try
      {
        LocalSyslogAppender.closelog();
      }
      catch (DllNotFoundException ex)
      {
      }
      if (!(this.m_handleToIdentity != IntPtr.Zero))
        return;
      Marshal.FreeHGlobal(this.m_handleToIdentity);
    }

    protected override bool RequiresLayout => true;

    protected virtual LocalSyslogAppender.SyslogSeverity GetSeverity(Level level)
    {
      if (this.m_levelMapping.Lookup(level) is LocalSyslogAppender.LevelSeverity levelSeverity)
        return levelSeverity.Severity;
      if (level >= Level.Alert)
        return LocalSyslogAppender.SyslogSeverity.Alert;
      if (level >= Level.Critical)
        return LocalSyslogAppender.SyslogSeverity.Critical;
      if (level >= Level.Error)
        return LocalSyslogAppender.SyslogSeverity.Error;
      if (level >= Level.Warn)
        return LocalSyslogAppender.SyslogSeverity.Warning;
      if (level >= Level.Notice)
        return LocalSyslogAppender.SyslogSeverity.Notice;
      return level >= Level.Info ? LocalSyslogAppender.SyslogSeverity.Informational : LocalSyslogAppender.SyslogSeverity.Debug;
    }

    private static int GeneratePriority(
      LocalSyslogAppender.SyslogFacility facility,
      LocalSyslogAppender.SyslogSeverity severity)
    {
      return (int) ((int) facility * 8 + severity);
    }

    [DllImport("libc")]
    private static extern void openlog(
      IntPtr ident,
      int option,
      LocalSyslogAppender.SyslogFacility facility);

    [DllImport("libc", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    private static extern void syslog(int priority, string format, string message);

    [DllImport("libc")]
    private static extern void closelog();

    public enum SyslogSeverity
    {
      Emergency,
      Alert,
      Critical,
      Error,
      Warning,
      Notice,
      Informational,
      Debug,
    }

    public enum SyslogFacility
    {
      Kernel,
      User,
      Mail,
      Daemons,
      Authorization,
      Syslog,
      Printer,
      News,
      Uucp,
      Clock,
      Authorization2,
      Ftp,
      Ntp,
      Audit,
      Alert,
      Clock2,
      Local0,
      Local1,
      Local2,
      Local3,
      Local4,
      Local5,
      Local6,
      Local7,
    }

    public class LevelSeverity : LevelMappingEntry
    {
      private LocalSyslogAppender.SyslogSeverity m_severity;

      public LocalSyslogAppender.SyslogSeverity Severity
      {
        get => this.m_severity;
        set => this.m_severity = value;
      }
    }
  }
}
