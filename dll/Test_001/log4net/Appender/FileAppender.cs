// Decompiled with JetBrains decompiler
// Type: log4net.Appender.FileAppender
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using log4net.Core;
using log4net.Layout;
using log4net.Util;
using System;
using System.IO;
using System.Text;

namespace log4net.Appender
{
  public class FileAppender : TextWriterAppender
  {
    private bool m_appendToFile = true;
    private string m_fileName = (string) null;
    private Encoding m_encoding = Encoding.Default;
    private SecurityContext m_securityContext;
    private FileAppender.LockingStream m_stream = (FileAppender.LockingStream) null;
    private FileAppender.LockingModelBase m_lockingModel = (FileAppender.LockingModelBase) new FileAppender.ExclusiveLock();

    public FileAppender()
    {
    }

    [Obsolete("Instead use the default constructor and set the Layout, File & AppendToFile properties")]
    public FileAppender(ILayout layout, string filename, bool append)
    {
      this.Layout = layout;
      this.File = filename;
      this.AppendToFile = append;
      this.ActivateOptions();
    }

    [Obsolete("Instead use the default constructor and set the Layout & File properties")]
    public FileAppender(ILayout layout, string filename)
      : this(layout, filename, true)
    {
    }

    public virtual string File
    {
      get => this.m_fileName;
      set => this.m_fileName = value;
    }

    public bool AppendToFile
    {
      get => this.m_appendToFile;
      set => this.m_appendToFile = value;
    }

    public Encoding Encoding
    {
      get => this.m_encoding;
      set => this.m_encoding = value;
    }

    public SecurityContext SecurityContext
    {
      get => this.m_securityContext;
      set => this.m_securityContext = value;
    }

    public FileAppender.LockingModelBase LockingModel
    {
      get => this.m_lockingModel;
      set => this.m_lockingModel = value;
    }

    public override void ActivateOptions()
    {
      base.ActivateOptions();
      if (this.m_securityContext == null)
        this.m_securityContext = SecurityContextProvider.DefaultProvider.CreateSecurityContext((object) this);
      if (this.m_lockingModel == null)
        this.m_lockingModel = (FileAppender.LockingModelBase) new FileAppender.ExclusiveLock();
      this.m_lockingModel.CurrentAppender = this;
      using (this.SecurityContext.Impersonate((object) this))
        this.m_fileName = FileAppender.ConvertToFullPath(this.m_fileName.Trim());
      if (this.m_fileName != null)
      {
        this.SafeOpenFile(this.m_fileName, this.m_appendToFile);
      }
      else
      {
        LogLog.Warn("FileAppender: File option not set for appender [" + this.Name + "].");
        LogLog.Warn("FileAppender: Are you using FileAppender instead of ConsoleAppender?");
      }
    }

    protected override void Reset()
    {
      base.Reset();
      this.m_fileName = (string) null;
    }

    protected override void PrepareWriter() => this.SafeOpenFile(this.m_fileName, this.m_appendToFile);

    protected override void Append(LoggingEvent loggingEvent)
    {
      if (!this.m_stream.AcquireLock())
        return;
      try
      {
        base.Append(loggingEvent);
      }
      finally
      {
        this.m_stream.ReleaseLock();
      }
    }

    protected override void Append(LoggingEvent[] loggingEvents)
    {
      if (!this.m_stream.AcquireLock())
        return;
      try
      {
        base.Append(loggingEvents);
      }
      finally
      {
        this.m_stream.ReleaseLock();
      }
    }

    protected override void WriteFooter()
    {
      if (this.m_stream == null)
        return;
      this.m_stream.AcquireLock();
      try
      {
        base.WriteFooter();
      }
      finally
      {
        this.m_stream.ReleaseLock();
      }
    }

    protected override void WriteHeader()
    {
      if (this.m_stream == null)
        return;
      if (!this.m_stream.AcquireLock())
        return;
      try
      {
        base.WriteHeader();
      }
      finally
      {
        this.m_stream.ReleaseLock();
      }
    }

    protected override void CloseWriter()
    {
      if (this.m_stream == null)
        return;
      this.m_stream.AcquireLock();
      try
      {
        base.CloseWriter();
      }
      finally
      {
        this.m_stream.ReleaseLock();
      }
    }

    protected void CloseFile() => this.WriteFooterAndCloseWriter();

    protected virtual void SafeOpenFile(string fileName, bool append)
    {
      try
      {
        this.OpenFile(fileName, append);
      }
      catch (Exception ex)
      {
        this.ErrorHandler.Error("OpenFile(" + fileName + "," + (object) append + ") call failed.", ex, ErrorCode.FileOpenFailure);
      }
    }

    protected virtual void OpenFile(string fileName, bool append)
    {
      if (LogLog.IsErrorEnabled)
      {
        bool flag = false;
        using (this.SecurityContext.Impersonate((object) this))
          flag = Path.IsPathRooted(fileName);
        if (!flag)
          LogLog.Error("FileAppender: INTERNAL ERROR. OpenFile(" + fileName + "): File name is not fully qualified.");
      }
      lock (this)
      {
        this.Reset();
        LogLog.Debug("FileAppender: Opening file for writing [" + fileName + "] append [" + (object) append + "]");
        this.m_fileName = fileName;
        this.m_appendToFile = append;
        this.LockingModel.CurrentAppender = this;
        this.LockingModel.OpenFile(fileName, append, this.m_encoding);
        this.m_stream = new FileAppender.LockingStream(this.LockingModel);
        if (this.m_stream != null)
        {
          this.m_stream.AcquireLock();
          try
          {
            this.SetQWForFiles((TextWriter) new StreamWriter((Stream) this.m_stream, this.m_encoding));
          }
          finally
          {
            this.m_stream.ReleaseLock();
          }
        }
        this.WriteHeader();
      }
    }

    protected virtual void SetQWForFiles(Stream fileStream) => this.SetQWForFiles((TextWriter) new StreamWriter(fileStream, this.m_encoding));

    protected virtual void SetQWForFiles(TextWriter writer) => this.QuietWriter = new QuietTextWriter(writer, this.ErrorHandler);

    protected static string ConvertToFullPath(string path) => SystemInfo.ConvertToFullPath(path);

    private sealed class LockingStream : Stream, IDisposable
    {
      private Stream m_realStream = (Stream) null;
      private FileAppender.LockingModelBase m_lockingModel = (FileAppender.LockingModelBase) null;
      private int m_readTotal = -1;
      private int m_lockLevel = 0;

      public LockingStream(FileAppender.LockingModelBase locking) => this.m_lockingModel = locking != null ? locking : throw new ArgumentException("Locking model may not be null", nameof (locking));

      public override IAsyncResult BeginRead(
        byte[] buffer,
        int offset,
        int count,
        AsyncCallback callback,
        object state)
      {
        this.AssertLocked();
        IAsyncResult asyncResult = this.m_realStream.BeginRead(buffer, offset, count, callback, state);
        this.m_readTotal = this.EndRead(asyncResult);
        return asyncResult;
      }

      public override IAsyncResult BeginWrite(
        byte[] buffer,
        int offset,
        int count,
        AsyncCallback callback,
        object state)
      {
        this.AssertLocked();
        IAsyncResult asyncResult = this.m_realStream.BeginWrite(buffer, offset, count, callback, state);
        this.EndWrite(asyncResult);
        return asyncResult;
      }

      public override void Close() => this.m_lockingModel.CloseFile();

      public override int EndRead(IAsyncResult asyncResult)
      {
        this.AssertLocked();
        return this.m_readTotal;
      }

      public override void EndWrite(IAsyncResult asyncResult)
      {
      }

      public override void Flush()
      {
        this.AssertLocked();
        this.m_realStream.Flush();
      }

      public override int Read(byte[] buffer, int offset, int count) => this.m_realStream.Read(buffer, offset, count);

      public override int ReadByte() => this.m_realStream.ReadByte();

      public override long Seek(long offset, SeekOrigin origin)
      {
        this.AssertLocked();
        return this.m_realStream.Seek(offset, origin);
      }

      public override void SetLength(long value)
      {
        this.AssertLocked();
        this.m_realStream.SetLength(value);
      }

      void IDisposable.Dispose() => this.Close();

      public override void Write(byte[] buffer, int offset, int count)
      {
        this.AssertLocked();
        this.m_realStream.Write(buffer, offset, count);
      }

      public override void WriteByte(byte value)
      {
        this.AssertLocked();
        this.m_realStream.WriteByte(value);
      }

      public override bool CanRead => false;

      public override bool CanSeek
      {
        get
        {
          this.AssertLocked();
          return this.m_realStream.CanSeek;
        }
      }

      public override bool CanWrite
      {
        get
        {
          this.AssertLocked();
          return this.m_realStream.CanWrite;
        }
      }

      public override long Length
      {
        get
        {
          this.AssertLocked();
          return this.m_realStream.Length;
        }
      }

      public override long Position
      {
        get
        {
          this.AssertLocked();
          return this.m_realStream.Position;
        }
        set
        {
          this.AssertLocked();
          this.m_realStream.Position = value;
        }
      }

      private void AssertLocked()
      {
        if (this.m_realStream == null)
          throw new FileAppender.LockingStream.LockStateException("The file is not currently locked");
      }

      public bool AcquireLock()
      {
        bool flag = false;
        lock (this)
        {
          if (this.m_lockLevel == 0)
            this.m_realStream = this.m_lockingModel.AcquireLock();
          if (this.m_realStream != null)
          {
            ++this.m_lockLevel;
            flag = true;
          }
        }
        return flag;
      }

      public void ReleaseLock()
      {
        lock (this)
        {
          --this.m_lockLevel;
          if (this.m_lockLevel != 0)
            return;
          this.m_lockingModel.ReleaseLock();
          this.m_realStream = (Stream) null;
        }
      }

      public sealed class LockStateException : LogException
      {
        public LockStateException(string message)
          : base(message)
        {
        }
      }
    }

    public abstract class LockingModelBase
    {
      private FileAppender m_appender = (FileAppender) null;

      public abstract void OpenFile(string filename, bool append, Encoding encoding);

      public abstract void CloseFile();

      public abstract Stream AcquireLock();

      public abstract void ReleaseLock();

      public FileAppender CurrentAppender
      {
        get => this.m_appender;
        set => this.m_appender = value;
      }
    }

    public class ExclusiveLock : FileAppender.LockingModelBase
    {
      private Stream m_stream = (Stream) null;

      public override void OpenFile(string filename, bool append, Encoding encoding)
      {
        try
        {
          using (this.CurrentAppender.SecurityContext.Impersonate((object) this))
          {
            string directoryName = Path.GetDirectoryName(filename);
            if (!Directory.Exists(directoryName))
              Directory.CreateDirectory(directoryName);
            FileMode mode = append ? FileMode.Append : FileMode.Create;
            this.m_stream = (Stream) new FileStream(filename, mode, FileAccess.Write, FileShare.Read);
          }
        }
        catch (Exception ex)
        {
          this.CurrentAppender.ErrorHandler.Error("Unable to acquire lock on file " + filename + ". " + ex.Message);
        }
      }

      public override void CloseFile()
      {
        using (this.CurrentAppender.SecurityContext.Impersonate((object) this))
          this.m_stream.Close();
      }

      public override Stream AcquireLock() => this.m_stream;

      public override void ReleaseLock()
      {
      }
    }

    public class MinimalLock : FileAppender.LockingModelBase
    {
      private string m_filename;
      private bool m_append;
      private Stream m_stream = (Stream) null;

      public override void OpenFile(string filename, bool append, Encoding encoding)
      {
        this.m_filename = filename;
        this.m_append = append;
      }

      public override void CloseFile()
      {
      }

      public override Stream AcquireLock()
      {
        if (this.m_stream == null)
        {
          try
          {
            using (this.CurrentAppender.SecurityContext.Impersonate((object) this))
            {
              string directoryName = Path.GetDirectoryName(this.m_filename);
              if (!Directory.Exists(directoryName))
                Directory.CreateDirectory(directoryName);
              this.m_stream = (Stream) new FileStream(this.m_filename, this.m_append ? FileMode.Append : FileMode.Create, FileAccess.Write, FileShare.Read);
              this.m_append = true;
            }
          }
          catch (Exception ex)
          {
            this.CurrentAppender.ErrorHandler.Error("Unable to acquire lock on file " + this.m_filename + ". " + ex.Message);
          }
        }
        return this.m_stream;
      }

      public override void ReleaseLock()
      {
        using (this.CurrentAppender.SecurityContext.Impersonate((object) this))
        {
          this.m_stream.Close();
          this.m_stream = (Stream) null;
        }
      }
    }
  }
}
