// Decompiled with JetBrains decompiler
// Type: log4net.Util.EmptyDictionary
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using System;
using System.Collections;

namespace log4net.Util
{
  [Serializable]
  public sealed class EmptyDictionary : IDictionary, ICollection, IEnumerable
  {
    private static readonly EmptyDictionary s_instance = new EmptyDictionary();

    private EmptyDictionary()
    {
    }

    public static EmptyDictionary Instance => EmptyDictionary.s_instance;

    public void CopyTo(Array array, int index)
    {
    }

    public bool IsSynchronized => true;

    public int Count => 0;

    public object SyncRoot => (object) this;

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) NullEnumerator.Instance;

    public void Add(object key, object value) => throw new InvalidOperationException();

    public void Clear() => throw new InvalidOperationException();

    public bool Contains(object key) => false;

    public IDictionaryEnumerator GetEnumerator() => (IDictionaryEnumerator) NullDictionaryEnumerator.Instance;

    public void Remove(object key) => throw new InvalidOperationException();

    public bool IsFixedSize => true;

    public bool IsReadOnly => true;

    public ICollection Keys => (ICollection) EmptyCollection.Instance;

    public ICollection Values => (ICollection) EmptyCollection.Instance;

    public object this[object key]
    {
      get => (object) null;
      set => throw new InvalidOperationException();
    }
  }
}
