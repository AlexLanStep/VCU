// Decompiled with JetBrains decompiler
// Type: log4net.Appender.BufferingAppenderSkeleton
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using log4net.Core;
using log4net.Util;
using System;
using System.Collections;

namespace log4net.Appender
{
  public abstract class BufferingAppenderSkeleton : AppenderSkeleton
  {
    private const int DEFAULT_BUFFER_SIZE = 512;
    private int m_bufferSize = 512;
    private CyclicBuffer m_cb;
    private ITriggeringEventEvaluator m_evaluator;
    private bool m_lossy = false;
    private ITriggeringEventEvaluator m_lossyEvaluator;
    private FixFlags m_fixFlags = FixFlags.All;
    private readonly bool m_eventMustBeFixed;

    protected BufferingAppenderSkeleton()
      : this(true)
    {
    }

    protected BufferingAppenderSkeleton(bool eventMustBeFixed) => this.m_eventMustBeFixed = eventMustBeFixed;

    public bool Lossy
    {
      get => this.m_lossy;
      set => this.m_lossy = value;
    }

    public int BufferSize
    {
      get => this.m_bufferSize;
      set => this.m_bufferSize = value;
    }

    public ITriggeringEventEvaluator Evaluator
    {
      get => this.m_evaluator;
      set => this.m_evaluator = value;
    }

    public ITriggeringEventEvaluator LossyEvaluator
    {
      get => this.m_lossyEvaluator;
      set => this.m_lossyEvaluator = value;
    }

    [Obsolete("Use Fix property")]
    public virtual bool OnlyFixPartialEventData
    {
      get => this.Fix == FixFlags.Partial;
      set
      {
        if (value)
          this.Fix = FixFlags.Partial;
        else
          this.Fix = FixFlags.All;
      }
    }

    public virtual FixFlags Fix
    {
      get => this.m_fixFlags;
      set => this.m_fixFlags = value;
    }

    public virtual void Flush() => this.Flush(false);

    public virtual void Flush(bool flushLossyBuffer)
    {
      lock (this)
      {
        if (this.m_cb == null || this.m_cb.Length <= 0)
          return;
        if (this.m_lossy)
        {
          if (!flushLossyBuffer)
            return;
          if (this.m_lossyEvaluator != null)
          {
            LoggingEvent[] loggingEventArray = this.m_cb.PopAll();
            ArrayList arrayList = new ArrayList(loggingEventArray.Length);
            foreach (LoggingEvent loggingEvent in loggingEventArray)
            {
              if (this.m_lossyEvaluator.IsTriggeringEvent(loggingEvent))
                arrayList.Add((object) loggingEvent);
            }
            if (arrayList.Count <= 0)
              return;
            this.SendBuffer((LoggingEvent[]) arrayList.ToArray(typeof (LoggingEvent)));
          }
          else
            this.m_cb.Clear();
        }
        else
          this.SendFromBuffer((LoggingEvent) null, this.m_cb);
      }
    }

    public override void ActivateOptions()
    {
      base.ActivateOptions();
      if (this.m_lossy && this.m_evaluator == null)
        this.ErrorHandler.Error("Appender [" + this.Name + "] is Lossy but has no Evaluator. The buffer will never be sent!");
      if (this.m_bufferSize > 1)
        this.m_cb = new CyclicBuffer(this.m_bufferSize);
      else
        this.m_cb = (CyclicBuffer) null;
    }

    protected override void OnClose() => this.Flush(true);

    protected override void Append(LoggingEvent loggingEvent)
    {
      if (this.m_cb == null || this.m_bufferSize <= 1)
      {
        if (this.m_lossy && (this.m_evaluator == null || !this.m_evaluator.IsTriggeringEvent(loggingEvent)) && (this.m_lossyEvaluator == null || !this.m_lossyEvaluator.IsTriggeringEvent(loggingEvent)))
          return;
        if (this.m_eventMustBeFixed)
          loggingEvent.Fix = this.Fix;
        this.SendBuffer(new LoggingEvent[1]{ loggingEvent });
      }
      else
      {
        loggingEvent.Fix = this.Fix;
        LoggingEvent loggingEvent1 = this.m_cb.Append(loggingEvent);
        if (loggingEvent1 != null)
        {
          if (!this.m_lossy)
          {
            this.SendFromBuffer(loggingEvent1, this.m_cb);
          }
          else
          {
            if (this.m_lossyEvaluator == null || !this.m_lossyEvaluator.IsTriggeringEvent(loggingEvent1))
              loggingEvent1 = (LoggingEvent) null;
            if (this.m_evaluator != null && this.m_evaluator.IsTriggeringEvent(loggingEvent))
            {
              this.SendFromBuffer(loggingEvent1, this.m_cb);
            }
            else
            {
              if (loggingEvent1 == null)
                return;
              this.SendBuffer(new LoggingEvent[1]
              {
                loggingEvent1
              });
            }
          }
        }
        else
        {
          if (this.m_evaluator == null || !this.m_evaluator.IsTriggeringEvent(loggingEvent))
            return;
          this.SendFromBuffer((LoggingEvent) null, this.m_cb);
        }
      }
    }

    protected virtual void SendFromBuffer(LoggingEvent firstLoggingEvent, CyclicBuffer buffer)
    {
      LoggingEvent[] loggingEventArray1 = buffer.PopAll();
      if (firstLoggingEvent == null)
        this.SendBuffer(loggingEventArray1);
      else if (loggingEventArray1.Length == 0)
      {
        this.SendBuffer(new LoggingEvent[1]
        {
          firstLoggingEvent
        });
      }
      else
      {
        LoggingEvent[] loggingEventArray2 = new LoggingEvent[loggingEventArray1.Length + 1];
        Array.Copy((Array) loggingEventArray1, 0, (Array) loggingEventArray2, 1, loggingEventArray1.Length);
        loggingEventArray2[0] = firstLoggingEvent;
        this.SendBuffer(loggingEventArray2);
      }
    }

    protected abstract void SendBuffer(LoggingEvent[] events);
  }
}
