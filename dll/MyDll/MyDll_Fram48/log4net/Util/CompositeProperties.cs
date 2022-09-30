// Decompiled with JetBrains decompiler
// Type: log4net.Util.CompositeProperties
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using System.Collections;

namespace log4net.Util
{
  public sealed class CompositeProperties
  {
    private PropertiesDictionary m_flattened = (PropertiesDictionary) null;
    private ArrayList m_nestedProperties = new ArrayList();

    internal CompositeProperties()
    {
    }

    public object this[string key]
    {
      get
      {
        if (this.m_flattened != null)
          return this.m_flattened[key];
        foreach (ReadOnlyPropertiesDictionary nestedProperty in this.m_nestedProperties)
        {
          if (nestedProperty.Contains(key))
            return nestedProperty[key];
        }
        return (object) null;
      }
    }

    public void Add(ReadOnlyPropertiesDictionary properties)
    {
      this.m_flattened = (PropertiesDictionary) null;
      this.m_nestedProperties.Add((object) properties);
    }

    public PropertiesDictionary Flatten()
    {
      if (this.m_flattened == null)
      {
        this.m_flattened = new PropertiesDictionary();
        int count = this.m_nestedProperties.Count;
        while (--count >= 0)
        {
          foreach (DictionaryEntry dictionaryEntry in (IEnumerable) this.m_nestedProperties[count])
            this.m_flattened[(string) dictionaryEntry.Key] = dictionaryEntry.Value;
        }
      }
      return this.m_flattened;
    }
  }
}
