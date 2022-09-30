// Decompiled with JetBrains decompiler
// Type: log4net.Util.GlobalContextProperties
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

namespace log4net.Util
{
  public sealed class GlobalContextProperties : ContextPropertiesBase
  {
    private volatile ReadOnlyPropertiesDictionary m_readOnlyProperties = new ReadOnlyPropertiesDictionary();
    private readonly object m_syncRoot = new object();

    internal GlobalContextProperties()
    {
    }

    public override object this[string key]
    {
      get => this.m_readOnlyProperties[key];
      set
      {
        lock (this.m_syncRoot)
        {
          PropertiesDictionary propertiesDictionary = new PropertiesDictionary(this.m_readOnlyProperties);
          propertiesDictionary[key] = value;
          this.m_readOnlyProperties = new ReadOnlyPropertiesDictionary((ReadOnlyPropertiesDictionary) propertiesDictionary);
        }
      }
    }

    public void Remove(string key)
    {
      lock (this.m_syncRoot)
      {
        if (!this.m_readOnlyProperties.Contains(key))
          return;
        PropertiesDictionary propertiesDictionary = new PropertiesDictionary(this.m_readOnlyProperties);
        propertiesDictionary.Remove(key);
        this.m_readOnlyProperties = new ReadOnlyPropertiesDictionary((ReadOnlyPropertiesDictionary) propertiesDictionary);
      }
    }

    public void Clear()
    {
      lock (this.m_syncRoot)
        this.m_readOnlyProperties = new ReadOnlyPropertiesDictionary();
    }

    internal ReadOnlyPropertiesDictionary GetReadOnlyProperties() => this.m_readOnlyProperties;
  }
}
