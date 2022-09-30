// Decompiled with JetBrains decompiler
// Type: log4net.Plugin.PluginSkeleton
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using log4net.Repository;

namespace log4net.Plugin
{
  public abstract class PluginSkeleton : IPlugin
  {
    private string m_name;
    private ILoggerRepository m_repository;

    protected PluginSkeleton(string name) => this.m_name = name;

    public virtual string Name
    {
      get => this.m_name;
      set => this.m_name = value;
    }

    public virtual void Attach(ILoggerRepository repository) => this.m_repository = repository;

    public virtual void Shutdown()
    {
    }

    protected virtual ILoggerRepository LoggerRepository
    {
      get => this.m_repository;
      set => this.m_repository = value;
    }
  }
}
