// Decompiled with JetBrains decompiler
// Type: log4net.Appender.UdpAppender
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using log4net.Core;
using log4net.Util;
using System;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace log4net.Appender
{
  public class UdpAppender : AppenderSkeleton
  {
    private IPAddress m_remoteAddress;
    private int m_remotePort;
    private IPEndPoint m_remoteEndPoint;
    private int m_localPort;
    private UdpClient m_client;
    private Encoding m_encoding = Encoding.Default;

    public IPAddress RemoteAddress
    {
      get => this.m_remoteAddress;
      set => this.m_remoteAddress = value;
    }

    public int RemotePort
    {
      get => this.m_remotePort;
      set => this.m_remotePort = value >= 0 && value <= (int) ushort.MaxValue ? value : throw SystemInfo.CreateArgumentOutOfRangeException(nameof (value), (object) value, "The value specified is less than " + 0.ToString((IFormatProvider) NumberFormatInfo.InvariantInfo) + " or greater than " + ((int) ushort.MaxValue).ToString((IFormatProvider) NumberFormatInfo.InvariantInfo) + ".");
    }

    public int LocalPort
    {
      get => this.m_localPort;
      set => this.m_localPort = value == 0 || value >= 0 && value <= (int) ushort.MaxValue ? value : throw SystemInfo.CreateArgumentOutOfRangeException(nameof (value), (object) value, "The value specified is less than " + 0.ToString((IFormatProvider) NumberFormatInfo.InvariantInfo) + " or greater than " + ((int) ushort.MaxValue).ToString((IFormatProvider) NumberFormatInfo.InvariantInfo) + ".");
    }

    public Encoding Encoding
    {
      get => this.m_encoding;
      set => this.m_encoding = value;
    }

    protected UdpClient Client
    {
      get => this.m_client;
      set => this.m_client = value;
    }

    protected IPEndPoint RemoteEndPoint
    {
      get => this.m_remoteEndPoint;
      set => this.m_remoteEndPoint = value;
    }

    public override void ActivateOptions()
    {
      base.ActivateOptions();
      if (this.RemoteAddress == null)
        throw new ArgumentNullException("The required property 'Address' was not specified.");
      if (this.RemotePort < 0 || this.RemotePort > (int) ushort.MaxValue)
      {
        // ISSUE: variable of a boxed type
        __Boxed<int> remotePort = (ValueType) this.RemotePort;
        string[] strArray1 = new string[5]
        {
          "The RemotePort is less than ",
          null,
          null,
          null,
          null
        };
        string[] strArray2 = strArray1;
        int num = 0;
        string str1 = num.ToString((IFormatProvider) NumberFormatInfo.InvariantInfo);
        strArray2[1] = str1;
        strArray1[2] = " or greater than ";
        string[] strArray3 = strArray1;
        num = (int) ushort.MaxValue;
        string str2 = num.ToString((IFormatProvider) NumberFormatInfo.InvariantInfo);
        strArray3[3] = str2;
        strArray1[4] = ".";
        string message = string.Concat(strArray1);
        throw SystemInfo.CreateArgumentOutOfRangeException("this.RemotePort", (object) remotePort, message);
      }
      if (this.LocalPort != 0 && (this.LocalPort < 0 || this.LocalPort > (int) ushort.MaxValue))
      {
        // ISSUE: variable of a boxed type
        __Boxed<int> localPort = (ValueType) this.LocalPort;
        string[] strArray4 = new string[5]
        {
          "The LocalPort is less than ",
          null,
          null,
          null,
          null
        };
        string[] strArray5 = strArray4;
        int num = 0;
        string str3 = num.ToString((IFormatProvider) NumberFormatInfo.InvariantInfo);
        strArray5[1] = str3;
        strArray4[2] = " or greater than ";
        string[] strArray6 = strArray4;
        num = (int) ushort.MaxValue;
        string str4 = num.ToString((IFormatProvider) NumberFormatInfo.InvariantInfo);
        strArray6[3] = str4;
        strArray4[4] = ".";
        string message = string.Concat(strArray4);
        throw SystemInfo.CreateArgumentOutOfRangeException("this.LocalPort", (object) localPort, message);
      }
      this.RemoteEndPoint = new IPEndPoint(this.RemoteAddress, this.RemotePort);
      this.InitializeClientConnection();
    }

    protected override void Append(LoggingEvent loggingEvent)
    {
      try
      {
        byte[] bytes = this.m_encoding.GetBytes(this.RenderLoggingEvent(loggingEvent).ToCharArray());
        this.Client.Send(bytes, bytes.Length, this.RemoteEndPoint);
      }
      catch (Exception ex)
      {
        this.ErrorHandler.Error("Unable to send logging event to remote host " + this.RemoteAddress.ToString() + " on port " + (object) this.RemotePort + ".", ex, ErrorCode.WriteFailure);
      }
    }

    protected override bool RequiresLayout => true;

    protected override void OnClose()
    {
      base.OnClose();
      if (this.Client == null)
        return;
      this.Client.Close();
      this.Client = (UdpClient) null;
    }

    protected virtual void InitializeClientConnection()
    {
      try
      {
        if (this.LocalPort == 0)
          this.Client = new UdpClient();
        else
          this.Client = new UdpClient(this.LocalPort);
      }
      catch (Exception ex)
      {
        this.ErrorHandler.Error("Could not initialize the UdpClient connection on port " + this.LocalPort.ToString((IFormatProvider) NumberFormatInfo.InvariantInfo) + ".", ex, ErrorCode.GenericFailure);
        this.Client = (UdpClient) null;
      }
    }
  }
}
