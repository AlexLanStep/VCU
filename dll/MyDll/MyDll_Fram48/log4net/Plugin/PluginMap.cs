// Decompiled with JetBrains decompiler
// Type: log4net.Plugin.PluginMap
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using log4net.Repository;
using System;
using System.Collections;

namespace log4net.Plugin
{
  public sealed class PluginMap
  {
    private readonly Hashtable m_mapName2Plugin = new Hashtable();
    private readonly ILoggerRepository m_repository;

    public PluginMap(ILoggerRepository repository) => this.m_repository = repository;

    public IPlugin this[string name]
    {
      get
      {
        if (name == null)
          throw new ArgumentNullException(nameof (name));
        lock (this)
          return (IPlugin) this.m_mapName2Plugin[(object) name];
      }
    }

    public PluginCollection AllPlugins
    {
      get
      {
        lock (this)
          return new PluginCollection(this.m_mapName2Plugin.Values);
      }
    }

    public void Add(IPlugin plugin)
    {
      if (plugin == null)
        throw new ArgumentNullException(nameof (plugin));
      IPlugin plugin1 = (IPlugin) null;
      lock (this)
      {
        plugin1 = this.m_mapName2Plugin[(object) plugin.Name] as IPlugin;
        this.m_mapName2Plugin[(object) plugin.Name] = (object) plugin;
      }
      plugin1?.Shutdown();
      plugin.Attach(this.m_repository);
    }

    public void Remove(IPlugin plugin)
    {
      if (plugin == null)
        throw new ArgumentNullException(nameof (plugin));
      lock (this)
        this.m_mapName2Plugin.Remove((object) plugin.Name);
    }
  }
}
