// Decompiled with JetBrains decompiler
// Type: log4net.Util.TypeConverters.TypeConverterAttribute
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using System;

namespace log4net.Util.TypeConverters
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Interface)]
  public sealed class TypeConverterAttribute : Attribute
  {
    private string m_typeName = (string) null;

    public TypeConverterAttribute()
    {
    }

    public TypeConverterAttribute(string typeName) => this.m_typeName = typeName;

    public TypeConverterAttribute(Type converterType) => this.m_typeName = SystemInfo.AssemblyQualifiedName(converterType);

    public string ConverterTypeName
    {
      get => this.m_typeName;
      set => this.m_typeName = value;
    }
  }
}
