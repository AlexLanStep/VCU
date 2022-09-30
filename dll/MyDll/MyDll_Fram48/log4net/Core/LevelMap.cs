// Decompiled with JetBrains decompiler
// Type: log4net.Core.LevelMap
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using log4net.Util;
using System;
using System.Collections;

namespace log4net.Core
{
  public sealed class LevelMap
  {
    private Hashtable m_mapName2Level = SystemInfo.CreateCaseInsensitiveHashtable();

    public void Clear() => this.m_mapName2Level.Clear();

    public Level this[string name]
    {
      get
      {
        if (name == null)
          throw new ArgumentNullException(nameof (name));
        lock (this)
          return (Level) this.m_mapName2Level[(object) name];
      }
    }

    public void Add(string name, int value) => this.Add(name, value, (string) null);

    public void Add(string name, int value, string displayName)
    {
      switch (name)
      {
        case "":
          throw SystemInfo.CreateArgumentOutOfRangeException(nameof (name), (object) name, "Parameter: name, Value: [" + name + "] out of range. Level name must not be empty");
        case null:
          throw new ArgumentNullException(nameof (name));
        default:
          switch (displayName)
          {
            case "":
            case null:
              displayName = name;
              break;
          }
          this.Add(new Level(value, name, displayName));
          break;
      }
    }

    public void Add(Level level)
    {
      if (level == (Level) null)
        throw new ArgumentNullException(nameof (level));
      lock (this)
        this.m_mapName2Level[(object) level.Name] = (object) level;
    }

    public LevelCollection AllLevels
    {
      get
      {
        lock (this)
          return new LevelCollection(this.m_mapName2Level.Values);
      }
    }

    public Level LookupWithDefault(Level defaultLevel)
    {
      if (defaultLevel == (Level) null)
        throw new ArgumentNullException(nameof (defaultLevel));
      lock (this)
      {
        Level level = (Level) this.m_mapName2Level[(object) defaultLevel.Name];
        if (!(level == (Level) null))
          return level;
        this.m_mapName2Level[(object) defaultLevel.Name] = (object) defaultLevel;
        return defaultLevel;
      }
    }
  }
}
