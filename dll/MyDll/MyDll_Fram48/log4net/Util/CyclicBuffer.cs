// Decompiled with JetBrains decompiler
// Type: log4net.Util.CyclicBuffer
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using log4net.Core;
using System;

namespace log4net.Util
{
  public class CyclicBuffer
  {
    private LoggingEvent[] m_events;
    private int m_first;
    private int m_last;
    private int m_numElems;
    private int m_maxSize;

    public CyclicBuffer(int maxSize)
    {
      this.m_maxSize = maxSize >= 1 ? maxSize : throw SystemInfo.CreateArgumentOutOfRangeException(nameof (maxSize), (object) maxSize, "Parameter: maxSize, Value: [" + (object) maxSize + "] out of range. Non zero positive integer required");
      this.m_events = new LoggingEvent[maxSize];
      this.m_first = 0;
      this.m_last = 0;
      this.m_numElems = 0;
    }

    public LoggingEvent Append(LoggingEvent loggingEvent)
    {
      if (loggingEvent == null)
        throw new ArgumentNullException(nameof (loggingEvent));
      lock (this)
      {
        LoggingEvent loggingEvent1 = this.m_events[this.m_last];
        this.m_events[this.m_last] = loggingEvent;
        if (++this.m_last == this.m_maxSize)
          this.m_last = 0;
        if (this.m_numElems < this.m_maxSize)
          ++this.m_numElems;
        else if (++this.m_first == this.m_maxSize)
          this.m_first = 0;
        return this.m_numElems < this.m_maxSize ? (LoggingEvent) null : loggingEvent1;
      }
    }

    public LoggingEvent PopOldest()
    {
      lock (this)
      {
        LoggingEvent loggingEvent = (LoggingEvent) null;
        if (this.m_numElems > 0)
        {
          --this.m_numElems;
          loggingEvent = this.m_events[this.m_first];
          this.m_events[this.m_first] = (LoggingEvent) null;
          if (++this.m_first == this.m_maxSize)
            this.m_first = 0;
        }
        return loggingEvent;
      }
    }

    public LoggingEvent[] PopAll()
    {
      lock (this)
      {
        LoggingEvent[] destinationArray = new LoggingEvent[this.m_numElems];
        if (this.m_numElems > 0)
        {
          if (this.m_first < this.m_last)
          {
            Array.Copy((Array) this.m_events, this.m_first, (Array) destinationArray, 0, this.m_numElems);
          }
          else
          {
            Array.Copy((Array) this.m_events, this.m_first, (Array) destinationArray, 0, this.m_maxSize - this.m_first);
            Array.Copy((Array) this.m_events, 0, (Array) destinationArray, this.m_maxSize - this.m_first, this.m_last);
          }
        }
        this.Clear();
        return destinationArray;
      }
    }

    public void Clear()
    {
      lock (this)
      {
        Array.Clear((Array) this.m_events, 0, this.m_events.Length);
        this.m_first = 0;
        this.m_last = 0;
        this.m_numElems = 0;
      }
    }

    public LoggingEvent this[int i]
    {
      get
      {
        lock (this)
          return i < 0 || i >= this.m_numElems ? (LoggingEvent) null : this.m_events[(this.m_first + i) % this.m_maxSize];
      }
    }

    public int MaxSize
    {
      get
      {
        lock (this)
          return this.m_maxSize;
      }
    }

    public int Length
    {
      get
      {
        lock (this)
          return this.m_numElems;
      }
    }
  }
}
