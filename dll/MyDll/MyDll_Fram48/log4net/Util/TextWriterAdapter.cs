// Decompiled with JetBrains decompiler
// Type: log4net.Util.TextWriterAdapter
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace log4net.Util
{
  public abstract class TextWriterAdapter : TextWriter
  {
    private TextWriter m_writer;

    protected TextWriterAdapter(TextWriter writer)
      : base((IFormatProvider) CultureInfo.InvariantCulture)
    {
      this.m_writer = writer;
    }

    protected TextWriter Writer
    {
      get => this.m_writer;
      set => this.m_writer = value;
    }

    public override Encoding Encoding => this.m_writer.Encoding;

    public override IFormatProvider FormatProvider => this.m_writer.FormatProvider;

    public override string NewLine
    {
      get => this.m_writer.NewLine;
      set => this.m_writer.NewLine = value;
    }

    public override void Close() => this.m_writer.Close();

    protected override void Dispose(bool disposing)
    {
      if (!disposing)
        return;
      this.m_writer.Dispose();
    }

    public override void Flush() => this.m_writer.Flush();

    public override void Write(char value) => this.m_writer.Write(value);

    public override void Write(char[] buffer, int index, int count) => this.m_writer.Write(buffer, index, count);

    public override void Write(string value) => this.m_writer.Write(value);
  }
}
