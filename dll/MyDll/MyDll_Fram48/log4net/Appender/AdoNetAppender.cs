// Decompiled with JetBrains decompiler
// Type: log4net.Appender.AdoNetAppender
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using log4net.Core;
using log4net.Util;
using System;
using System.Collections;
using System.Data;
using System.Globalization;
using System.IO;

namespace log4net.Appender
{
  public class AdoNetAppender : BufferingAppenderSkeleton
  {
    protected bool m_usePreparedCommand;
    protected ArrayList m_parameters;
    private SecurityContext m_securityContext;
    private IDbConnection m_dbConnection;
    private IDbCommand m_dbCommand;
    private string m_connectionString;
    private string m_connectionType;
    private string m_commandText;
    private CommandType m_commandType;
    private bool m_useTransactions;
    private bool m_reconnectOnError;

    public AdoNetAppender()
    {
      this.m_connectionType = "System.Data.OleDb.OleDbConnection, System.Data, Version=1.0.3300.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
      this.m_useTransactions = true;
      this.m_commandType = CommandType.Text;
      this.m_parameters = new ArrayList();
      this.m_reconnectOnError = false;
    }

    public string ConnectionString
    {
      get => this.m_connectionString;
      set => this.m_connectionString = value;
    }

    public string ConnectionType
    {
      get => this.m_connectionType;
      set => this.m_connectionType = value;
    }

    public string CommandText
    {
      get => this.m_commandText;
      set => this.m_commandText = value;
    }

    public CommandType CommandType
    {
      get => this.m_commandType;
      set => this.m_commandType = value;
    }

    public bool UseTransactions
    {
      get => this.m_useTransactions;
      set => this.m_useTransactions = value;
    }

    public SecurityContext SecurityContext
    {
      get => this.m_securityContext;
      set => this.m_securityContext = value;
    }

    public bool ReconnectOnError
    {
      get => this.m_reconnectOnError;
      set => this.m_reconnectOnError = value;
    }

    protected IDbConnection Connection
    {
      get => this.m_dbConnection;
      set => this.m_dbConnection = value;
    }

    public override void ActivateOptions()
    {
      base.ActivateOptions();
      this.m_usePreparedCommand = this.m_commandText != null && this.m_commandText.Length > 0;
      if (this.m_securityContext == null)
        this.m_securityContext = SecurityContextProvider.DefaultProvider.CreateSecurityContext((object) this);
      this.InitializeDatabaseConnection();
      this.InitializeDatabaseCommand();
    }

    protected override void OnClose()
    {
      base.OnClose();
      if (this.m_dbCommand != null)
      {
        try
        {
          this.m_dbCommand.Dispose();
        }
        catch (Exception ex)
        {
          LogLog.Warn("AdoNetAppender: Exception while disposing cached command object", ex);
        }
        this.m_dbCommand = (IDbCommand) null;
      }
      if (this.m_dbConnection == null)
        return;
      try
      {
        this.m_dbConnection.Close();
      }
      catch (Exception ex)
      {
        LogLog.Warn("AdoNetAppender: Exception while disposing cached connection object", ex);
      }
      this.m_dbConnection = (IDbConnection) null;
    }

    protected override void SendBuffer(LoggingEvent[] events)
    {
      if (this.m_reconnectOnError && (this.m_dbConnection == null || this.m_dbConnection.State != ConnectionState.Open))
      {
        LogLog.Debug("AdoNetAppender: Attempting to reconnect to database. Current Connection State: " + (this.m_dbConnection == null ? "<null>" : this.m_dbConnection.State.ToString()));
        this.InitializeDatabaseConnection();
        this.InitializeDatabaseCommand();
      }
      if (this.m_dbConnection == null || this.m_dbConnection.State != ConnectionState.Open)
        return;
      if (this.m_useTransactions)
      {
        IDbTransaction dbTran = (IDbTransaction) null;
        try
        {
          dbTran = this.m_dbConnection.BeginTransaction();
          this.SendBuffer(dbTran, events);
          dbTran.Commit();
        }
        catch (Exception ex1)
        {
          if (dbTran != null)
          {
            try
            {
              dbTran.Rollback();
            }
            catch (Exception ex2)
            {
            }
          }
          this.ErrorHandler.Error("Exception while writing to database", ex1);
        }
      }
      else
        this.SendBuffer((IDbTransaction) null, events);
    }

    public void AddParameter(AdoNetAppenderParameter parameter) => this.m_parameters.Add((object) parameter);

    protected virtual void SendBuffer(IDbTransaction dbTran, LoggingEvent[] events)
    {
      if (this.m_usePreparedCommand)
      {
        if (this.m_dbCommand == null)
          return;
        if (dbTran != null)
          this.m_dbCommand.Transaction = dbTran;
        foreach (LoggingEvent loggingEvent in events)
        {
          foreach (AdoNetAppenderParameter parameter in this.m_parameters)
            parameter.FormatValue(this.m_dbCommand, loggingEvent);
          this.m_dbCommand.ExecuteNonQuery();
        }
      }
      else
      {
        using (IDbCommand command = this.m_dbConnection.CreateCommand())
        {
          if (dbTran != null)
            command.Transaction = dbTran;
          foreach (LoggingEvent logEvent in events)
          {
            string logStatement = this.GetLogStatement(logEvent);
            LogLog.Debug("AdoNetAppender: LogStatement [" + logStatement + "]");
            command.CommandText = logStatement;
            command.ExecuteNonQuery();
          }
        }
      }
    }

    protected virtual string GetLogStatement(LoggingEvent logEvent)
    {
      if (this.Layout == null)
      {
        this.ErrorHandler.Error("ADOAppender: No Layout specified.");
        return "";
      }
      StringWriter writer = new StringWriter((IFormatProvider) CultureInfo.InvariantCulture);
      this.Layout.Format((TextWriter) writer, logEvent);
      return writer.ToString();
    }

    private void InitializeDatabaseConnection()
    {
      try
      {
        if (this.m_dbCommand != null)
        {
          try
          {
            this.m_dbCommand.Dispose();
          }
          catch (Exception ex)
          {
            LogLog.Warn("AdoNetAppender: Exception while disposing cached command object", ex);
          }
          this.m_dbCommand = (IDbCommand) null;
        }
        if (this.m_dbConnection != null)
        {
          try
          {
            this.m_dbConnection.Close();
          }
          catch (Exception ex)
          {
            LogLog.Warn("AdoNetAppender: Exception while disposing cached connection object", ex);
          }
          this.m_dbConnection = (IDbConnection) null;
        }
        this.m_dbConnection = (IDbConnection) Activator.CreateInstance(this.ResolveConnectionType());
        this.m_dbConnection.ConnectionString = this.m_connectionString;
        using (this.SecurityContext.Impersonate((object) this))
          this.m_dbConnection.Open();
      }
      catch (Exception ex)
      {
        this.ErrorHandler.Error("Could not open database connection [" + this.m_connectionString + "]", ex);
        this.m_dbConnection = (IDbConnection) null;
      }
    }

    protected virtual Type ResolveConnectionType()
    {
      try
      {
        return SystemInfo.GetTypeFromString(this.m_connectionType, true, false);
      }
      catch (Exception ex)
      {
        this.ErrorHandler.Error("Failed to load connection type [" + this.m_connectionType + "]", ex);
        throw;
      }
    }

    private void InitializeDatabaseCommand()
    {
      if (this.m_dbConnection == null)
        return;
      if (!this.m_usePreparedCommand)
        return;
      try
      {
        if (this.m_dbCommand != null)
        {
          try
          {
            this.m_dbCommand.Dispose();
          }
          catch (Exception ex)
          {
            LogLog.Warn("AdoNetAppender: Exception while disposing cached command object", ex);
          }
          this.m_dbCommand = (IDbCommand) null;
        }
        this.m_dbCommand = this.m_dbConnection.CreateCommand();
        this.m_dbCommand.CommandText = this.m_commandText;
        this.m_dbCommand.CommandType = this.m_commandType;
      }
      catch (Exception ex)
      {
        this.ErrorHandler.Error("Could not create database command [" + this.m_commandText + "]", ex);
        if (this.m_dbCommand != null)
        {
          try
          {
            this.m_dbCommand.Dispose();
          }
          catch
          {
          }
          this.m_dbCommand = (IDbCommand) null;
        }
      }
      if (this.m_dbCommand != null)
      {
        try
        {
          foreach (AdoNetAppenderParameter parameter in this.m_parameters)
          {
            try
            {
              parameter.Prepare(this.m_dbCommand);
            }
            catch (Exception ex)
            {
              this.ErrorHandler.Error("Could not add database command parameter [" + parameter.ParameterName + "]", ex);
              throw;
            }
          }
        }
        catch
        {
          try
          {
            this.m_dbCommand.Dispose();
          }
          catch
          {
          }
          this.m_dbCommand = (IDbCommand) null;
        }
      }
      if (this.m_dbCommand == null)
        return;
      try
      {
        this.m_dbCommand.Prepare();
      }
      catch (Exception ex)
      {
        this.ErrorHandler.Error("Could not prepare database command [" + this.m_commandText + "]", ex);
        try
        {
          this.m_dbCommand.Dispose();
        }
        catch
        {
        }
        this.m_dbCommand = (IDbCommand) null;
      }
    }
  }
}
