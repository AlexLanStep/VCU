// Decompiled with JetBrains decompiler
// Type: log4net.Util.LogicalThreadContextProperties
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using System.Runtime.Remoting.Messaging;

namespace log4net.Util
{
  public sealed class LogicalThreadContextProperties : ContextPropertiesBase
  {
    internal LogicalThreadContextProperties()
    {
    }

    public override object this[string key]
    {
      get => this.GetProperties(false)?[key];
      set => this.GetProperties(true)[key] = value;
    }

    public void Remove(string key) => this.GetProperties(false)?.Remove(key);

    public void Clear() => this.GetProperties(false)?.Clear();

    internal PropertiesDictionary GetProperties(bool create)
    {
      PropertiesDictionary data = (PropertiesDictionary) CallContext.GetData("log4net.Util.LogicalThreadContextProperties");
      if (data == null && create)
      {
        data = new PropertiesDictionary();
        CallContext.SetData("log4net.Util.LogicalThreadContextProperties", (object) data);
      }
      return data;
    }
  }
}
