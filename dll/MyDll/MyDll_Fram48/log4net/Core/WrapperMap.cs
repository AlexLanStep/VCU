// Decompiled with JetBrains decompiler
// Type: log4net.Core.WrapperMap
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using log4net.Repository;
using System;
using System.Collections;

namespace log4net.Core
{
  public class WrapperMap
  {
    private readonly Hashtable m_repositories = new Hashtable();
    private readonly WrapperCreationHandler m_createWrapperHandler;
    private readonly LoggerRepositoryShutdownEventHandler m_shutdownHandler;

    public WrapperMap(WrapperCreationHandler createWrapperHandler)
    {
      this.m_createWrapperHandler = createWrapperHandler;
      this.m_shutdownHandler = new LoggerRepositoryShutdownEventHandler(this.ILoggerRepository_Shutdown);
    }

    public virtual ILoggerWrapper GetWrapper(ILogger logger)
    {
      if (logger == null)
        return (ILoggerWrapper) null;
      lock (this)
      {
        Hashtable hashtable = (Hashtable) this.m_repositories[(object) logger.Repository];
        if (hashtable == null)
        {
          hashtable = new Hashtable();
          this.m_repositories[(object) logger.Repository] = (object) hashtable;
          logger.Repository.ShutdownEvent += this.m_shutdownHandler;
        }
        if (!(hashtable[(object) logger] is ILoggerWrapper newWrapperObject))
        {
          newWrapperObject = this.CreateNewWrapperObject(logger);
          hashtable[(object) logger] = (object) newWrapperObject;
        }
        return newWrapperObject;
      }
    }

    protected Hashtable Repositories => this.m_repositories;

    protected virtual ILoggerWrapper CreateNewWrapperObject(ILogger logger) => this.m_createWrapperHandler != null ? this.m_createWrapperHandler(logger) : (ILoggerWrapper) null;

    protected virtual void RepositoryShutdown(ILoggerRepository repository)
    {
      lock (this)
      {
        this.m_repositories.Remove((object) repository);
        repository.ShutdownEvent -= this.m_shutdownHandler;
      }
    }

    private void ILoggerRepository_Shutdown(object sender, EventArgs e)
    {
      if (!(sender is ILoggerRepository repository))
        return;
      this.RepositoryShutdown(repository);
    }
  }
}
