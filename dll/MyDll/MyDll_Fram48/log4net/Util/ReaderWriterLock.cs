// Decompiled with JetBrains decompiler
// Type: log4net.Util.ReaderWriterLock
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

namespace log4net.Util
{
  public sealed class ReaderWriterLock
  {
    private System.Threading.ReaderWriterLock m_lock;

    public ReaderWriterLock() => this.m_lock = new System.Threading.ReaderWriterLock();

    public void AcquireReaderLock() => this.m_lock.AcquireReaderLock(-1);

    public void ReleaseReaderLock() => this.m_lock.ReleaseReaderLock();

    public void AcquireWriterLock() => this.m_lock.AcquireWriterLock(-1);

    public void ReleaseWriterLock() => this.m_lock.ReleaseWriterLock();
  }
}
