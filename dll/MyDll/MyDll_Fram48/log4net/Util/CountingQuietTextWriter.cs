// Decompiled with JetBrains decompiler
// Type: log4net.Util.CountingQuietTextWriter
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using log4net.Core;
using System;
using System.IO;

namespace log4net.Util
{
  public class CountingQuietTextWriter : QuietTextWriter
  {
    private long m_countBytes;

    public CountingQuietTextWriter(TextWriter writer, IErrorHandler errorHandler)
      : base(writer, errorHandler)
    {
      this.m_countBytes = 0L;
    }

    public override void Write(char value)
    {
      try
      {
        base.Write(value);
        this.m_countBytes += (long) this.Encoding.GetByteCount(new char[1]
        {
          value
        });
      }
      catch (Exception ex)
      {
        this.ErrorHandler.Error("Failed to write [" + (object) value + "].", ex, ErrorCode.WriteFailure);
      }
    }

    public override void Write(char[] buffer, int index, int count)
    {
      if (count <= 0)
        return;
      try
      {
        base.Write(buffer, index, count);
        this.m_countBytes += (long) this.Encoding.GetByteCount(buffer, index, count);
      }
      catch (Exception ex)
      {
        this.ErrorHandler.Error("Failed to write buffer.", ex, ErrorCode.WriteFailure);
      }
    }

    public override void Write(string str)
    {
      if (str == null)
        return;
      if (str.Length <= 0)
        return;
      try
      {
        base.Write(str);
        this.m_countBytes += (long) this.Encoding.GetByteCount(str);
      }
      catch (Exception ex)
      {
        this.ErrorHandler.Error("Failed to write [" + str + "].", ex, ErrorCode.WriteFailure);
      }
    }

    public long Count
    {
      get => this.m_countBytes;
      set => this.m_countBytes = value;
    }
  }
}
