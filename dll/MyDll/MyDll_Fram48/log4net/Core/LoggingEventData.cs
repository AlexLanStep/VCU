// Decompiled with JetBrains decompiler
// Type: log4net.Core.LoggingEventData
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using log4net.Util;
using System;

namespace log4net.Core
{
  public struct LoggingEventData
  {
    public string LoggerName;
    public Level Level;
    public string Message;
    public string ThreadName;
    public DateTime TimeStamp;
    public LocationInfo LocationInfo;
    public string UserName;
    public string Identity;
    public string ExceptionString;
    public string Domain;
    public PropertiesDictionary Properties;
  }
}
