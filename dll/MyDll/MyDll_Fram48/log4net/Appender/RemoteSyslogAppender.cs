// Decompiled with JetBrains decompiler
// Type: log4net.Appender.RemoteSyslogAppender
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using log4net.Core;
using log4net.Layout;
using log4net.Util;
using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;

namespace log4net.Appender
{
  public class RemoteSyslogAppender : UdpAppender
  {
    private const int DefaultSyslogPort = 514;
    private RemoteSyslogAppender.SyslogFacility m_facility = RemoteSyslogAppender.SyslogFacility.User;
    private PatternLayout m_identity;
    private LevelMapping m_levelMapping = new LevelMapping();

    public RemoteSyslogAppender()
    {
      this.RemotePort = 514;
      this.RemoteAddress = IPAddress.Parse("127.0.0.1");
      this.Encoding = Encoding.ASCII;
    }

    public PatternLayout Identity
    {
      get => this.m_identity;
      set => this.m_identity = value;
    }

    public RemoteSyslogAppender.SyslogFacility Facility
    {
      get => this.m_facility;
      set => this.m_facility = value;
    }

    public void AddMapping(RemoteSyslogAppender.LevelSeverity mapping) => this.m_levelMapping.Add((LevelMappingEntry) mapping);

    protected override void Append(LoggingEvent loggingEvent)
    {
      try
      {
        StringWriter writer = new StringWriter((IFormatProvider) CultureInfo.InvariantCulture);
        int priority = RemoteSyslogAppender.GeneratePriority(this.m_facility, this.GetSeverity(loggingEvent.Level));
        writer.Write('<');
        writer.Write(priority);
        writer.Write('>');
        if (this.m_identity != null)
          this.m_identity.Format((TextWriter) writer, loggingEvent);
        else
          writer.Write(loggingEvent.Domain);
        writer.Write(": ");
        this.RenderLoggingEvent((TextWriter) writer, loggingEvent);
        byte[] bytes = this.Encoding.GetBytes(writer.ToString().ToCharArray());
        this.Client.Send(bytes, bytes.Length, this.RemoteEndPoint);
      }
      catch (Exception ex)
      {
        this.ErrorHandler.Error("Unable to send logging event to remote syslog " + this.RemoteAddress.ToString() + " on port " + (object) this.RemotePort + ".", ex, ErrorCode.WriteFailure);
      }
    }

    public override void ActivateOptions()
    {
      base.ActivateOptions();
      this.m_levelMapping.ActivateOptions();
    }

    protected virtual RemoteSyslogAppender.SyslogSeverity GetSeverity(
      Level level)
    {
      if (this.m_levelMapping.Lookup(level) is RemoteSyslogAppender.LevelSeverity levelSeverity)
        return levelSeverity.Severity;
      if (level >= Level.Alert)
        return RemoteSyslogAppender.SyslogSeverity.Alert;
      if (level >= Level.Critical)
        return RemoteSyslogAppender.SyslogSeverity.Critical;
      if (level >= Level.Error)
        return RemoteSyslogAppender.SyslogSeverity.Error;
      if (level >= Level.Warn)
        return RemoteSyslogAppender.SyslogSeverity.Warning;
      if (level >= Level.Notice)
        return RemoteSyslogAppender.SyslogSeverity.Notice;
      return level >= Level.Info ? RemoteSyslogAppender.SyslogSeverity.Informational : RemoteSyslogAppender.SyslogSeverity.Debug;
    }

    public static int GeneratePriority(
      RemoteSyslogAppender.SyslogFacility facility,
      RemoteSyslogAppender.SyslogSeverity severity)
    {
      if (facility < RemoteSyslogAppender.SyslogFacility.Kernel || facility > RemoteSyslogAppender.SyslogFacility.Local7)
        throw new ArgumentException("SyslogFacility out of range", nameof (facility));
      if (severity < RemoteSyslogAppender.SyslogSeverity.Emergency || severity > RemoteSyslogAppender.SyslogSeverity.Debug)
        throw new ArgumentException("SyslogSeverity out of range", nameof (severity));
      return (int) ((int) facility * 8 + severity);
    }

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
      private RemoteSyslogAppender.SyslogSeverity m_severity;

      public RemoteSyslogAppender.SyslogSeverity Severity
      {
        get => this.m_severity;
        set => this.m_severity = value;
      }
    }
  }
}
