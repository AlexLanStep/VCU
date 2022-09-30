// Decompiled with JetBrains decompiler
// Type: log4net.Util.OptionConverter
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using log4net.Core;
using log4net.Util.TypeConverters;
using System;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace log4net.Util
{
  public sealed class OptionConverter
  {
    private const string DELIM_START = "${";
    private const char DELIM_STOP = '}';
    private const int DELIM_START_LEN = 2;
    private const int DELIM_STOP_LEN = 1;

    private OptionConverter()
    {
    }

    public static bool ToBoolean(string argValue, bool defaultValue)
    {
      if (argValue != null)
      {
        if (argValue.Length > 0)
        {
          try
          {
            return bool.Parse(argValue);
          }
          catch (Exception ex)
          {
            LogLog.Error("OptionConverter: [" + argValue + "] is not in proper bool form.", ex);
          }
        }
      }
      return defaultValue;
    }

    public static long ToFileSize(string argValue, long defaultValue)
    {
      if (argValue == null)
        return defaultValue;
      string str = argValue.Trim().ToUpper(CultureInfo.InvariantCulture);
      long num = 1;
      int length1;
      if ((length1 = str.IndexOf("KB")) != -1)
      {
        num = 1024L;
        str = str.Substring(0, length1);
      }
      else
      {
        int length2;
        if ((length2 = str.IndexOf("MB")) != -1)
        {
          num = 1048576L;
          str = str.Substring(0, length2);
        }
        else
        {
          int length3;
          if ((length3 = str.IndexOf("GB")) != -1)
          {
            num = 1073741824L;
            str = str.Substring(0, length3);
          }
        }
      }
      if (str != null)
      {
        string s = str.Trim();
        long val;
        if (SystemInfo.TryParse(s, out val))
          return val * num;
        LogLog.Error("OptionConverter: [" + s + "] is not in the correct file size syntax.");
      }
      return defaultValue;
    }

    public static object ConvertStringTo(Type target, string txt)
    {
      if (target == null)
        throw new ArgumentNullException(nameof (target));
      if (typeof (string) == target || typeof (object) == target)
        return (object) txt;
      IConvertFrom convertFrom = ConverterRegistry.GetConvertFrom(target);
      if (convertFrom != null && convertFrom.CanConvertFrom(typeof (string)))
        return convertFrom.ConvertFrom((object) txt);
      if (target.IsEnum)
        return OptionConverter.ParseEnum(target, txt, true);
      return target.GetMethod("Parse", new Type[1]
      {
        typeof (string)
      })?.Invoke((object) null, BindingFlags.InvokeMethod, (Binder) null, new object[1]
      {
        (object) txt
      }, CultureInfo.InvariantCulture);
    }

    public static bool CanConvertTypeTo(Type sourceType, Type targetType)
    {
      if (sourceType == null || targetType == null)
        return false;
      if (targetType.IsAssignableFrom(sourceType))
        return true;
      IConvertTo convertTo = ConverterRegistry.GetConvertTo(sourceType, targetType);
      if (convertTo != null && convertTo.CanConvertTo(targetType))
        return true;
      IConvertFrom convertFrom = ConverterRegistry.GetConvertFrom(targetType);
      return convertFrom != null && convertFrom.CanConvertFrom(sourceType);
    }

    public static object ConvertTypeTo(object sourceInstance, Type targetType)
    {
      Type type = sourceInstance.GetType();
      if (targetType.IsAssignableFrom(type))
        return sourceInstance;
      IConvertTo convertTo = ConverterRegistry.GetConvertTo(type, targetType);
      if (convertTo != null && convertTo.CanConvertTo(targetType))
        return convertTo.ConvertTo(sourceInstance, targetType);
      IConvertFrom convertFrom = ConverterRegistry.GetConvertFrom(targetType);
      return convertFrom != null && convertFrom.CanConvertFrom(type) ? convertFrom.ConvertFrom(sourceInstance) : throw new ArgumentException("Cannot convert source object [" + sourceInstance.ToString() + "] to target type [" + targetType.Name + "]", nameof (sourceInstance));
    }

    public static object InstantiateByClassName(
      string className,
      Type superClass,
      object defaultValue)
    {
      if (className != null)
      {
        try
        {
          Type typeFromString = SystemInfo.GetTypeFromString(className, true, true);
          if (superClass.IsAssignableFrom(typeFromString))
            return Activator.CreateInstance(typeFromString);
          LogLog.Error("OptionConverter: A [" + className + "] object is not assignable to a [" + superClass.FullName + "] variable.");
          return defaultValue;
        }
        catch (Exception ex)
        {
          LogLog.Error("OptionConverter: Could not instantiate class [" + className + "].", ex);
        }
      }
      return defaultValue;
    }

    public static string SubstituteVariables(string value, IDictionary props)
    {
      StringBuilder stringBuilder = new StringBuilder();
      int startIndex1 = 0;
      int startIndex2;
      while (true)
      {
        startIndex2 = value.IndexOf("${", startIndex1);
        if (startIndex2 != -1)
        {
          stringBuilder.Append(value.Substring(startIndex1, startIndex2 - startIndex1));
          int num = value.IndexOf('}', startIndex2);
          if (num != -1)
          {
            int startIndex3 = startIndex2 + 2;
            string key = value.Substring(startIndex3, num - startIndex3);
            if (props[(object) key] is string prop)
              stringBuilder.Append(prop);
            startIndex1 = num + 1;
          }
          else
            goto label_6;
        }
        else
          break;
      }
      if (startIndex1 == 0)
        return value;
      stringBuilder.Append(value.Substring(startIndex1, value.Length - startIndex1));
      return stringBuilder.ToString();
label_6:
      throw new LogException("[" + value + "] has no closing brace. Opening brace at position [" + (object) startIndex2 + "]");
    }

    private static object ParseEnum(Type enumType, string value, bool ignoreCase) => Enum.Parse(enumType, value, ignoreCase);
  }
}
