

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
