

using log4net.Util;

namespace log4net
{
  public sealed class LogicalThreadContext
  {
    private static readonly LogicalThreadContextProperties s_properties = new LogicalThreadContextProperties();
    private static readonly ThreadContextStacks s_stacks = new ThreadContextStacks((ContextPropertiesBase) LogicalThreadContext.s_properties);

    private LogicalThreadContext()
    {
    }

    public static LogicalThreadContextProperties Properties => LogicalThreadContext.s_properties;

    public static ThreadContextStacks Stacks => LogicalThreadContext.s_stacks;
  }
}
