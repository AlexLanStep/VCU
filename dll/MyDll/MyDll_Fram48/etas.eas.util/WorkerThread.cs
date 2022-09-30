
using ETAS.EAS.Util.log4net;
using System;
using System.Collections;
using System.Reflection;
using System.Threading;

namespace ETAS.EAS.Util
{
  public class WorkerThread
  {
    private static readonly IEASLog logger = EASLogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    protected Queue m_fifo;
    protected Mutex m_mutex;
    protected ManualResetEvent m_event;
    protected Thread m_thread;
    private string name;
    private IWorkerThread m_workerThread;

    public WorkerThread(string name, IWorkerThread workerThread)
    {
      this.m_fifo = new Queue();
      this.m_mutex = new Mutex();
      this.m_event = new ManualResetEvent(false);
      this.m_thread = new Thread(new ThreadStart(this.run));
      this.m_workerThread = workerThread;
      this.m_thread.Name = name;
      this.name = name;
      this.m_thread.Start();
    }

    public void stop()
    {
      WorkerThread.logger.info("stop Thread", (object)"abort");
      try
      {
        this.m_thread.Abort();
        this.m_thread.Join(15000);
      }
      catch (Exception ex)
      {
        WorkerThread.logger.Error((object)("Stop Thread Exception : " + this.name), ex);
      }
    }

    public void addWork(object parameter)
    {
      this.m_mutex.WaitOne();
      this.m_fifo.Enqueue(parameter);
      this.m_mutex.ReleaseMutex();
      this.m_event.Set();
    }

    public void run()
    {
      try
      {
        while (true)
        {
          this.m_event.WaitOne();
          this.m_mutex.WaitOne();
          object parameter = this.m_fifo.Dequeue();
          if (this.m_fifo.Count == 0)
            this.m_event.Reset();
          this.m_mutex.ReleaseMutex();
          try
          {
            this.m_workerThread.scheduledInvoke(parameter);
          }
          catch (ThreadAbortException ex)
          {
            throw ex;
          }
          catch (Exception ex)
          {
            WorkerThread.logger.warning("scheduledInvoke failed: ", (object)ex.Message);
          }
        }
      }
      catch (ThreadAbortException ex)
      {
        WorkerThread.logger.Error((object)("Run Thread Exception : " + this.name), (Exception)ex);
      }
    }
  }
}
