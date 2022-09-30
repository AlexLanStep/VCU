// Decompiled with JetBrains decompiler
// Type: log4net.Core.Level
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using System;

namespace log4net.Core
{
  [Serializable]
  public sealed class Level : IComparable
  {
    public static readonly Level Off = new Level(int.MaxValue, "OFF");
    public static readonly Level Emergency = new Level(120000, "EMERGENCY");
    public static readonly Level Fatal = new Level(110000, "FATAL");
    public static readonly Level Alert = new Level(100000, "ALERT");
    public static readonly Level Critical = new Level(90000, "CRITICAL");
    public static readonly Level Severe = new Level(80000, "SEVERE");
    public static readonly Level Error = new Level(70000, "ERROR");
    public static readonly Level Warn = new Level(60000, "WARN");
    public static readonly Level Notice = new Level(50000, "NOTICE");
    public static readonly Level Info = new Level(40000, "INFO");
    public static readonly Level Debug = new Level(30000, "DEBUG");
    public static readonly Level Fine = new Level(30000, "FINE");
    public static readonly Level Trace = new Level(20000, "TRACE");
    public static readonly Level Finer = new Level(20000, "FINER");
    public static readonly Level Verbose = new Level(10000, "VERBOSE");
    public static readonly Level Finest = new Level(10000, "FINEST");
    public static readonly Level All = new Level(int.MinValue, "ALL");
    private readonly int m_levelValue;
    private readonly string m_levelName;
    private readonly string m_levelDisplayName;

    public Level(int level, string levelName, string displayName)
    {
      if (levelName == null)
        throw new ArgumentNullException(nameof (levelName));
      if (displayName == null)
        throw new ArgumentNullException(nameof (displayName));
      this.m_levelValue = level;
      this.m_levelName = string.Intern(levelName);
      this.m_levelDisplayName = displayName;
    }

    public Level(int level, string levelName)
      : this(level, levelName, levelName)
    {
    }

    public string Name => this.m_levelName;

    public int Value => this.m_levelValue;

    public string DisplayName => this.m_levelDisplayName;

    public override string ToString() => this.m_levelName;

    public override bool Equals(object o)
    {
      Level level = o as Level;
      return level != (Level) null ? this.m_levelValue == level.m_levelValue : base.Equals(o);
    }

    public override int GetHashCode() => this.m_levelValue;

    public int CompareTo(object r)
    {
      Level r1 = r as Level;
      return r1 != (Level) null ? Level.Compare(this, r1) : throw new ArgumentException("Parameter: r, Value: [" + r + "] is not an instance of Level");
    }

    public static bool operator >(Level l, Level r) => l.m_levelValue > r.m_levelValue;

    public static bool operator <(Level l, Level r) => l.m_levelValue < r.m_levelValue;

    public static bool operator >=(Level l, Level r) => l.m_levelValue >= r.m_levelValue;

    public static bool operator <=(Level l, Level r) => l.m_levelValue <= r.m_levelValue;

    public static bool operator ==(Level l, Level r) => (object) l != null && (object) r != null ? l.m_levelValue == r.m_levelValue : (object) l == (object) r;

    public static bool operator !=(Level l, Level r) => !(l == r);

    public static int Compare(Level l, Level r)
    {
      if ((object) l == (object) r || l == (Level) null && r == (Level) null)
        return 0;
      if (l == (Level) null)
        return -1;
      return r == (Level) null ? 1 : l.m_levelValue - r.m_levelValue;
    }
  }
}
