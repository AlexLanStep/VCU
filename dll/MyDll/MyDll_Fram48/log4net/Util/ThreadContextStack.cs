// Decompiled with JetBrains decompiler
// Type: log4net.Util.ThreadContextStack
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using log4net.Core;
using System;
using System.Collections;

namespace log4net.Util
{
  public sealed class ThreadContextStack : IFixingRequired
  {
    private Stack m_stack = new Stack();

    internal ThreadContextStack()
    {
    }

    public int Count => this.m_stack.Count;

    public void Clear() => this.m_stack.Clear();

    public string Pop()
    {
      Stack stack = this.m_stack;
      return stack.Count > 0 ? ((ThreadContextStack.StackFrame) stack.Pop()).Message : "";
    }

    public IDisposable Push(string message)
    {
      Stack stack = this.m_stack;
      stack.Push((object) new ThreadContextStack.StackFrame(message, stack.Count > 0 ? (ThreadContextStack.StackFrame) stack.Peek() : (ThreadContextStack.StackFrame) null));
      return (IDisposable) new ThreadContextStack.AutoPopStackFrame(stack, stack.Count - 1);
    }

    internal string GetFullMessage()
    {
      Stack stack = this.m_stack;
      return stack.Count > 0 ? ((ThreadContextStack.StackFrame) stack.Peek()).FullMessage : (string) null;
    }

    internal Stack InternalStack
    {
      get => this.m_stack;
      set => this.m_stack = value;
    }

    public override string ToString() => this.GetFullMessage();

    object IFixingRequired.GetFixedObject() => (object) this.GetFullMessage();

    private sealed class StackFrame
    {
      private readonly string m_message;
      private readonly ThreadContextStack.StackFrame m_parent;
      private string m_fullMessage = (string) null;

      internal StackFrame(string message, ThreadContextStack.StackFrame parent)
      {
        this.m_message = message;
        this.m_parent = parent;
        if (parent != null)
          return;
        this.m_fullMessage = message;
      }

      internal string Message => this.m_message;

      internal string FullMessage
      {
        get
        {
          if (this.m_fullMessage == null && this.m_parent != null)
            this.m_fullMessage = this.m_parent.FullMessage + " " + this.m_message;
          return this.m_fullMessage;
        }
      }
    }

    private struct AutoPopStackFrame : IDisposable
    {
      private Stack m_frameStack;
      private int m_frameDepth;

      internal AutoPopStackFrame(Stack frameStack, int frameDepth)
      {
        this.m_frameStack = frameStack;
        this.m_frameDepth = frameDepth;
      }

      public void Dispose()
      {
        if (this.m_frameDepth < 0 || this.m_frameStack == null)
          return;
        while (this.m_frameStack.Count > this.m_frameDepth)
          this.m_frameStack.Pop();
      }
    }
  }
}
