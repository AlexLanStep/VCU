// Decompiled with JetBrains decompiler
// Type: log4net.Util.PatternConverter
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using log4net.Repository;
using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Text;

namespace log4net.Util
{
  public abstract class PatternConverter
  {
    private const int c_renderBufferSize = 256;
    private const int c_renderBufferMaxCapacity = 1024;
    private static readonly string[] SPACES = new string[6]
    {
      " ",
      "  ",
      "    ",
      "        ",
      "                ",
      "                                "
    };
    private PatternConverter m_next;
    private int m_min = -1;
    private int m_max = int.MaxValue;
    private bool m_leftAlign = false;
    private string m_option = (string) null;
    private ReusableStringWriter m_formatWriter = new ReusableStringWriter((IFormatProvider) CultureInfo.InvariantCulture);

    public virtual PatternConverter Next => this.m_next;

    public virtual FormattingInfo FormattingInfo
    {
      get => new FormattingInfo(this.m_min, this.m_max, this.m_leftAlign);
      set
      {
        this.m_min = value.Min;
        this.m_max = value.Max;
        this.m_leftAlign = value.LeftAlign;
      }
    }

    public virtual string Option
    {
      get => this.m_option;
      set => this.m_option = value;
    }

    protected abstract void Convert(TextWriter writer, object state);

    public virtual PatternConverter SetNext(PatternConverter patternConverter)
    {
      this.m_next = patternConverter;
      return this.m_next;
    }

    public virtual void Format(TextWriter writer, object state)
    {
      if (this.m_min < 0 && this.m_max == int.MaxValue)
      {
        this.Convert(writer, state);
      }
      else
      {
        this.m_formatWriter.Reset(1024, 256);
        this.Convert((TextWriter) this.m_formatWriter, state);
        StringBuilder stringBuilder = this.m_formatWriter.GetStringBuilder();
        int length = stringBuilder.Length;
        if (length > this.m_max)
          writer.Write(stringBuilder.ToString(length - this.m_max, this.m_max));
        else if (length < this.m_min)
        {
          if (this.m_leftAlign)
          {
            writer.Write(stringBuilder.ToString());
            PatternConverter.SpacePad(writer, this.m_min - length);
          }
          else
          {
            PatternConverter.SpacePad(writer, this.m_min - length);
            writer.Write(stringBuilder.ToString());
          }
        }
        else
          writer.Write(stringBuilder.ToString());
      }
    }

    protected static void SpacePad(TextWriter writer, int length)
    {
      for (; length >= 32; length -= 32)
        writer.Write(PatternConverter.SPACES[5]);
      for (int index = 4; index >= 0; --index)
      {
        if ((length & 1 << index) != 0)
          writer.Write(PatternConverter.SPACES[index]);
      }
    }

    protected static void WriteDictionary(
      TextWriter writer,
      ILoggerRepository repository,
      IDictionary value)
    {
      writer.Write("{");
      bool flag = true;
      foreach (DictionaryEntry dictionaryEntry in value)
      {
        if (flag)
          flag = false;
        else
          writer.Write(", ");
        PatternConverter.WriteObject(writer, repository, dictionaryEntry.Key);
        writer.Write("=");
        PatternConverter.WriteObject(writer, repository, dictionaryEntry.Value);
      }
      writer.Write("}");
    }

    protected static void WriteObject(
      TextWriter writer,
      ILoggerRepository repository,
      object value)
    {
      if (repository != null)
        repository.RendererMap.FindAndRender(value, writer);
      else if (value == null)
        writer.Write(SystemInfo.NullText);
      else
        writer.Write(value.ToString());
    }
  }
}
