// Decompiled with JetBrains decompiler
// Type: log4net.NDC
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

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
