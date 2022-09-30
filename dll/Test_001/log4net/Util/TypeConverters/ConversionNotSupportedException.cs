// Decompiled with JetBrains decompiler
// Type: log4net.Util.TypeConverters.ConversionNotSupportedException
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using System;
using System.Runtime.Serialization;

namespace log4net.Util.TypeConverters
{
  [Serializable]
  public class ConversionNotSupportedException : ApplicationException
  {
    public ConversionNotSupportedException()
    {
    }

    public ConversionNotSupportedException(string message)
      : base(message)
    {
    }

    public ConversionNotSupportedException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    protected ConversionNotSupportedException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }

    public static ConversionNotSupportedException Create(
      Type destinationType,
      object sourceValue)
    {
      return ConversionNotSupportedException.Create(destinationType, sourceValue, (Exception) null);
    }

    public static ConversionNotSupportedException Create(
      Type destinationType,
      object sourceValue,
      Exception innerException)
    {
      if (sourceValue == null)
        return new ConversionNotSupportedException("Cannot convert value [null] to type [" + (object) destinationType + "]", innerException);
      return new ConversionNotSupportedException("Cannot convert from type [" + (object) sourceValue.GetType() + "] value [" + sourceValue + "] to type [" + (object) destinationType + "]", innerException);
    }
  }
}
