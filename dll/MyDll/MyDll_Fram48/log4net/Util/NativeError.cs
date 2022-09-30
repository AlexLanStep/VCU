// Decompiled with JetBrains decompiler
// Type: log4net.Util.NativeError
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace log4net.Util
{
  public sealed class NativeError
  {
    private int m_number;
    private string m_message;

    private NativeError(int number, string message)
    {
      this.m_number = number;
      this.m_message = message;
    }

    public int Number => this.m_number;

    public string Message => this.m_message;

    public static NativeError GetLastError()
    {
      int lastWin32Error = Marshal.GetLastWin32Error();
      return new NativeError(lastWin32Error, NativeError.GetErrorMessage(lastWin32Error));
    }

    public static NativeError GetError(int number) => new NativeError(number, NativeError.GetErrorMessage(number));

    public static string GetErrorMessage(int messageId)
    {
      int num1 = 256;
      int num2 = 512;
      int num3 = 4096;
      string lpBuffer = "";
      IntPtr lpSource = new IntPtr();
      IntPtr Arguments = new IntPtr();
      string errorMessage;
      if (messageId != 0)
      {
        if (NativeError.FormatMessage(num1 | num3 | num2, ref lpSource, messageId, 0, ref lpBuffer, (int) byte.MaxValue, Arguments) > 0)
          errorMessage = lpBuffer.TrimEnd('\r', '\n');
        else
          errorMessage = (string) null;
      }
      else
        errorMessage = (string) null;
      return errorMessage;
    }

    public override string ToString() => string.Format((IFormatProvider) CultureInfo.InvariantCulture, "0x{0:x8}", (object) this.Number) + (this.Message != null ? ": " + this.Message : "");

    [DllImport("Kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern int FormatMessage(
      int dwFlags,
      ref IntPtr lpSource,
      int dwMessageId,
      int dwLanguageId,
      ref string lpBuffer,
      int nSize,
      IntPtr Arguments);
  }
}
