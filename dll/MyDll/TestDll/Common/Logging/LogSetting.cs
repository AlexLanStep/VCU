// Decompiled with JetBrains decompiler
// Type: Common.Logging.LogSetting
// Assembly: Common.Logging, Version=1.1.0.1, Culture=neutral, PublicKeyToken=65e474d141e25e07
// MVID: F190E754-F88D-465E-80CE-5B5FC84116FB
// Assembly location: E:\LabCar\BasaDll\Common.Logging.dll

namespace Common.Logging;

internal class LogSetting
{
  private Type _factoryAdapterType = (Type)null;
  private NameValueCollection _properties = (NameValueCollection)null;

  public LogSetting(Type factoryAdapterType, NameValueCollection properties)
  {
    this._factoryAdapterType = factoryAdapterType;
    this._properties = properties;
  }

  public Type FactoryAdapterType => this._factoryAdapterType;

  public NameValueCollection Properties => this._properties;
}
