// Decompiled with JetBrains decompiler
// Type: log4net.Util.LevelMapping
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using log4net.Core;
using System;
using System.Collections;

namespace log4net.Util
{
  public sealed class LevelMapping : IOptionHandler
  {
    private Hashtable m_entriesMap = new Hashtable();
    private LevelMappingEntry[] m_entries = (LevelMappingEntry[]) null;

    public void Add(LevelMappingEntry entry)
    {
      if (this.m_entriesMap.ContainsKey((object) entry.Level))
        this.m_entriesMap.Remove((object) entry.Level);
      this.m_entriesMap.Add((object) entry.Level, (object) entry);
    }

    public LevelMappingEntry Lookup(Level level)
    {
      if (this.m_entries != null)
      {
        foreach (LevelMappingEntry entry in this.m_entries)
        {
          if (level >= entry.Level)
            return entry;
        }
      }
      return (LevelMappingEntry) null;
    }

    public void ActivateOptions()
    {
      Level[] keys = new Level[this.m_entriesMap.Count];
      LevelMappingEntry[] items = new LevelMappingEntry[this.m_entriesMap.Count];
      this.m_entriesMap.Keys.CopyTo((Array) keys, 0);
      this.m_entriesMap.Values.CopyTo((Array) items, 0);
      Array.Sort((Array) keys, (Array) items, 0, keys.Length, (IComparer) null);
      Array.Reverse((Array) items, 0, items.Length);
      foreach (LevelMappingEntry levelMappingEntry in items)
        levelMappingEntry.ActivateOptions();
      this.m_entries = items;
    }
  }
}
