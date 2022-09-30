// Decompiled with JetBrains decompiler
// Type: log4net.Util.PatternStringConverters.ProcessIdPatternConverter
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using System.Diagnostics;
using System.IO;
using System.Security;

namespace log4net.Util.PatternStringConverters
{
  internal sealed class ProcessIdPatternConverter : PatternConverter
  {
    protected override void Convert(TextWriter writer, object state)
    {
      try
      {
        writer.Write(Process.GetCurrentProcess().Id);
      }
      catch (SecurityException ex)
      {
        LogLog.Debug("ProcessIdPatternConverter: Security exception while trying to get current process id. Error Ignored.");
        writer.Write(SystemInfo.NotAvailableText);
      }
    }
  }
}
