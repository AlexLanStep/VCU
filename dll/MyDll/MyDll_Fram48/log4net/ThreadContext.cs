

using log4net.Util;

namespace log4net
{
  public sealed class ThreadContext
  {
    private static readonly ThreadContextProperties s_properties = new ThreadContextProperties();
    private static readonly ThreadContextStacks s_stacks = new ThreadContextStacks((ContextPropertiesBase) ThreadContext.s_properties);

    private ThreadContext()
    {
    }

    public static ThreadContextProperties Properties => ThreadContext.s_properties;

    public static ThreadContextStacks Stacks => ThreadContext.s_stacks;
  }
}
