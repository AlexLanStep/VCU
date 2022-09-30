// Decompiled with JetBrains decompiler
// Type: log4net.Appender.SmtpPickupDirAppender
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using log4net.Core;
using log4net.Util;
using System;
using System.IO;

namespace log4net.Appender
{
  public class SmtpPickupDirAppender : BufferingAppenderSkeleton
  {
    private string m_to;
    private string m_from;
    private string m_subject;
    private string m_pickupDir;
    private SecurityContext m_securityContext;

    public string To
    {
      get => this.m_to;
      set => this.m_to = value;
    }

    public string From
    {
      get => this.m_from;
      set => this.m_from = value;
    }

    public string Subject
    {
      get => this.m_subject;
      set => this.m_subject = value;
    }

    public string PickupDir
    {
      get => this.m_pickupDir;
      set => this.m_pickupDir = value;
    }

    public SecurityContext SecurityContext
    {
      get => this.m_securityContext;
      set => this.m_securityContext = value;
    }

    protected override void SendBuffer(LoggingEvent[] events)
    {
      try
      {
        string path = (string) null;
        StreamWriter writer = (StreamWriter) null;
        using (this.SecurityContext.Impersonate((object) this))
        {
          path = Path.Combine(this.m_pickupDir, SystemInfo.NewGuid().ToString("N"));
          writer = File.CreateText(path);
        }
        if (writer == null)
        {
          this.ErrorHandler.Error("Failed to create output file for writing [" + path + "]", (Exception) null, ErrorCode.FileOpenFailure);
        }
        else
        {
          using (writer)
          {
            writer.WriteLine("To: " + this.m_to);
            writer.WriteLine("From: " + this.m_from);
            writer.WriteLine("Subject: " + this.m_subject);
            writer.WriteLine("");
            string header = this.Layout.Header;
            if (header != null)
              writer.Write(header);
            for (int index = 0; index < events.Length; ++index)
              this.RenderLoggingEvent((TextWriter) writer, events[index]);
            string footer = this.Layout.Footer;
            if (footer != null)
              writer.Write(footer);
            writer.WriteLine("");
            writer.WriteLine(".");
          }
        }
      }
      catch (Exception ex)
      {
        this.ErrorHandler.Error("Error occurred while sending e-mail notification.", ex);
      }
    }

    public override void ActivateOptions()
    {
      base.ActivateOptions();
      if (this.m_securityContext == null)
        this.m_securityContext = SecurityContextProvider.DefaultProvider.CreateSecurityContext((object) this);
      using (this.SecurityContext.Impersonate((object) this))
        this.m_pickupDir = SmtpPickupDirAppender.ConvertToFullPath(this.m_pickupDir.Trim());
    }

    protected override bool RequiresLayout => true;

    protected static string ConvertToFullPath(string path) => SystemInfo.ConvertToFullPath(path);
  }
}
