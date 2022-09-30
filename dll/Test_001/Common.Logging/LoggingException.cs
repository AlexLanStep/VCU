// Decompiled with JetBrains decompiler
// Type: Common.Logging.LoggingException
// Assembly: Common.Logging, Version=1.1.0.1, Culture=neutral, PublicKeyToken=65e474d141e25e07
// MVID: F190E754-F88D-465E-80CE-5B5FC84116FB
// Assembly location: E:\LabCar\BasaDll\Common.Logging.dll

using System;
using System.Runtime.Serialization;

namespace Common.Logging
{
  [Serializable]
  public class LoggingException : ApplicationException
  {
    public LoggingException()
      : base("Could not configure the Common.Logging framework.")
    {
    }

    public LoggingException(Exception ex)
      : base(ex.Message, ex)
    {
    }

    public LoggingException(string message)
      : base(message)
    {
    }

    public LoggingException(string message, Exception inner)
      : base(message, inner)
    {
    }

    protected LoggingException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
