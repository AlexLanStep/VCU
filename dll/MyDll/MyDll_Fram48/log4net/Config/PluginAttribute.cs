// Decompiled with JetBrains decompiler
// Type: log4net.Config.PluginAttribute
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using log4net.Core;
using log4net.Plugin;
using log4net.Util;
using System;

namespace log4net.Config
{
  [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
  [Serializable]
  public sealed class PluginAttribute : Attribute, IPluginFactory
  {
    private string m_typeName = (string) null;
    private Type m_type = (Type) null;

    public PluginAttribute(string typeName) => this.m_typeName = typeName;

    public PluginAttribute(Type type) => this.m_type = type;

    public Type Type
    {
      get => this.m_type;
      set => this.m_type = value;
    }

    public string TypeName
    {
      get => this.m_typeName;
      set => this.m_typeName = value;
    }

    public IPlugin CreatePlugin()
    {
      Type type = this.m_type;
      if (this.m_type == null)
        type = SystemInfo.GetTypeFromString(this.m_typeName, true, true);
      return typeof (IPlugin).IsAssignableFrom(type) ? (IPlugin) Activator.CreateInstance(type) : throw new LogException("Plugin type [" + type.FullName + "] does not implement the log4net.IPlugin interface");
    }

    public override string ToString() => this.m_type != null ? "PluginAttribute[Type=" + this.m_type.FullName + "]" : "PluginAttribute[Type=" + this.m_typeName + "]";
  }
}
