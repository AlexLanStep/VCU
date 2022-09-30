// Decompiled with JetBrains decompiler
// Type: log4net.Config.RepositoryAttribute
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using System;

namespace log4net.Config
{
  [AttributeUsage(AttributeTargets.Assembly)]
  [Serializable]
  public class RepositoryAttribute : Attribute
  {
    private string m_name = (string) null;
    private Type m_repositoryType = (Type) null;

    public RepositoryAttribute()
    {
    }

    public RepositoryAttribute(string name) => this.m_name = name;

    public string Name
    {
      get => this.m_name;
      set => this.m_name = value;
    }

    public Type RepositoryType
    {
      get => this.m_repositoryType;
      set => this.m_repositoryType = value;
    }
  }
}
