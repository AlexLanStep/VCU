

using log4net.Util;
using System;
using System.Collections;

namespace log4net
{
  public sealed class NDC
  {
    private NDC()
    {
    }

    public static int Depth => ThreadContext.Stacks[nameof (NDC)].Count;

    public static void Clear() => ThreadContext.Stacks[nameof (NDC)].Clear();

    public static Stack CloneStack() => ThreadContext.Stacks[nameof (NDC)].InternalStack;

    public static void Inherit(Stack stack) => ThreadContext.Stacks[nameof (NDC)].InternalStack = stack;

    public static string Pop() => ThreadContext.Stacks[nameof (NDC)].Pop();

    public static IDisposable Push(string message) => ThreadContext.Stacks[nameof (NDC)].Push(message);

    public static void Remove()
    {
    }

    public static void SetMaxDepth(int maxDepth)
    {
      if (maxDepth < 0)
        return;
      ThreadContextStack stack = ThreadContext.Stacks[nameof (NDC)];
      if (maxDepth == 0)
      {
        stack.Clear();
      }
      else
      {
        while (stack.Count > maxDepth)
          stack.Pop();
      }
    }
  }
}
