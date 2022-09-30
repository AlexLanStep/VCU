// Decompiled with JetBrains decompiler
// Type: ETAS.LCA.Logging.Aspect.Sinks.LoggingServerSink
// Assembly: ETAS.LCA.Logging, Version=1.0.0.0, Culture=neutral, PublicKeyToken=24eada690b1476fd
// MVID: 3E0EA91C-5947-4488-8BAF-799929BC7052
// Assembly location: E:\LabCar\BasaDll\ETAS.LCA.Logging.dll

using ETAS.LCA.Logging.Aspect.Attributes;
using System;
using System.Collections;
using System.Runtime.Remoting.Messaging;

namespace ETAS.LCA.Logging.Aspect.Sinks
{
  public class LoggingServerSink : IMessageSink
  {
    private IMessageSink m_next;
    private LogMethodVisibility m_visibility;
    private ILog m_logger;

    public LoggingServerSink(IMessageSink next, LogMethodVisibility visibility, Type type)
    {
      this.m_next = next;
      this.m_visibility = visibility;
      this.m_logger = LogManager.GetLogger(type);
    }

    public IMessage SyncProcessMessage(IMessage msg)
    {
      DateTime now = DateTime.Now;
      if (msg is IMethodMessage)
      {
        IMethodMessage methodMessage = (IMethodMessage) msg;
        Console.WriteLine("Begin aspect injection");
        IDictionaryEnumerator enumerator = methodMessage.Properties.GetEnumerator();
        while (enumerator.MoveNext())
        {
          string str = enumerator.Key.ToString();
          object obj = enumerator.Value;
          if (str == "__Args")
          {
            object[] objArray = (object[]) obj;
            for (int index = 0; index < objArray.Length; ++index)
              Console.WriteLine("Arg [{0}] = {1}", (object) index, (object) objArray[index].ToString());
          }
          if (str == "__MethodSignature" && obj != null)
          {
            object[] objArray = (object[]) obj;
            for (int index = 0; index < objArray.Length; ++index)
              Console.WriteLine("Param [{0}] = {1}", (object) index, (object) objArray[index].ToString());
          }
          if (str == "__MethodName" && obj != null)
            Console.WriteLine("MethodName: [{0}]", (object) (string) obj);
        }
      }
      IMessage message = this.m_next.SyncProcessMessage(msg);
      if (message is IMethodReturnMessage)
      {
        IMethodReturnMessage methodReturnMessage = (IMethodReturnMessage) message;
        if (methodReturnMessage.Exception != null)
        {
          Console.WriteLine("Exception has occured: {0} {1}", (object) methodReturnMessage.Exception.Message, (object) methodReturnMessage.Exception.StackTrace);
        }
        else
        {
          for (int index = 0; index < methodReturnMessage.ArgCount; ++index)
            Console.WriteLine("Return value {0} is {1}", (object) index, methodReturnMessage.Args[index]);
        }
      }
      if (msg is IMethodMessage)
      {
        Console.WriteLine("Time Taken for Executing Target Method {0} Milliseconds", (object) DateTime.Now.Subtract(now).TotalMilliseconds);
        Console.WriteLine("End aspect injection");
      }
      return message;
    }

    public IMessageSink NextSink => this.m_next;

    public IMessageCtrl AsyncProcessMessage(IMessage msg, IMessageSink replySink) => (IMessageCtrl) null;
  }
}
