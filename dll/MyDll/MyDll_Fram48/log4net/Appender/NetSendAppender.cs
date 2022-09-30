// Decompiled with JetBrains decompiler
// Type: log4net.Appender.NetSendAppender
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using log4net.Core;
using log4net.Util;
using System;
using System.Runtime.InteropServices;

namespace log4net.Appender
{
  public class NetSendAppender : AppenderSkeleton
  {
    private string m_server;
    private string m_sender;
    private string m_recipient;
    private SecurityContext m_securityContext;

    public string Sender
    {
      get => this.m_sender;
      set => this.m_sender = value;
    }

    public string Recipient
    {
      get => this.m_recipient;
      set => this.m_recipient = value;
    }

    public string Server
    {
      get => this.m_server;
      set => this.m_server = value;
    }

    public SecurityContext SecurityContext
    {
      get => this.m_securityContext;
      set => this.m_securityContext = value;
    }

    public override void ActivateOptions()
    {
      base.ActivateOptions();
      if (this.Recipient == null)
        throw new ArgumentNullException("Recipient", "The required property 'Recipient' was not specified.");
      if (this.m_securityContext != null)
        return;
      this.m_securityContext = SecurityContextProvider.DefaultProvider.CreateSecurityContext((object) this);
    }

    protected override void Append(LoggingEvent loggingEvent)
    {
      NativeError nativeError = (NativeError) null;
      string buffer = this.RenderLoggingEvent(loggingEvent);
      using (this.m_securityContext.Impersonate((object) this))
      {
        int number = NetSendAppender.NetMessageBufferSend(this.Server, this.Recipient, this.Sender, buffer, buffer.Length * Marshal.SystemDefaultCharSize);
        if (number != 0)
          nativeError = NativeError.GetError(number);
      }
      if (nativeError == null)
        return;
      this.ErrorHandler.Error(nativeError.ToString() + " (Params: Server=" + this.Server + ", Recipient=" + this.Recipient + ", Sender=" + this.Sender + ")");
    }

    protected override bool RequiresLayout => true;

    [DllImport("netapi32.dll", SetLastError = true)]
    protected static extern int NetMessageBufferSend(
      [MarshalAs(UnmanagedType.LPWStr)] string serverName,
      [MarshalAs(UnmanagedType.LPWStr)] string msgName,
      [MarshalAs(UnmanagedType.LPWStr)] string fromName,
      [MarshalAs(UnmanagedType.LPWStr)] string buffer,
      int bufferSize);
  }
}
