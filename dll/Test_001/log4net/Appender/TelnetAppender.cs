// Decompiled with JetBrains decompiler
// Type: log4net.Appender.TelnetAppender
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using log4net.Core;
using log4net.Util;
using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace log4net.Appender
{
  public class TelnetAppender : AppenderSkeleton
  {
    private TelnetAppender.SocketHandler m_handler;
    private int m_listeningPort = 23;

    public int Port
    {
      get => this.m_listeningPort;
      set => this.m_listeningPort = value >= 0 && value <= (int) ushort.MaxValue ? value : throw SystemInfo.CreateArgumentOutOfRangeException(nameof (value), (object) value, "The value specified for Port is less than " + 0.ToString((IFormatProvider) NumberFormatInfo.InvariantInfo) + " or greater than " + ((int) ushort.MaxValue).ToString((IFormatProvider) NumberFormatInfo.InvariantInfo) + ".");
    }

    protected override void OnClose()
    {
      base.OnClose();
      if (this.m_handler == null)
        return;
      this.m_handler.Dispose();
      this.m_handler = (TelnetAppender.SocketHandler) null;
    }

    protected override bool RequiresLayout => true;

    public override void ActivateOptions()
    {
      base.ActivateOptions();
      try
      {
        LogLog.Debug("TelnetAppender: Creating SocketHandler to listen on port [" + (object) this.m_listeningPort + "]");
        this.m_handler = new TelnetAppender.SocketHandler(this.m_listeningPort);
      }
      catch (Exception ex)
      {
        LogLog.Error("TelnetAppender: Failed to create SocketHandler", ex);
        throw;
      }
    }

    protected override void Append(LoggingEvent loggingEvent)
    {
      if (this.m_handler == null || !this.m_handler.HasConnections)
        return;
      this.m_handler.Send(this.RenderLoggingEvent(loggingEvent));
    }

    protected class SocketHandler : IDisposable
    {
      private const int MAX_CONNECTIONS = 20;
      private Socket m_serverSocket;
      private ArrayList m_clients = new ArrayList();

      public SocketHandler(int port)
      {
        this.m_serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        this.m_serverSocket.Bind((EndPoint) new IPEndPoint(IPAddress.Any, port));
        this.m_serverSocket.Listen(5);
        this.m_serverSocket.BeginAccept(new AsyncCallback(this.OnConnect), (object) null);
      }

      public void Send(string message)
      {
        foreach (TelnetAppender.SocketHandler.SocketClient client in this.m_clients)
        {
          try
          {
            client.Send(message);
          }
          catch (Exception ex)
          {
            client.Dispose();
            this.RemoveClient(client);
          }
        }
      }

      private void AddClient(TelnetAppender.SocketHandler.SocketClient client)
      {
        lock (this)
        {
          ArrayList arrayList = (ArrayList) this.m_clients.Clone();
          arrayList.Add((object) client);
          this.m_clients = arrayList;
        }
      }

      private void RemoveClient(TelnetAppender.SocketHandler.SocketClient client)
      {
        lock (this)
        {
          ArrayList arrayList = (ArrayList) this.m_clients.Clone();
          arrayList.Remove((object) client);
          this.m_clients = arrayList;
        }
      }

      public bool HasConnections
      {
        get
        {
          ArrayList clients = this.m_clients;
          return clients != null && clients.Count > 0;
        }
      }

      private void OnConnect(IAsyncResult asyncResult)
      {
        try
        {
          Socket socket = this.m_serverSocket.EndAccept(asyncResult);
          LogLog.Debug("TelnetAppender: Accepting connection from [" + socket.RemoteEndPoint.ToString() + "]");
          TelnetAppender.SocketHandler.SocketClient client = new TelnetAppender.SocketHandler.SocketClient(socket);
          int count = this.m_clients.Count;
          if (count < 20)
          {
            try
            {
              client.Send("TelnetAppender v1.0 (" + (object) (count + 1) + " active connections)\r\n\r\n");
              this.AddClient(client);
            }
            catch
            {
              client.Dispose();
            }
          }
          else
          {
            client.Send("Sorry - Too many connections.\r\n");
            client.Dispose();
          }
        }
        catch
        {
        }
        finally
        {
          if (this.m_serverSocket != null)
            this.m_serverSocket.BeginAccept(new AsyncCallback(this.OnConnect), (object) null);
        }
      }

      public void Dispose()
      {
        foreach (TelnetAppender.SocketHandler.SocketClient client in this.m_clients)
          client.Dispose();
        this.m_clients.Clear();
        Socket serverSocket = this.m_serverSocket;
        this.m_serverSocket = (Socket) null;
        try
        {
          serverSocket.Shutdown(SocketShutdown.Both);
        }
        catch
        {
        }
        try
        {
          serverSocket.Close();
        }
        catch
        {
        }
      }

      protected class SocketClient : IDisposable
      {
        private Socket m_socket;
        private StreamWriter m_writer;

        public SocketClient(Socket socket)
        {
          this.m_socket = socket;
          try
          {
            this.m_writer = new StreamWriter((Stream) new NetworkStream(socket));
          }
          catch
          {
            this.Dispose();
            throw;
          }
        }

        public void Send(string message)
        {
          this.m_writer.Write(message);
          this.m_writer.Flush();
        }

        public void Dispose()
        {
          try
          {
            if (this.m_writer != null)
            {
              this.m_writer.Close();
              this.m_writer = (StreamWriter) null;
            }
          }
          catch
          {
          }
          if (this.m_socket == null)
            return;
          try
          {
            this.m_socket.Shutdown(SocketShutdown.Both);
          }
          catch
          {
          }
          try
          {
            this.m_socket.Close();
          }
          catch
          {
          }
          this.m_socket = (Socket) null;
        }
      }
    }
  }
}
