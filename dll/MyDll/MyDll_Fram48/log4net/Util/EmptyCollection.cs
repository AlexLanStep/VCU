// Decompiled with JetBrains decompiler
// Type: log4net.Util.EmptyCollection
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using System;
using System.Collections;

namespace log4net.Util
{
  [Serializable]
  public sealed class EmptyCollection : ICollection, IEnumerable
  {
    private static readonly EmptyCollection s_instance = new EmptyCollection();

    private EmptyCollection()
    {
    }

    public static EmptyCollection Instance => EmptyCollection.s_instance;

    public void CopyTo(Array array, int index)
    {
    }

    public bool IsSynchronized => true;

    public int Count => 0;

    public object SyncRoot => (object) this;

    public IEnumerator GetEnumerator() => (IEnumerator) NullEnumerator.Instance;
  }
}
