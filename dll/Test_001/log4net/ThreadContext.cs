// Decompiled with JetBrains decompiler
// Type: log4net.ThreadContext
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

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
