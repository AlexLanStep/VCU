// Decompiled with JetBrains decompiler
// Type: log4net.Repository.Hierarchy.LoggerKey
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

namespace log4net.Repository.Hierarchy
{
  internal sealed class LoggerKey
  {
    private readonly string m_name;
    private readonly int m_hashCache;

    internal LoggerKey(string name)
    {
      this.m_name = string.Intern(name);
      this.m_hashCache = name.GetHashCode();
    }

    public override int GetHashCode() => this.m_hashCache;

    public override bool Equals(object obj)
    {
      if (this == obj)
        return true;
      return obj is LoggerKey loggerKey && (object) this.m_name == (object) loggerKey.m_name;
    }
  }
}
