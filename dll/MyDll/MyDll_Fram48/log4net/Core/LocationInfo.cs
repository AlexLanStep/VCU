// Decompiled with JetBrains decompiler
// Type: log4net.Core.LocationInfo
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using log4net.Util;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Security;

namespace log4net.Core
{
  [Serializable]
  public class LocationInfo
  {
    private const string NA = "?";
    private readonly string m_className;
    private readonly string m_fileName;
    private readonly string m_lineNumber;
    private readonly string m_methodName;
    private readonly string m_fullInfo;

    public LocationInfo(Type callerStackBoundaryDeclaringType)
    {
      this.m_className = "?";
      this.m_fileName = "?";
      this.m_lineNumber = "?";
      this.m_methodName = "?";
      this.m_fullInfo = "?";
      if (callerStackBoundaryDeclaringType == null)
        return;
      try
      {
        StackTrace stackTrace = new StackTrace(true);
        int index;
        for (index = 0; index < stackTrace.FrameCount; ++index)
        {
          System.Diagnostics.StackFrame frame = stackTrace.GetFrame(index);
          if (frame != null && frame.GetMethod().DeclaringType == callerStackBoundaryDeclaringType)
            break;
        }
        for (; index < stackTrace.FrameCount; ++index)
        {
          System.Diagnostics.StackFrame frame = stackTrace.GetFrame(index);
          if (frame != null && frame.GetMethod().DeclaringType != callerStackBoundaryDeclaringType)
            break;
        }
        if (index >= stackTrace.FrameCount)
          return;
        System.Diagnostics.StackFrame frame1 = stackTrace.GetFrame(index);
        if (frame1 == null)
          return;
        MethodBase method = frame1.GetMethod();
        if (method != null)
        {
          this.m_methodName = method.Name;
          if (method.DeclaringType != null)
            this.m_className = method.DeclaringType.FullName;
        }
        this.m_fileName = frame1.GetFileName();
        this.m_lineNumber = frame1.GetFileLineNumber().ToString((IFormatProvider) NumberFormatInfo.InvariantInfo);
        this.m_fullInfo = this.m_className + (object) '.' + this.m_methodName + (object) '(' + this.m_fileName + (object) ':' + this.m_lineNumber + (object) ')';
      }
      catch (SecurityException ex)
      {
        LogLog.Debug("LocationInfo: Security exception while trying to get caller stack frame. Error Ignored. Location Information Not Available.");
      }
    }

    public LocationInfo(string className, string methodName, string fileName, string lineNumber)
    {
      this.m_className = className;
      this.m_fileName = fileName;
      this.m_lineNumber = lineNumber;
      this.m_methodName = methodName;
      this.m_fullInfo = this.m_className + (object) '.' + this.m_methodName + (object) '(' + this.m_fileName + (object) ':' + this.m_lineNumber + (object) ')';
    }

    public string ClassName => this.m_className;

    public string FileName => this.m_fileName;

    public string LineNumber => this.m_lineNumber;

    public string MethodName => this.m_methodName;

    public string FullInfo => this.m_fullInfo;
  }
}
