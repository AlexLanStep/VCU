// Decompiled with JetBrains decompiler
// Type: log4net.Util.ReusableStringWriter
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using System;
using System.IO;
using System.Text;

namespace log4net.Util
{
  public class ReusableStringWriter : StringWriter
  {
    public ReusableStringWriter(IFormatProvider formatProvider)
      : base(formatProvider)
    {
    }

    protected override void Dispose(bool disposing)
    {
    }

    public void Reset(int maxCapacity, int defaultSize)
    {
      StringBuilder stringBuilder = this.GetStringBuilder();
      stringBuilder.Length = 0;
      if (stringBuilder.Capacity <= maxCapacity)
        return;
      stringBuilder.Capacity = defaultSize;
    }
  }
}
