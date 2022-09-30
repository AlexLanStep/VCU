// Decompiled with JetBrains decompiler
// Type: log4net.Util.SystemInfo
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Security;

namespace log4net.Util
{
  public sealed class SystemInfo
  {
    private const string DEFAULT_NULL_TEXT = "(null)";
    private const string DEFAULT_NOT_AVAILABLE_TEXT = "NOT AVAILABLE";
    public static readonly Type[] EmptyTypes = new Type[0];
    private static string s_hostName;
    private static string s_appFriendlyName;
    private static string s_nullText;
    private static string s_notAvailableText;
    private static DateTime s_processStartTime = DateTime.Now;

    private SystemInfo()
    {
    }

    static SystemInfo()
    {
      string str1 = "(null)";
      string str2 = "NOT AVAILABLE";
      string appSetting1 = SystemInfo.GetAppSetting("log4net.NullText");
      if (appSetting1 != null && appSetting1.Length > 0)
      {
        LogLog.Debug("SystemInfo: Initializing NullText value to [" + appSetting1 + "].");
        str1 = appSetting1;
      }
      string appSetting2 = SystemInfo.GetAppSetting("log4net.NotAvailableText");
      if (appSetting2 != null && appSetting2.Length > 0)
      {
        LogLog.Debug("SystemInfo: Initializing NotAvailableText value to [" + appSetting2 + "].");
        str2 = appSetting2;
      }
      SystemInfo.s_notAvailableText = str2;
      SystemInfo.s_nullText = str1;
    }

    public static string NewLine => Environment.NewLine;

    public static string ApplicationBaseDirectory => AppDomain.CurrentDomain.BaseDirectory;

    public static string ConfigurationFileLocation => AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;

    public static string EntryAssemblyLocation => Assembly.GetEntryAssembly().Location;

    public static int CurrentThreadId => AppDomain.GetCurrentThreadId();

    public static string HostName
    {
      get
      {
        if (SystemInfo.s_hostName == null)
        {
          try
          {
            SystemInfo.s_hostName = Dns.GetHostName();
          }
          catch (SocketException ex)
          {
          }
          catch (SecurityException ex)
          {
          }
          if (SystemInfo.s_hostName != null)
          {
            if (SystemInfo.s_hostName.Length != 0)
              goto label_9;
          }
          try
          {
            SystemInfo.s_hostName = Environment.MachineName;
          }
          catch (InvalidOperationException ex)
          {
          }
          catch (SecurityException ex)
          {
          }
label_9:
          if (SystemInfo.s_hostName == null || SystemInfo.s_hostName.Length == 0)
            SystemInfo.s_hostName = SystemInfo.s_notAvailableText;
        }
        return SystemInfo.s_hostName;
      }
    }

    public static string ApplicationFriendlyName
    {
      get
      {
        if (SystemInfo.s_appFriendlyName == null)
        {
          try
          {
            SystemInfo.s_appFriendlyName = AppDomain.CurrentDomain.FriendlyName;
          }
          catch (SecurityException ex)
          {
            LogLog.Debug("SystemInfo: Security exception while trying to get current domain friendly name. Error Ignored.");
          }
          if (SystemInfo.s_appFriendlyName != null)
          {
            if (SystemInfo.s_appFriendlyName.Length != 0)
              goto label_7;
          }
          try
          {
            SystemInfo.s_appFriendlyName = Path.GetFileName(SystemInfo.EntryAssemblyLocation);
          }
          catch (SecurityException ex)
          {
          }
label_7:
          if (SystemInfo.s_appFriendlyName == null || SystemInfo.s_appFriendlyName.Length == 0)
            SystemInfo.s_appFriendlyName = SystemInfo.s_notAvailableText;
        }
        return SystemInfo.s_appFriendlyName;
      }
    }

    public static DateTime ProcessStartTime => SystemInfo.s_processStartTime;

    public static string NullText
    {
      get => SystemInfo.s_nullText;
      set => SystemInfo.s_nullText = value;
    }

    public static string NotAvailableText
    {
      get => SystemInfo.s_notAvailableText;
      set => SystemInfo.s_notAvailableText = value;
    }

    public static string AssemblyLocationInfo(Assembly myAssembly)
    {
      if (myAssembly.GlobalAssemblyCache)
        return "Global Assembly Cache";
      try
      {
        return myAssembly.Location;
      }
      catch (SecurityException ex)
      {
        return "Location Permission Denied";
      }
    }

    public static string AssemblyQualifiedName(Type type) => type.FullName + ", " + type.Assembly.FullName;

    public static string AssemblyShortName(Assembly myAssembly)
    {
      string str = myAssembly.FullName;
      int length = str.IndexOf(',');
      if (length > 0)
        str = str.Substring(0, length);
      return str.Trim();
    }

    public static string AssemblyFileName(Assembly myAssembly) => Path.GetFileName(myAssembly.Location);

    public static Type GetTypeFromString(
      Type relativeType,
      string typeName,
      bool throwOnError,
      bool ignoreCase)
    {
      return SystemInfo.GetTypeFromString(relativeType.Assembly, typeName, throwOnError, ignoreCase);
    }

    public static Type GetTypeFromString(string typeName, bool throwOnError, bool ignoreCase) => SystemInfo.GetTypeFromString(Assembly.GetCallingAssembly(), typeName, throwOnError, ignoreCase);

    public static Type GetTypeFromString(
      Assembly relativeAssembly,
      string typeName,
      bool throwOnError,
      bool ignoreCase)
    {
      if (typeName.IndexOf(',') != -1)
        return Type.GetType(typeName, throwOnError, ignoreCase);
      Type type1 = relativeAssembly.GetType(typeName, false, ignoreCase);
      if (type1 != null)
        return type1;
      Assembly[] assemblyArray = (Assembly[]) null;
      try
      {
        assemblyArray = AppDomain.CurrentDomain.GetAssemblies();
      }
      catch (SecurityException ex)
      {
      }
      if (assemblyArray != null)
      {
        foreach (Assembly assembly in assemblyArray)
        {
          Type type2 = assembly.GetType(typeName, false, ignoreCase);
          if (type2 != null)
          {
            LogLog.Debug("SystemInfo: Loaded type [" + typeName + "] from assembly [" + assembly.FullName + "] by searching loaded assemblies.");
            return type2;
          }
        }
      }
      if (throwOnError)
        throw new TypeLoadException("Could not load type [" + typeName + "]. Tried assembly [" + relativeAssembly.FullName + "] and all loaded assemblies");
      return (Type) null;
    }

    public static Guid NewGuid() => Guid.NewGuid();

    public static ArgumentOutOfRangeException CreateArgumentOutOfRangeException(
      string parameterName,
      object actualValue,
      string message)
    {
      return new ArgumentOutOfRangeException(parameterName, actualValue, message);
    }

    public static bool TryParse(string s, out int val)
    {
      val = 0;
      try
      {
        double result;
        if (double.TryParse(s, NumberStyles.Integer, (IFormatProvider) CultureInfo.InvariantCulture, out result))
        {
          val = Convert.ToInt32(result);
          return true;
        }
      }
      catch
      {
      }
      return false;
    }

    public static bool TryParse(string s, out long val)
    {
      val = 0L;
      try
      {
        double result;
        if (double.TryParse(s, NumberStyles.Integer, (IFormatProvider) CultureInfo.InvariantCulture, out result))
        {
          val = Convert.ToInt64(result);
          return true;
        }
      }
      catch
      {
      }
      return false;
    }

    public static string GetAppSetting(string key)
    {
      try
      {
        return ConfigurationSettings.AppSettings[key];
      }
      catch (Exception ex)
      {
        LogLog.Error("DefaultRepositorySelector: Exception while reading ConfigurationSettings. Check your .config file is well formed XML.", ex);
      }
      return (string) null;
    }

    public static string ConvertToFullPath(string path)
    {
      if (path == null)
        throw new ArgumentNullException(nameof (path));
      string path1 = "";
      try
      {
        string applicationBaseDirectory = SystemInfo.ApplicationBaseDirectory;
        if (applicationBaseDirectory != null)
        {
          Uri uri = new Uri(applicationBaseDirectory);
          if (uri.IsFile)
            path1 = uri.LocalPath;
        }
      }
      catch
      {
      }
      return path1 != null && path1.Length > 0 ? Path.GetFullPath(Path.Combine(path1, path)) : Path.GetFullPath(path);
    }

    public static Hashtable CreateCaseInsensitiveHashtable() => CollectionsUtil.CreateCaseInsensitiveHashtable();
  }
}
