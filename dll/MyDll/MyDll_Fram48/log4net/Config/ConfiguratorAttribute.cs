// Decompiled with JetBrains decompiler
// Type: log4net.Config.ConfiguratorAttribute
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using log4net.Repository;
using System;
using System.Reflection;

namespace log4net.Config
{
  [AttributeUsage(AttributeTargets.Assembly)]
  public abstract class ConfiguratorAttribute : Attribute, IComparable
  {
    private int m_priority = 0;

    protected ConfiguratorAttribute(int priority) => this.m_priority = priority;

    public abstract void Configure(Assembly sourceAssembly, ILoggerRepository targetRepository);

    public int CompareTo(object obj)
    {
      if (this == obj)
        return 0;
      int num = -1;
      if (obj is ConfiguratorAttribute configuratorAttribute)
      {
        num = configuratorAttribute.m_priority.CompareTo((object) this.m_priority);
        if (num == 0)
          num = -1;
      }
      return num;
    }
  }
}
