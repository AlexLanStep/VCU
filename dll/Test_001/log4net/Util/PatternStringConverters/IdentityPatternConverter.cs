// Decompiled with JetBrains decompiler
// Type: log4net.Util.PatternStringConverters.IdentityPatternConverter
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using System.IO;
using System.Security;
using System.Threading;

namespace log4net.Util.PatternStringConverters
{
  internal sealed class IdentityPatternConverter : PatternConverter
  {
    protected override void Convert(TextWriter writer, object state)
    {
      try
      {
        if (Thread.CurrentPrincipal == null || Thread.CurrentPrincipal.Identity == null || Thread.CurrentPrincipal.Identity.Name == null)
          return;
        writer.Write(Thread.CurrentPrincipal.Identity.Name);
      }
      catch (SecurityException ex)
      {
        LogLog.Debug("IdentityPatternConverter: Security exception while trying to get current thread principal. Error Ignored.");
        writer.Write(SystemInfo.NotAvailableText);
      }
    }
  }
}
