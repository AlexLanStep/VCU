
using log4net.Util;

namespace log4net
{
  public sealed class GlobalContext
  {
    private static readonly GlobalContextProperties s_properties = new GlobalContextProperties();

    private GlobalContext()
    {
    }

    static GlobalContext() => GlobalContext.Properties["log4net:HostName"] = (object) SystemInfo.HostName;

    public static GlobalContextProperties Properties => GlobalContext.s_properties;
  }
}
