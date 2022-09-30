// Decompiled with JetBrains decompiler
// Type: log4net.Util.SystemStringFormat
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using System;
using System.Text;

namespace log4net.Util
{
  public sealed class SystemStringFormat
  {
    private readonly IFormatProvider m_provider;
    private readonly string m_format;
    private readonly object[] m_args;

    public SystemStringFormat(IFormatProvider provider, string format, params object[] args)
    {
      this.m_provider = provider;
      this.m_format = format;
      this.m_args = args;
    }

    public override string ToString() => SystemStringFormat.StringFormat(this.m_provider, this.m_format, this.m_args);

    private static string StringFormat(
      IFormatProvider provider,
      string format,
      params object[] args)
    {
      try
      {
        if (format == null)
          return (string) null;
        return args == null ? format : string.Format(provider, format, args);
      }
      catch (Exception ex)
      {
        LogLog.Warn("StringFormat: Exception while rendering format [" + format + "]", ex);
        return SystemStringFormat.StringFormatError(ex, format, args);
      }
      catch
      {
        LogLog.Warn("StringFormat: Exception while rendering format [" + format + "]");
        return SystemStringFormat.StringFormatError((Exception) null, format, args);
      }
    }

    private static string StringFormatError(
      Exception formatException,
      string format,
      object[] args)
    {
      try
      {
        StringBuilder buffer = new StringBuilder("<log4net.Error>");
        if (formatException != null)
          buffer.Append("Exception during StringFormat: ").Append(formatException.Message);
        else
          buffer.Append("Exception during StringFormat");
        buffer.Append(" <format>").Append(format).Append("</format>");
        buffer.Append("<args>");
        SystemStringFormat.RenderArray((Array) args, buffer);
        buffer.Append("</args>");
        buffer.Append("</log4net.Error>");
        return buffer.ToString();
      }
      catch (Exception ex)
      {
        LogLog.Error("StringFormat: INTERNAL ERROR during StringFormat error handling", ex);
        return "<log4net.Error>Exception during StringFormat. See Internal Log.</log4net.Error>";
      }
      catch
      {
        LogLog.Error("StringFormat: INTERNAL ERROR during StringFormat error handling");
        return "<log4net.Error>Exception during StringFormat. See Internal Log.</log4net.Error>";
      }
    }

    private static void RenderArray(Array array, StringBuilder buffer)
    {
      if (array == null)
        buffer.Append(SystemInfo.NullText);
      else if (array.Rank != 1)
      {
        buffer.Append(array.ToString());
      }
      else
      {
        buffer.Append("{");
        int length = array.Length;
        if (length > 0)
        {
          SystemStringFormat.RenderObject(array.GetValue(0), buffer);
          for (int index = 1; index < length; ++index)
          {
            buffer.Append(", ");
            SystemStringFormat.RenderObject(array.GetValue(index), buffer);
          }
        }
        buffer.Append("}");
      }
    }

    private static void RenderObject(object obj, StringBuilder buffer)
    {
      if (obj == null)
      {
        buffer.Append(SystemInfo.NullText);
      }
      else
      {
        try
        {
          buffer.Append(obj);
        }
        catch (Exception ex)
        {
          buffer.Append("<Exception: ").Append(ex.Message).Append(">");
        }
        catch
        {
          buffer.Append("<Exception>");
        }
      }
    }
  }
}
