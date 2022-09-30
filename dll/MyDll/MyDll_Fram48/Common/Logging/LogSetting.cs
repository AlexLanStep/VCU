
using System;
using System.Collections.Specialized;


namespace Common.Logging
{

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
}