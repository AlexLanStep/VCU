// Decompiled with JetBrains decompiler
// Type: log4net.Config.AliasRepositoryAttribute
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using System;

namespace log4net.Config
{
  [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
  [Serializable]
  public class AliasRepositoryAttribute : Attribute
  {
    private string m_name = (string) null;

    public AliasRepositoryAttribute(string name) => this.Name = name;

    public string Name
    {
      get => this.m_name;
      set => this.m_name = value;
    }
  }
}
