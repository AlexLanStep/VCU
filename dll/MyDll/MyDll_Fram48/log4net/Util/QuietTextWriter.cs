// Decompiled with JetBrains decompiler
// Type: log4net.Util.QuietTextWriter
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using log4net.Core;
using System;
using System.IO;

namespace log4net.Util
{
  public class QuietTextWriter : TextWriterAdapter
  {
    private IErrorHandler m_errorHandler;
    private bool m_closed = false;

    public QuietTextWriter(TextWriter writer, IErrorHandler errorHandler)
      : base(writer)
    {
      this.ErrorHandler = errorHandler != null ? errorHandler : throw new ArgumentNullException(nameof (errorHandler));
    }

    public IErrorHandler ErrorHandler
    {
      get => this.m_errorHandler;
      set => this.m_errorHandler = value != null ? value : throw new ArgumentNullException(nameof (value));
    }

    public bool Closed => this.m_closed;

    public override void Write(char value)
    {
      try
      {
        base.Write(value);
      }
      catch (Exception ex)
      {
        this.m_errorHandler.Error("Failed to write [" + (object) value + "].", ex, ErrorCode.WriteFailure);
      }
    }

    public override void Write(char[] buffer, int index, int count)
    {
      try
      {
        base.Write(buffer, index, count);
      }
      catch (Exception ex)
      {
        this.m_errorHandler.Error("Failed to write buffer.", ex, ErrorCode.WriteFailure);
      }
    }

    public override void Write(string value)
    {
      try
      {
        base.Write(value);
      }
      catch (Exception ex)
      {
        this.m_errorHandler.Error("Failed to write [" + value + "].", ex, ErrorCode.WriteFailure);
      }
    }

    public override void Close()
    {
      this.m_closed = true;
      base.Close();
    }
  }
}
