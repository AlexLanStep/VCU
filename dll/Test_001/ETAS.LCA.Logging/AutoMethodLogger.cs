// Decompiled with JetBrains decompiler
// Type: ETAS.LCA.Logging.AutoMethodLogger
// Assembly: ETAS.LCA.Logging, Version=1.0.0.0, Culture=neutral, PublicKeyToken=24eada690b1476fd
// MVID: 3E0EA91C-5947-4488-8BAF-799929BC7052
// Assembly location: E:\LabCar\BasaDll\ETAS.LCA.Logging.dll

using System;

namespace ETAS.LCA.Logging
{
  public class AutoMethodLogger : IDisposable
  {
    private bool m_bDisposed = false;
    private string m_strMethodName = "";
    private ILog m_pLogger = (ILog) null;

    public AutoMethodLogger(ILog pLogger, string strMethodName)
    {
      this.m_pLogger = pLogger;
      this.m_strMethodName = strMethodName;
      if (this.m_pLogger == null || this.m_strMethodName == null)
        return;
      this.m_pLogger.Entering(this.m_strMethodName);
    }

    ~AutoMethodLogger() => this.Dispose(false);

    private void Dispose(bool bDisposing)
    {
      if (!this.m_bDisposed && this.m_pLogger != null && this.m_strMethodName != null)
        this.m_pLogger.Exiting(this.m_strMethodName);
      this.m_bDisposed = true;
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }
  }
}
