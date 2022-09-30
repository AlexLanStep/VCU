// Decompiled with JetBrains decompiler
// Type: log4net.Appender.AdoNetAppenderParameter
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using log4net.Core;
using log4net.Layout;
using System;
using System.Data;

namespace log4net.Appender
{
  public class AdoNetAppenderParameter
  {
    private string m_parameterName;
    private DbType m_dbType;
    private bool m_inferType = true;
    private byte m_precision;
    private byte m_scale;
    private int m_size;
    private IRawLayout m_layout;

    public AdoNetAppenderParameter()
    {
      this.m_precision = (byte) 0;
      this.m_scale = (byte) 0;
      this.m_size = 0;
    }

    public string ParameterName
    {
      get => this.m_parameterName;
      set => this.m_parameterName = value;
    }

    public DbType DbType
    {
      get => this.m_dbType;
      set
      {
        this.m_dbType = value;
        this.m_inferType = false;
      }
    }

    public byte Precision
    {
      get => this.m_precision;
      set => this.m_precision = value;
    }

    public byte Scale
    {
      get => this.m_scale;
      set => this.m_scale = value;
    }

    public int Size
    {
      get => this.m_size;
      set => this.m_size = value;
    }

    public IRawLayout Layout
    {
      get => this.m_layout;
      set => this.m_layout = value;
    }

    public virtual void Prepare(IDbCommand command)
    {
      IDbDataParameter parameter = command.CreateParameter();
      parameter.ParameterName = this.m_parameterName;
      if (!this.m_inferType)
        parameter.DbType = this.m_dbType;
      if (this.m_precision != (byte) 0)
        parameter.Precision = this.m_precision;
      if (this.m_scale != (byte) 0)
        parameter.Scale = this.m_scale;
      if (this.m_size != 0)
        parameter.Size = this.m_size;
      command.Parameters.Add((object) parameter);
    }

    public virtual void FormatValue(IDbCommand command, LoggingEvent loggingEvent) => ((IDataParameter) command.Parameters[this.m_parameterName]).Value = this.Layout.Format(loggingEvent) ?? (object) DBNull.Value;
  }
}
