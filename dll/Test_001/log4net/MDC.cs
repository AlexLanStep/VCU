// Decompiled with JetBrains decompiler
// Type: log4net.MDC
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

namespace log4net
{
  public sealed class MDC
  {
    private MDC()
    {
    }

    public static string Get(string key) => ThreadContext.Properties[key]?.ToString();

    public static void Set(string key, string value) => ThreadContext.Properties[key] = (object) value;

    public static void Remove(string key) => ThreadContext.Properties.Remove(key);

    public static void Clear() => ThreadContext.Properties.Clear();
  }
}
