// Decompiled with JetBrains decompiler
// Type: ETAS.LCA.Logging.Aspect.Properties.LoggingProperty
// Assembly: ETAS.LCA.Logging, Version=1.0.0.0, Culture=neutral, PublicKeyToken=24eada690b1476fd
// MVID: 3E0EA91C-5947-4488-8BAF-799929BC7052
// Assembly location: E:\LabCar\BasaDll\ETAS.LCA.Logging.dll

using ETAS.LCA.Logging.Aspect.Attributes;
using ETAS.LCA.Logging.Aspect.Sinks;
using System;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;

namespace ETAS.LCA.Logging.Aspect.Properties
{
  public class LoggingProperty : IContextProperty, IContributeServerContextSink
  {
    private LogMethodVisibility m_visibility;
    private Type m_type;

    public LoggingProperty(LogMethodVisibility visibility, Type type)
    {
      this.m_visibility = visibility;
      this.m_type = type;
    }

    public string Name => nameof (LoggingProperty);

    public bool IsNewContextOK(Context newCtx) => true;

    public void Freeze(Context newContext)
    {
    }

    public IMessageSink GetServerContextSink(IMessageSink nextSink) => (IMessageSink) new LoggingServerSink(nextSink, this.m_visibility, this.m_type);
  }
}
