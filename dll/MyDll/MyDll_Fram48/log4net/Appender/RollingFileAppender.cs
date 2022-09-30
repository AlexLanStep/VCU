// Decompiled with JetBrains decompiler
// Type: log4net.Appender.RollingFileAppender
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using log4net.Core;
using log4net.Util;
using System;
using System.Collections;
using System.Globalization;
using System.IO;

namespace log4net.Appender
{
  public class RollingFileAppender : FileAppender
  {
    private RollingFileAppender.IDateTime m_dateTime = (RollingFileAppender.IDateTime) null;
    private string m_datePattern = ".yyyy-MM-dd";
    private string m_scheduledFilename = (string) null;
    private DateTime m_nextCheck = DateTime.MaxValue;
    private DateTime m_now;
    private RollingFileAppender.RollPoint m_rollPoint;
    private long m_maxFileSize = 10485760;
    private int m_maxSizeRollBackups = 0;
    private int m_curSizeRollBackups = 0;
    private int m_countDirection = -1;
    private RollingFileAppender.RollingMode m_rollingStyle = RollingFileAppender.RollingMode.Composite;
    private bool m_rollDate = true;
    private bool m_rollSize = true;
    private bool m_staticLogFileName = true;
    private string m_baseFileName;
    private static readonly DateTime s_date1970 = new DateTime(1970, 1, 1);

    public RollingFileAppender() => this.m_dateTime = (RollingFileAppender.IDateTime) new RollingFileAppender.DefaultDateTime();

    public string DatePattern
    {
      get => this.m_datePattern;
      set => this.m_datePattern = value;
    }

    public int MaxSizeRollBackups
    {
      get => this.m_maxSizeRollBackups;
      set => this.m_maxSizeRollBackups = value;
    }

    public long MaxFileSize
    {
      get => this.m_maxFileSize;
      set => this.m_maxFileSize = value;
    }

    public string MaximumFileSize
    {
      get => this.m_maxFileSize.ToString((IFormatProvider) NumberFormatInfo.InvariantInfo);
      set => this.m_maxFileSize = OptionConverter.ToFileSize(value, this.m_maxFileSize + 1L);
    }

    public int CountDirection
    {
      get => this.m_countDirection;
      set => this.m_countDirection = value;
    }

    public RollingFileAppender.RollingMode RollingStyle
    {
      get => this.m_rollingStyle;
      set
      {
        this.m_rollingStyle = value;
        switch (this.m_rollingStyle)
        {
          case RollingFileAppender.RollingMode.Once:
            this.m_rollDate = false;
            this.m_rollSize = false;
            this.AppendToFile = false;
            break;
          case RollingFileAppender.RollingMode.Size:
            this.m_rollDate = false;
            this.m_rollSize = true;
            break;
          case RollingFileAppender.RollingMode.Date:
            this.m_rollDate = true;
            this.m_rollSize = false;
            break;
          case RollingFileAppender.RollingMode.Composite:
            this.m_rollDate = true;
            this.m_rollSize = true;
            break;
        }
      }
    }

    public bool StaticLogFileName
    {
      get => this.m_staticLogFileName;
      set => this.m_staticLogFileName = value;
    }

    protected override void SetQWForFiles(TextWriter writer) => this.QuietWriter = (QuietTextWriter) new CountingQuietTextWriter(writer, this.ErrorHandler);

    protected override void Append(LoggingEvent loggingEvent)
    {
      this.AdjustFileBeforeAppend();
      base.Append(loggingEvent);
    }

    protected override void Append(LoggingEvent[] loggingEvents)
    {
      this.AdjustFileBeforeAppend();
      base.Append(loggingEvents);
    }

    protected virtual void AdjustFileBeforeAppend()
    {
      if (this.m_rollDate)
      {
        DateTime now = this.m_dateTime.Now;
        if (now >= this.m_nextCheck)
        {
          this.m_now = now;
          this.m_nextCheck = this.NextCheckDate(this.m_now, this.m_rollPoint);
          this.RollOverTime(true);
        }
      }
      if (!this.m_rollSize || this.File == null || ((CountingQuietTextWriter) this.QuietWriter).Count < this.m_maxFileSize)
        return;
      this.RollOverSize();
    }

    protected override void OpenFile(string fileName, bool append)
    {
      lock (this)
      {
        fileName = this.GetNextOutputFileName(fileName);
        long num = 0;
        if (append)
        {
          using (this.SecurityContext.Impersonate((object) this))
          {
            if (File.Exists(fileName))
              num = new FileInfo(fileName).Length;
          }
        }
        else if (LogLog.IsErrorEnabled && this.m_maxSizeRollBackups != 0 && this.FileExists(fileName))
          LogLog.Error("RollingFileAppender: INTERNAL ERROR. Append is False but OutputFile [" + fileName + "] already exists.");
        if (!this.m_staticLogFileName)
          this.m_scheduledFilename = fileName;
        base.OpenFile(fileName, append);
        ((CountingQuietTextWriter) this.QuietWriter).Count = num;
      }
    }

    protected string GetNextOutputFileName(string fileName)
    {
      if (!this.m_staticLogFileName)
      {
        fileName = fileName.Trim();
        if (this.m_rollDate)
          fileName += this.m_now.ToString(this.m_datePattern, (IFormatProvider) DateTimeFormatInfo.InvariantInfo);
        if (this.m_countDirection >= 0)
          fileName = fileName + (object) '.' + (object) this.m_curSizeRollBackups;
      }
      return fileName;
    }

    private void DetermineCurSizeRollBackups()
    {
      this.m_curSizeRollBackups = 0;
      string str = (string) null;
      string baseFile = (string) null;
      using (this.SecurityContext.Impersonate((object) this))
      {
        str = Path.GetFullPath(this.m_baseFileName);
        baseFile = Path.GetFileName(str);
      }
      ArrayList existingFiles = this.GetExistingFiles(str);
      this.InitializeRollBackups(baseFile, existingFiles);
      LogLog.Debug("RollingFileAppender: curSizeRollBackups starts at [" + (object) this.m_curSizeRollBackups + "]");
    }

    private static string GetWildcardPatternForFile(string baseFileName) => baseFileName + (object) '*';

    private ArrayList GetExistingFiles(string baseFilePath)
    {
      ArrayList existingFiles = new ArrayList();
      string path = (string) null;
      using (this.SecurityContext.Impersonate((object) this))
      {
        string fullPath = Path.GetFullPath(baseFilePath);
        path = Path.GetDirectoryName(fullPath);
        if (Directory.Exists(path))
        {
          string fileName1 = Path.GetFileName(fullPath);
          string[] files = Directory.GetFiles(path, RollingFileAppender.GetWildcardPatternForFile(fileName1));
          if (files != null)
          {
            for (int index = 0; index < files.Length; ++index)
            {
              string fileName2 = Path.GetFileName(files[index]);
              if (fileName2.StartsWith(fileName1))
                existingFiles.Add((object) fileName2);
            }
          }
        }
      }
      LogLog.Debug("RollingFileAppender: Searched for existing files in [" + path + "]");
      return existingFiles;
    }

    private void RollOverIfDateBoundaryCrossing()
    {
      if (!this.m_staticLogFileName || !this.m_rollDate || !this.FileExists(this.m_baseFileName))
        return;
      DateTime lastWriteTime;
      using (this.SecurityContext.Impersonate((object) this))
        lastWriteTime = File.GetLastWriteTime(this.m_baseFileName);
      LogLog.Debug("RollingFileAppender: [" + lastWriteTime.ToString(this.m_datePattern, (IFormatProvider) DateTimeFormatInfo.InvariantInfo) + "] vs. [" + this.m_now.ToString(this.m_datePattern, (IFormatProvider) DateTimeFormatInfo.InvariantInfo) + "]");
      if (lastWriteTime.ToString(this.m_datePattern, (IFormatProvider) DateTimeFormatInfo.InvariantInfo).Equals(this.m_now.ToString(this.m_datePattern, (IFormatProvider) DateTimeFormatInfo.InvariantInfo)))
        return;
      this.m_scheduledFilename = this.m_baseFileName + lastWriteTime.ToString(this.m_datePattern, (IFormatProvider) DateTimeFormatInfo.InvariantInfo);
      LogLog.Debug("RollingFileAppender: Initial roll over to [" + this.m_scheduledFilename + "]");
      this.RollOverTime(false);
      LogLog.Debug("RollingFileAppender: curSizeRollBackups after rollOver at [" + (object) this.m_curSizeRollBackups + "]");
    }

    protected void ExistingInit()
    {
      this.DetermineCurSizeRollBackups();
      this.RollOverIfDateBoundaryCrossing();
      if (this.AppendToFile)
        return;
      bool flag = false;
      string nextOutputFileName = this.GetNextOutputFileName(this.m_baseFileName);
      using (this.SecurityContext.Impersonate((object) this))
        flag = File.Exists(nextOutputFileName);
      if (!flag)
        return;
      if (this.m_maxSizeRollBackups == 0)
      {
        LogLog.Debug("RollingFileAppender: Output file [" + nextOutputFileName + "] already exists. MaxSizeRollBackups is 0; cannot roll. Overwriting existing file.");
      }
      else
      {
        LogLog.Debug("RollingFileAppender: Output file [" + nextOutputFileName + "] already exists. Not appending to file. Rolling existing file out of the way.");
        this.RollOverRenameFiles(nextOutputFileName);
      }
    }

    private void InitializeFromOneFile(string baseFile, string curFileName)
    {
      if (!curFileName.StartsWith(baseFile) || curFileName.Equals(baseFile))
        return;
      int num1 = curFileName.LastIndexOf(".");
      if (-1 == num1)
        return;
      if (this.m_staticLogFileName)
      {
        int num2 = curFileName.Length - num1;
        if (baseFile.Length + num2 != curFileName.Length)
          return;
      }
      if (this.m_rollDate)
      {
        if (!this.m_staticLogFileName)
        {
          if (!curFileName.StartsWith(baseFile + this.m_dateTime.Now.ToString(this.m_datePattern, (IFormatProvider) DateTimeFormatInfo.InvariantInfo)))
          {
            LogLog.Debug("RollingFileAppender: Ignoring file [" + curFileName + "] because it is from a different date period");
            return;
          }
        }
      }
      try
      {
        int val;
        if (!SystemInfo.TryParse(curFileName.Substring(num1 + 1), out val) || val <= this.m_curSizeRollBackups)
          return;
        if (this.m_maxSizeRollBackups != 0)
        {
          if (-1 == this.m_maxSizeRollBackups)
            this.m_curSizeRollBackups = val;
          else if (this.m_countDirection >= 0)
            this.m_curSizeRollBackups = val;
          else if (val <= this.m_maxSizeRollBackups)
            this.m_curSizeRollBackups = val;
        }
        LogLog.Debug("RollingFileAppender: File name [" + curFileName + "] moves current count to [" + (object) this.m_curSizeRollBackups + "]");
      }
      catch (FormatException ex)
      {
        LogLog.Debug("RollingFileAppender: Encountered a backup file not ending in .x [" + curFileName + "]");
      }
    }

    private void InitializeRollBackups(string baseFile, ArrayList arrayFiles)
    {
      if (arrayFiles == null)
        return;
      string lower = baseFile.ToLower(CultureInfo.InvariantCulture);
      foreach (string arrayFile in arrayFiles)
        this.InitializeFromOneFile(lower, arrayFile.ToLower(CultureInfo.InvariantCulture));
    }

    private RollingFileAppender.RollPoint ComputeCheckPeriod(string datePattern)
    {
      string str1 = RollingFileAppender.s_date1970.ToString(datePattern, (IFormatProvider) DateTimeFormatInfo.InvariantInfo);
      for (int checkPeriod = 0; checkPeriod <= 5; ++checkPeriod)
      {
        string str2 = this.NextCheckDate(RollingFileAppender.s_date1970, (RollingFileAppender.RollPoint) checkPeriod).ToString(datePattern, (IFormatProvider) DateTimeFormatInfo.InvariantInfo);
        LogLog.Debug("RollingFileAppender: Type = [" + (object) checkPeriod + "], r0 = [" + str1 + "], r1 = [" + str2 + "]");
        if (str1 != null && str2 != null && !str1.Equals(str2))
          return (RollingFileAppender.RollPoint) checkPeriod;
      }
      return RollingFileAppender.RollPoint.InvalidRollPoint;
    }

    public override void ActivateOptions()
    {
      if (this.m_rollDate && this.m_datePattern != null)
      {
        this.m_now = this.m_dateTime.Now;
        this.m_rollPoint = this.ComputeCheckPeriod(this.m_datePattern);
        this.m_nextCheck = this.m_rollPoint != RollingFileAppender.RollPoint.InvalidRollPoint ? this.NextCheckDate(this.m_now, this.m_rollPoint) : throw new ArgumentException("Invalid RollPoint, unable to parse [" + this.m_datePattern + "]");
      }
      else if (this.m_rollDate)
        this.ErrorHandler.Error("Either DatePattern or rollingStyle options are not set for [" + this.Name + "].");
      if (this.SecurityContext == null)
        this.SecurityContext = SecurityContextProvider.DefaultProvider.CreateSecurityContext((object) this);
      using (this.SecurityContext.Impersonate((object) this))
      {
        this.File = FileAppender.ConvertToFullPath(this.File.Trim());
        this.m_baseFileName = this.File;
      }
      if (this.m_rollDate && this.File != null && this.m_scheduledFilename == null)
        this.m_scheduledFilename = this.File + this.m_now.ToString(this.m_datePattern, (IFormatProvider) DateTimeFormatInfo.InvariantInfo);
      this.ExistingInit();
      base.ActivateOptions();
    }

    protected void RollOverTime(bool fileIsOpen)
    {
      if (this.m_staticLogFileName)
      {
        if (this.m_datePattern == null)
        {
          this.ErrorHandler.Error("Missing DatePattern option in rollOver().");
          return;
        }
        string str = this.m_now.ToString(this.m_datePattern, (IFormatProvider) DateTimeFormatInfo.InvariantInfo);
        if (this.m_scheduledFilename.Equals(this.File + str))
        {
          this.ErrorHandler.Error("Compare " + this.m_scheduledFilename + " : " + this.File + str);
          return;
        }
        if (fileIsOpen)
          this.CloseFile();
        for (int index = 1; index <= this.m_curSizeRollBackups; ++index)
          this.RollFile(this.File + (object) '.' + (object) index, this.m_scheduledFilename + (object) '.' + (object) index);
        this.RollFile(this.File, this.m_scheduledFilename);
      }
      this.m_curSizeRollBackups = 0;
      this.m_scheduledFilename = this.File + this.m_now.ToString(this.m_datePattern, (IFormatProvider) DateTimeFormatInfo.InvariantInfo);
      if (!fileIsOpen)
        return;
      this.SafeOpenFile(this.m_baseFileName, false);
    }

    protected void RollFile(string fromFile, string toFile)
    {
      if (this.FileExists(fromFile))
      {
        this.DeleteFile(toFile);
        try
        {
          LogLog.Debug("RollingFileAppender: Moving [" + fromFile + "] -> [" + toFile + "]");
          using (this.SecurityContext.Impersonate((object) this))
            File.Move(fromFile, toFile);
        }
        catch (Exception ex)
        {
          this.ErrorHandler.Error("Exception while rolling file [" + fromFile + "] -> [" + toFile + "]", ex, ErrorCode.GenericFailure);
        }
      }
      else
        LogLog.Warn("RollingFileAppender: Cannot RollFile [" + fromFile + "] -> [" + toFile + "]. Source does not exist");
    }

    protected bool FileExists(string path)
    {
      using (this.SecurityContext.Impersonate((object) this))
        return File.Exists(path);
    }

    protected void DeleteFile(string fileName)
    {
      if (!this.FileExists(fileName))
        return;
      string path = fileName;
      string destFileName = fileName + "." + (object) Environment.TickCount + ".DeletePending";
      try
      {
        using (this.SecurityContext.Impersonate((object) this))
          File.Move(fileName, destFileName);
        path = destFileName;
      }
      catch (Exception ex)
      {
        LogLog.Debug("RollingFileAppender: Exception while moving file to be deleted [" + fileName + "] -> [" + destFileName + "]", ex);
      }
      try
      {
        using (this.SecurityContext.Impersonate((object) this))
          File.Delete(path);
        LogLog.Debug("RollingFileAppender: Deleted file [" + fileName + "]");
      }
      catch (Exception ex)
      {
        if (path == fileName)
          this.ErrorHandler.Error("Exception while deleting file [" + path + "]", ex, ErrorCode.GenericFailure);
        else
          LogLog.Debug("RollingFileAppender: Exception while deleting temp file [" + path + "]", ex);
      }
    }

    protected void RollOverSize()
    {
      this.CloseFile();
      LogLog.Debug("RollingFileAppender: rolling over count [" + (object) ((CountingQuietTextWriter) this.QuietWriter).Count + "]");
      LogLog.Debug("RollingFileAppender: maxSizeRollBackups [" + (object) this.m_maxSizeRollBackups + "]");
      LogLog.Debug("RollingFileAppender: curSizeRollBackups [" + (object) this.m_curSizeRollBackups + "]");
      LogLog.Debug("RollingFileAppender: countDirection [" + (object) this.m_countDirection + "]");
      this.RollOverRenameFiles(this.File);
      if (!this.m_staticLogFileName && this.m_countDirection >= 0)
        ++this.m_curSizeRollBackups;
      this.SafeOpenFile(this.m_baseFileName, false);
    }

    protected void RollOverRenameFiles(string baseFileName)
    {
      if (this.m_maxSizeRollBackups == 0)
        return;
      if (this.m_countDirection < 0)
      {
        if (this.m_curSizeRollBackups == this.m_maxSizeRollBackups)
        {
          this.DeleteFile(baseFileName + (object) '.' + (object) this.m_maxSizeRollBackups);
          --this.m_curSizeRollBackups;
        }
        for (int curSizeRollBackups = this.m_curSizeRollBackups; curSizeRollBackups >= 1; --curSizeRollBackups)
          this.RollFile(baseFileName + "." + (object) curSizeRollBackups, baseFileName + (object) '.' + (object) (curSizeRollBackups + 1));
        ++this.m_curSizeRollBackups;
        this.RollFile(baseFileName, baseFileName + ".1");
      }
      else
      {
        if (this.m_curSizeRollBackups >= this.m_maxSizeRollBackups && this.m_maxSizeRollBackups > 0)
        {
          int num = this.m_curSizeRollBackups - this.m_maxSizeRollBackups;
          if (this.m_staticLogFileName)
            ++num;
          string str = baseFileName;
          if (!this.m_staticLogFileName)
          {
            int length = str.LastIndexOf(".");
            if (length >= 0)
              str = str.Substring(0, length);
          }
          this.DeleteFile(str + (object) '.' + (object) num);
        }
        if (!this.m_staticLogFileName)
          return;
        ++this.m_curSizeRollBackups;
        this.RollFile(baseFileName, baseFileName + (object) '.' + (object) this.m_curSizeRollBackups);
      }
    }

    protected DateTime NextCheckDate(
      DateTime currentDateTime,
      RollingFileAppender.RollPoint rollPoint)
    {
      DateTime dateTime = currentDateTime;
      switch (rollPoint)
      {
        case RollingFileAppender.RollPoint.TopOfMinute:
          dateTime = dateTime.AddMilliseconds((double) -dateTime.Millisecond);
          dateTime = dateTime.AddSeconds((double) -dateTime.Second);
          dateTime = dateTime.AddMinutes(1.0);
          break;
        case RollingFileAppender.RollPoint.TopOfHour:
          dateTime = dateTime.AddMilliseconds((double) -dateTime.Millisecond);
          dateTime = dateTime.AddSeconds((double) -dateTime.Second);
          dateTime = dateTime.AddMinutes((double) -dateTime.Minute);
          dateTime = dateTime.AddHours(1.0);
          break;
        case RollingFileAppender.RollPoint.HalfDay:
          dateTime = dateTime.AddMilliseconds((double) -dateTime.Millisecond);
          dateTime = dateTime.AddSeconds((double) -dateTime.Second);
          dateTime = dateTime.AddMinutes((double) -dateTime.Minute);
          if (dateTime.Hour < 12)
          {
            dateTime = dateTime.AddHours((double) (12 - dateTime.Hour));
            break;
          }
          dateTime = dateTime.AddHours((double) -dateTime.Hour);
          dateTime = dateTime.AddDays(1.0);
          break;
        case RollingFileAppender.RollPoint.TopOfDay:
          dateTime = dateTime.AddMilliseconds((double) -dateTime.Millisecond);
          dateTime = dateTime.AddSeconds((double) -dateTime.Second);
          dateTime = dateTime.AddMinutes((double) -dateTime.Minute);
          dateTime = dateTime.AddHours((double) -dateTime.Hour);
          dateTime = dateTime.AddDays(1.0);
          break;
        case RollingFileAppender.RollPoint.TopOfWeek:
          dateTime = dateTime.AddMilliseconds((double) -dateTime.Millisecond);
          dateTime = dateTime.AddSeconds((double) -dateTime.Second);
          dateTime = dateTime.AddMinutes((double) -dateTime.Minute);
          dateTime = dateTime.AddHours((double) -dateTime.Hour);
          dateTime = dateTime.AddDays((double) (7 - dateTime.DayOfWeek));
          break;
        case RollingFileAppender.RollPoint.TopOfMonth:
          dateTime = dateTime.AddMilliseconds((double) -dateTime.Millisecond);
          dateTime = dateTime.AddSeconds((double) -dateTime.Second);
          dateTime = dateTime.AddMinutes((double) -dateTime.Minute);
          dateTime = dateTime.AddHours((double) -dateTime.Hour);
          dateTime = dateTime.AddDays((double) (1 - dateTime.Day));
          dateTime = dateTime.AddMonths(1);
          break;
      }
      return dateTime;
    }

    public enum RollingMode
    {
      Once,
      Size,
      Date,
      Composite,
    }

    protected enum RollPoint
    {
      InvalidRollPoint = -1, // 0xFFFFFFFF
      TopOfMinute = 0,
      TopOfHour = 1,
      HalfDay = 2,
      TopOfDay = 3,
      TopOfWeek = 4,
      TopOfMonth = 5,
    }

    public interface IDateTime
    {
      DateTime Now { get; }
    }

    private class DefaultDateTime : RollingFileAppender.IDateTime
    {
      public DateTime Now => DateTime.Now;
    }
  }
}
