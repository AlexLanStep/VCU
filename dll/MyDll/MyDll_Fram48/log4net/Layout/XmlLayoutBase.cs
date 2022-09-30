// Decompiled with JetBrains decompiler
// Type: log4net.Layout.XmlLayoutBase
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using log4net.Core;
using log4net.Util;
using System;
using System.IO;
using System.Xml;

namespace log4net.Layout
{
  public abstract class XmlLayoutBase : LayoutSkeleton
  {
    private bool m_locationInfo = false;
    private readonly ProtectCloseTextWriter m_protectCloseTextWriter = new ProtectCloseTextWriter((TextWriter) null);
    private string m_invalidCharReplacement = "?";

    protected XmlLayoutBase()
      : this(false)
    {
      this.IgnoresException = false;
    }

    protected XmlLayoutBase(bool locationInfo)
    {
      this.IgnoresException = false;
      this.m_locationInfo = locationInfo;
    }

    public bool LocationInfo
    {
      get => this.m_locationInfo;
      set => this.m_locationInfo = value;
    }

    public string InvalidCharReplacement
    {
      get => this.m_invalidCharReplacement;
      set => this.m_invalidCharReplacement = value;
    }

    public override void ActivateOptions()
    {
    }

    public override string ContentType => "text/xml";

    public override void Format(TextWriter writer, LoggingEvent loggingEvent)
    {
      if (loggingEvent == null)
        throw new ArgumentNullException(nameof (loggingEvent));
      this.m_protectCloseTextWriter.Attach(writer);
      XmlTextWriter writer1 = new XmlTextWriter((TextWriter) this.m_protectCloseTextWriter);
      writer1.Formatting = Formatting.None;
      writer1.Namespaces = false;
      this.FormatXml((XmlWriter) writer1, loggingEvent);
      writer1.WriteWhitespace(SystemInfo.NewLine);
      writer1.Close();
      this.m_protectCloseTextWriter.Attach((TextWriter) null);
    }

    protected abstract void FormatXml(XmlWriter writer, LoggingEvent loggingEvent);
  }
}
