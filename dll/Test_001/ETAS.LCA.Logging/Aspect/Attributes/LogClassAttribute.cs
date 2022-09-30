// Decompiled with JetBrains decompiler
// Type: ETAS.LCA.Logging.Aspect.Attributes.LogClassAttribute
// Assembly: ETAS.LCA.Logging, Version=1.0.0.0, Culture=neutral, PublicKeyToken=24eada690b1476fd
// MVID: 3E0EA91C-5947-4488-8BAF-799929BC7052
// Assembly location: E:\LabCar\BasaDll\ETAS.LCA.Logging.dll

using ETAS.LCA.Logging.Aspect.Properties;
using System;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Contexts;

namespace ETAS.LCA.Logging.Aspect.Attributes
{
  [AttributeUsage(AttributeTargets.Class)]
  public class LogClassAttribute : Attribute, IContextAttribute
  {
    private LogMethodVisibility m_visibility;

    public LogClassAttribute(LogMethodVisibility visibility) => this.m_visibility = visibility;

    public void GetPropertiesForNewContext(IConstructionCallMessage msg)
    {
      LoggingProperty loggingProperty = new LoggingProperty(this.m_visibility, msg.ActivationType);
      msg.ContextProperties.Add((object) loggingProperty);
    }

    public bool IsContextOK(Context ctx, IConstructionCallMessage msg) => false;
  }
}
