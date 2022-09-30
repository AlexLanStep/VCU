// Decompiled with JetBrains decompiler
// Type: log4net.Appender.ColoredConsoleAppender
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using log4net.Core;
using log4net.Layout;
using log4net.Util;
using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace log4net.Appender
{
  public class ColoredConsoleAppender : AppenderSkeleton
  {
    public const string ConsoleOut = "Console.Out";
    public const string ConsoleError = "Console.Error";
    private const uint STD_OUTPUT_HANDLE = 4294967285;
    private const uint STD_ERROR_HANDLE = 4294967284;
    private static readonly char[] s_windowsNewline = new char[2]
    {
      '\r',
      '\n'
    };
    private bool m_writeToErrorStream = false;
    private LevelMapping m_levelMapping = new LevelMapping();
    private StreamWriter m_consoleOutputWriter = (StreamWriter) null;

    public ColoredConsoleAppender()
    {
    }

    [Obsolete("Instead use the default constructor and set the Layout property")]
    public ColoredConsoleAppender(ILayout layout)
      : this(layout, false)
    {
    }

    [Obsolete("Instead use the default constructor and set the Layout & Target properties")]
    public ColoredConsoleAppender(ILayout layout, bool writeToErrorStream)
    {
      this.Layout = layout;
      this.m_writeToErrorStream = writeToErrorStream;
    }

    public virtual string Target
    {
      get => this.m_writeToErrorStream ? "Console.Error" : "Console.Out";
      set
      {
        if (string.Compare("Console.Error", value.Trim(), true, CultureInfo.InvariantCulture) == 0)
          this.m_writeToErrorStream = true;
        else
          this.m_writeToErrorStream = false;
      }
    }

    public void AddMapping(ColoredConsoleAppender.LevelColors mapping) => this.m_levelMapping.Add((LevelMappingEntry) mapping);

    protected override void Append(LoggingEvent loggingEvent)
    {
      if (this.m_consoleOutputWriter == null)
        return;
      IntPtr zero = IntPtr.Zero;
      IntPtr consoleHandle = !this.m_writeToErrorStream ? ColoredConsoleAppender.GetStdHandle(4294967285U) : ColoredConsoleAppender.GetStdHandle(4294967284U);
      ushort attributes = 7;
      if (this.m_levelMapping.Lookup(loggingEvent.Level) is ColoredConsoleAppender.LevelColors levelColors)
        attributes = levelColors.CombinedColor;
      string str = this.RenderLoggingEvent(loggingEvent);
      ColoredConsoleAppender.CONSOLE_SCREEN_BUFFER_INFO bufferInfo;
      ColoredConsoleAppender.GetConsoleScreenBufferInfo(consoleHandle, out bufferInfo);
      ColoredConsoleAppender.SetConsoleTextAttribute(consoleHandle, attributes);
      char[] charArray = str.ToCharArray();
      int length = charArray.Length;
      bool flag = false;
      if (length > 1 && charArray[length - 2] == '\r' && charArray[length - 1] == '\n')
      {
        length -= 2;
        flag = true;
      }
      this.m_consoleOutputWriter.Write(charArray, 0, length);
      ColoredConsoleAppender.SetConsoleTextAttribute(consoleHandle, bufferInfo.wAttributes);
      if (!flag)
        return;
      this.m_consoleOutputWriter.Write(ColoredConsoleAppender.s_windowsNewline, 0, 2);
    }

    protected override bool RequiresLayout => true;

    public override void ActivateOptions()
    {
      base.ActivateOptions();
      this.m_levelMapping.ActivateOptions();
      this.m_consoleOutputWriter = new StreamWriter(!this.m_writeToErrorStream ? Console.OpenStandardOutput() : Console.OpenStandardError(), Encoding.GetEncoding(ColoredConsoleAppender.GetConsoleOutputCP()), 256);
      this.m_consoleOutputWriter.AutoFlush = true;
      GC.SuppressFinalize((object) this.m_consoleOutputWriter);
    }

    [DllImport("Kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern int GetConsoleOutputCP();

    [DllImport("Kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern bool SetConsoleTextAttribute(IntPtr consoleHandle, ushort attributes);

    [DllImport("Kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern bool GetConsoleScreenBufferInfo(
      IntPtr consoleHandle,
      out ColoredConsoleAppender.CONSOLE_SCREEN_BUFFER_INFO bufferInfo);

    [DllImport("Kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr GetStdHandle(uint type);

    [Flags]
    public enum Colors
    {
      Blue = 1,
      Green = 2,
      Red = 4,
      White = Red | Green | Blue, // 0x00000007
      Yellow = Red | Green, // 0x00000006
      Purple = Red | Blue, // 0x00000005
      Cyan = Green | Blue, // 0x00000003
      HighIntensity = 8,
    }

    private struct COORD
    {
      public ushort x;
      public ushort y;
    }

    private struct SMALL_RECT
    {
      public ushort Left;
      public ushort Top;
      public ushort Right;
      public ushort Bottom;
    }

    private struct CONSOLE_SCREEN_BUFFER_INFO
    {
      public ColoredConsoleAppender.COORD dwSize;
      public ColoredConsoleAppender.COORD dwCursorPosition;
      public ushort wAttributes;
      public ColoredConsoleAppender.SMALL_RECT srWindow;
      public ColoredConsoleAppender.COORD dwMaximumWindowSize;
    }

    public class LevelColors : LevelMappingEntry
    {
      private ColoredConsoleAppender.Colors m_foreColor;
      private ColoredConsoleAppender.Colors m_backColor;
      private ushort m_combinedColor = 0;

      public ColoredConsoleAppender.Colors ForeColor
      {
        get => this.m_foreColor;
        set => this.m_foreColor = value;
      }

      public ColoredConsoleAppender.Colors BackColor
      {
        get => this.m_backColor;
        set => this.m_backColor = value;
      }

      public override void ActivateOptions()
      {
        base.ActivateOptions();
        this.m_combinedColor = (ushort) (this.m_foreColor + ((int) this.m_backColor << 4));
      }

      internal ushort CombinedColor => this.m_combinedColor;
    }
  }
}
