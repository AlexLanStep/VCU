// Decompiled with JetBrains decompiler
// Type: log4net.Layout.Layout2RawLayoutAdapter
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using log4net.Core;
using System;
using System.Globalization;
using System.IO;

namespace log4net.Layout
{
  public class Layout2RawLayoutAdapter : IRawLayout
  {
    private ILayout m_layout;

    public Layout2RawLayoutAdapter(ILayout layout) => this.m_layout = layout;

    public virtual object Format(LoggingEvent loggingEvent)
    {
      StringWriter writer = new StringWriter((IFormatProvider) CultureInfo.InvariantCulture);
      this.m_layout.Format((TextWriter) writer, loggingEvent);
      return (object) writer.ToString();
    }
  }
}
