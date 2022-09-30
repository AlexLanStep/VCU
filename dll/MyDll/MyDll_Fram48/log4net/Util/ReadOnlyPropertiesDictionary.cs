// Decompiled with JetBrains decompiler
// Type: log4net.Util.ReadOnlyPropertiesDictionary
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using System;
using System.Collections;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Xml;

namespace log4net.Util
{
  [Serializable]
  public class ReadOnlyPropertiesDictionary : ISerializable, IDictionary, ICollection, IEnumerable
  {
    private Hashtable m_hashtable = new Hashtable();

    public ReadOnlyPropertiesDictionary()
    {
    }

    public ReadOnlyPropertiesDictionary(ReadOnlyPropertiesDictionary propertiesDictionary)
    {
      foreach (DictionaryEntry properties in (IEnumerable) propertiesDictionary)
        this.InnerHashtable.Add(properties.Key, properties.Value);
    }

    protected ReadOnlyPropertiesDictionary(SerializationInfo info, StreamingContext context)
    {
      foreach (SerializationEntry serializationEntry in info)
        this.InnerHashtable[(object) XmlConvert.DecodeName(serializationEntry.Name)] = serializationEntry.Value;
    }

    public string[] GetKeys()
    {
      string[] keys = new string[this.InnerHashtable.Count];
      this.InnerHashtable.Keys.CopyTo((Array) keys, 0);
      return keys;
    }

    public virtual object this[string key]
    {
      get => this.InnerHashtable[(object) key];
      set => throw new NotSupportedException("This is a Read Only Dictionary and can not be modified");
    }

    public bool Contains(string key) => this.InnerHashtable.Contains((object) key);

    protected Hashtable InnerHashtable => this.m_hashtable;

    [PermissionSet(SecurityAction.Demand, XML = "<PermissionSet class=\"System.Security.PermissionSet\"\r\n               version=\"1\">\r\n   <IPermission class=\"System.Security.Permissions.SecurityPermission, mscorlib, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"\r\n                version=\"1\"\r\n                Flags=\"SerializationFormatter\"/>\r\n</PermissionSet>\r\n")]
    public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      foreach (DictionaryEntry dictionaryEntry in this.InnerHashtable)
      {
        string key = dictionaryEntry.Key as string;
        object obj = dictionaryEntry.Value;
        if (key != null && obj != null && obj.GetType().IsSerializable)
          info.AddValue(XmlConvert.EncodeLocalName(key), obj);
      }
    }

    IDictionaryEnumerator IDictionary.GetEnumerator() => this.InnerHashtable.GetEnumerator();

    void IDictionary.Remove(object key) => throw new NotSupportedException("This is a Read Only Dictionary and can not be modified");

    bool IDictionary.Contains(object key) => this.InnerHashtable.Contains(key);

    public virtual void Clear() => throw new NotSupportedException("This is a Read Only Dictionary and can not be modified");

    void IDictionary.Add(object key, object value) => throw new NotSupportedException("This is a Read Only Dictionary and can not be modified");

    bool IDictionary.IsReadOnly => true;

    object IDictionary.this[object key]
    {
      get => key is string ? this.InnerHashtable[key] : throw new ArgumentException("key must be a string");
      set => throw new NotSupportedException("This is a Read Only Dictionary and can not be modified");
    }

    ICollection IDictionary.Values => this.InnerHashtable.Values;

    ICollection IDictionary.Keys => this.InnerHashtable.Keys;

    bool IDictionary.IsFixedSize => this.InnerHashtable.IsFixedSize;

    void ICollection.CopyTo(Array array, int index) => this.InnerHashtable.CopyTo(array, index);

    bool ICollection.IsSynchronized => this.InnerHashtable.IsSynchronized;

    public int Count => this.InnerHashtable.Count;

    object ICollection.SyncRoot => this.InnerHashtable.SyncRoot;

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable) this.InnerHashtable).GetEnumerator();
  }
}
