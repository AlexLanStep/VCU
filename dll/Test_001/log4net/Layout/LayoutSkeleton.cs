// Decompiled with JetBrains decompiler
// Type: log4net.Layout.LayoutSkeleton
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using log4net.Core;
using System.IO;

namespace log4net.Layout
{
  public abstract class LayoutSkeleton : ILayout, IOptionHandler
  {
    private string m_header = (string) null;
    private string m_footer = (string) null;
    private bool m_ignoresException = true;

    public abstract void ActivateOptions();

    public abstract void Format(TextWriter writer, LoggingEvent loggingEvent);

    public virtual string ContentType => "text/plain";

    public virtual string Header
    {
      get => this.m_header;
      set => this.m_header = value;
    }

    public virtual string Footer
    {
      get => this.m_footer;
      set => this.m_footer = value;
    }

    public virtual bool IgnoresException
    {
      get => this.m_ignoresException;
      set => this.m_ignoresException = value;
    }
  }
}
