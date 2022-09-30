// Decompiled with JetBrains decompiler
// Type: log4net.Util.ThreadContextProperties
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using System;
using System.Threading;

namespace log4net.Util
{
  public sealed class ThreadContextProperties : ContextPropertiesBase
  {
    private static readonly LocalDataStoreSlot s_threadLocalSlot = Thread.AllocateDataSlot();

    internal ThreadContextProperties()
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
      PropertiesDictionary data = (PropertiesDictionary) Thread.GetData(ThreadContextProperties.s_threadLocalSlot);
      if (data == null && create)
      {
        data = new PropertiesDictionary();
        Thread.SetData(ThreadContextProperties.s_threadLocalSlot, (object) data);
      }
      return data;
    }
  }
}
