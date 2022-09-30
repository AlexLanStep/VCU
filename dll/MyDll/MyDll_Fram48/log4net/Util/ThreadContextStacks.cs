// Decompiled with JetBrains decompiler
// Type: log4net.Util.ThreadContextStacks
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

namespace log4net.Util
{
  public sealed class ThreadContextStacks
  {
    private readonly ContextPropertiesBase m_properties;

    internal ThreadContextStacks(ContextPropertiesBase properties) => this.m_properties = properties;

    public ThreadContextStack this[string key]
    {
      get
      {
        object property = this.m_properties[key];
        if (property == null)
        {
          threadContextStack = new ThreadContextStack();
          this.m_properties[key] = (object) threadContextStack;
        }
        else if (!(property is ThreadContextStack threadContextStack))
        {
          string nullText = SystemInfo.NullText;
          try
          {
            nullText = property.ToString();
          }
          catch
          {
          }
          LogLog.Error("ThreadContextStacks: Request for stack named [" + key + "] failed because a property with the same name exists which is a [" + property.GetType().Name + "] with value [" + nullText + "]");
          threadContextStack = new ThreadContextStack();
        }
        return threadContextStack;
      }
    }
  }
}
