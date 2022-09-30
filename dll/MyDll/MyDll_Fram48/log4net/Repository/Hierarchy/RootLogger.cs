// Decompiled with JetBrains decompiler
// Type: log4net.Repository.Hierarchy.RootLogger
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using log4net.Core;
using log4net.Util;
using System;

namespace log4net.Repository.Hierarchy
{
  public class RootLogger : Logger
  {
    public RootLogger(Level level)
      : base("root")
    {
      this.Level = level;
    }

    public override Level EffectiveLevel => base.Level;

    public override Level Level
    {
      get => base.Level;
      set
      {
        if (value == (Level) null)
          LogLog.Error("RootLogger: You have tried to set a null level to root.", (Exception) new LogException());
        else
          base.Level = value;
      }
    }
  }
}
