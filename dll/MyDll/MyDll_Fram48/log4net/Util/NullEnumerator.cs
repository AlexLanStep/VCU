// Decompiled with JetBrains decompiler
// Type: log4net.Util.NullEnumerator
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using System;
using System.Collections;

namespace log4net.Util
{
  public sealed class NullEnumerator : IEnumerator
  {
    private static readonly NullEnumerator s_instance = new NullEnumerator();

    private NullEnumerator()
    {
    }

    public static NullEnumerator Instance => NullEnumerator.s_instance;

    public object Current => throw new InvalidOperationException();

    public bool MoveNext() => false;

    public void Reset()
    {
    }
  }
}
