// Decompiled with JetBrains decompiler
// Type: log4net.Appender.AnsiColorTerminalAppender
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using log4net.Core;
using log4net.Util;
using System;
using System.Globalization;
using System.Text;

namespace log4net.Appender
{
  public class AnsiColorTerminalAppender : AppenderSkeleton
  {
    public const string ConsoleOut = "Console.Out";
    public const string ConsoleError = "Console.Error";
    private const string PostEventCodes = "\u001B[0m";
    private bool m_writeToErrorStream = false;
    private LevelMapping m_levelMapping = new LevelMapping();

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

    public void AddMapping(AnsiColorTerminalAppender.LevelColors mapping) => this.m_levelMapping.Add((LevelMappingEntry) mapping);

    protected override void Append(LoggingEvent loggingEvent)
    {
      string str1 = this.RenderLoggingEvent(loggingEvent);
      if (this.m_levelMapping.Lookup(loggingEvent.Level) is AnsiColorTerminalAppender.LevelColors levelColors)
        str1 = levelColors.CombinedColor + str1;
      string str2 = str1.Length <= 1 ? (str1[0] == '\n' || str1[0] == '\r' ? "\u001B[0m" + str1 : str1 + "\u001B[0m") : (str1.EndsWith("\r\n") || str1.EndsWith("\n\r") ? str1.Insert(str1.Length - 2, "\u001B[0m") : (str1.EndsWith("\n") || str1.EndsWith("\r") ? str1.Insert(str1.Length - 1, "\u001B[0m") : str1 + "\u001B[0m"));
      if (this.m_writeToErrorStream)
        Console.Error.Write(str2);
      else
        Console.Write(str2);
    }

    protected override bool RequiresLayout => true;

    public override void ActivateOptions()
    {
      base.ActivateOptions();
      this.m_levelMapping.ActivateOptions();
    }

    [Flags]
    public enum AnsiAttributes
    {
      Bright = 1,
      Dim = 2,
      Underscore = 4,
      Blink = 8,
      Reverse = 16, // 0x00000010
      Hidden = 32, // 0x00000020
      Strikethrough = 64, // 0x00000040
    }

    public enum AnsiColor
    {
      Black,
      Red,
      Green,
      Yellow,
      Blue,
      Magenta,
      Cyan,
      White,
    }

    public class LevelColors : LevelMappingEntry
    {
      private AnsiColorTerminalAppender.AnsiColor m_foreColor;
      private AnsiColorTerminalAppender.AnsiColor m_backColor;
      private AnsiColorTerminalAppender.AnsiAttributes m_attributes;
      private string m_combinedColor = "";

      public AnsiColorTerminalAppender.AnsiColor ForeColor
      {
        get => this.m_foreColor;
        set => this.m_foreColor = value;
      }

      public AnsiColorTerminalAppender.AnsiColor BackColor
      {
        get => this.m_backColor;
        set => this.m_backColor = value;
      }

      public AnsiColorTerminalAppender.AnsiAttributes Attributes
      {
        get => this.m_attributes;
        set => this.m_attributes = value;
      }

      public override void ActivateOptions()
      {
        base.ActivateOptions();
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("\u001B[0;");
        stringBuilder.Append((int) (30 + this.m_foreColor));
        stringBuilder.Append(';');
        stringBuilder.Append((int) (40 + this.m_backColor));
        if ((this.m_attributes & AnsiColorTerminalAppender.AnsiAttributes.Bright) > (AnsiColorTerminalAppender.AnsiAttributes) 0)
          stringBuilder.Append(";1");
        if ((this.m_attributes & AnsiColorTerminalAppender.AnsiAttributes.Dim) > (AnsiColorTerminalAppender.AnsiAttributes) 0)
          stringBuilder.Append(";2");
        if ((this.m_attributes & AnsiColorTerminalAppender.AnsiAttributes.Underscore) > (AnsiColorTerminalAppender.AnsiAttributes) 0)
          stringBuilder.Append(";4");
        if ((this.m_attributes & AnsiColorTerminalAppender.AnsiAttributes.Blink) > (AnsiColorTerminalAppender.AnsiAttributes) 0)
          stringBuilder.Append(";5");
        if ((this.m_attributes & AnsiColorTerminalAppender.AnsiAttributes.Reverse) > (AnsiColorTerminalAppender.AnsiAttributes) 0)
          stringBuilder.Append(";7");
        if ((this.m_attributes & AnsiColorTerminalAppender.AnsiAttributes.Hidden) > (AnsiColorTerminalAppender.AnsiAttributes) 0)
          stringBuilder.Append(";8");
        if ((this.m_attributes & AnsiColorTerminalAppender.AnsiAttributes.Strikethrough) > (AnsiColorTerminalAppender.AnsiAttributes) 0)
          stringBuilder.Append(";9");
        stringBuilder.Append('m');
        this.m_combinedColor = stringBuilder.ToString();
      }

      internal string CombinedColor => this.m_combinedColor;
    }
  }
}
