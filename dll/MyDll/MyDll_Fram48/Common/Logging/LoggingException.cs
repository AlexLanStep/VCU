
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