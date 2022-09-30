// Decompiled with JetBrains decompiler
// Type: log4net.Core.LogException
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using System;
using System.Runtime.Serialization;

namespace log4net.Core
{
  [Serializable]
  public class LogException : ApplicationException
  {
    public LogException()
    {
    }

    public LogException(string message)
      : base(message)
    {
    }

    public LogException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    protected LogException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
