// Decompiled with JetBrains decompiler
// Type: Common.Logging.ILoggerFactoryAdapter
// Assembly: Common.Logging, Version=1.1.0.1, Culture=neutral, PublicKeyToken=65e474d141e25e07
// MVID: F190E754-F88D-465E-80CE-5B5FC84116FB
// Assembly location: E:\LabCar\BasaDll\Common.Logging.dll

using System;

namespace Common.Logging
{
  public interface ILoggerFactoryAdapter
  {
    ILog GetLogger(Type type);

    ILog GetLogger(string name);
  }
}
