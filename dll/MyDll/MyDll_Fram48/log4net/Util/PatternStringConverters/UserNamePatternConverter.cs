// Decompiled with JetBrains decompiler
// Type: log4net.Util.PatternStringConverters.UserNamePatternConverter
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using System.IO;
using System.Security;
using System.Security.Principal;

namespace log4net.Util.PatternStringConverters
{
  internal sealed class UserNamePatternConverter : PatternConverter
  {
    protected override void Convert(TextWriter writer, object state)
    {
      try
      {
        WindowsIdentity current = WindowsIdentity.GetCurrent();
        if (current == null || current.Name == null)
          return;
        writer.Write(current.Name);
      }
      catch (SecurityException ex)
      {
        LogLog.Debug("UserNamePatternConverter: Security exception while trying to get current windows identity. Error Ignored.");
        writer.Write(SystemInfo.NotAvailableText);
      }
    }
  }
}
