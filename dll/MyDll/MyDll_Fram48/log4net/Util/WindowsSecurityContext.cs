// Decompiled with JetBrains decompiler
// Type: log4net.Util.WindowsSecurityContext
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using log4net.Core;
using System;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace log4net.Util
{
  public class WindowsSecurityContext : SecurityContext, IOptionHandler
  {
    private WindowsSecurityContext.ImpersonationMode m_impersonationMode = WindowsSecurityContext.ImpersonationMode.User;
    private string m_userName;
    private string m_domainName = Environment.MachineName;
    private string m_password;
    private WindowsIdentity m_identity;

    public WindowsSecurityContext.ImpersonationMode Credentials
    {
      get => this.m_impersonationMode;
      set => this.m_impersonationMode = value;
    }

    public string UserName
    {
      get => this.m_userName;
      set => this.m_userName = value;
    }

    public string DomainName
    {
      get => this.m_domainName;
      set => this.m_domainName = value;
    }

    public string Password
    {
      set => this.m_password = value;
    }

    public void ActivateOptions()
    {
      if (this.m_impersonationMode != WindowsSecurityContext.ImpersonationMode.User)
        return;
      if (this.m_userName == null)
        throw new ArgumentNullException("m_userName");
      if (this.m_domainName == null)
        throw new ArgumentNullException("m_domainName");
      if (this.m_password == null)
        throw new ArgumentNullException("m_password");
      this.m_identity = WindowsSecurityContext.LogonUser(this.m_userName, this.m_domainName, this.m_password);
    }

    public override IDisposable Impersonate(object state)
    {
      if (this.m_impersonationMode == WindowsSecurityContext.ImpersonationMode.User)
      {
        if (this.m_identity != null)
          return (IDisposable) new WindowsSecurityContext.DisposableImpersonationContext(this.m_identity.Impersonate());
      }
      else if (this.m_impersonationMode == WindowsSecurityContext.ImpersonationMode.Process)
        return (IDisposable) new WindowsSecurityContext.DisposableImpersonationContext(WindowsIdentity.Impersonate(IntPtr.Zero));
      return (IDisposable) null;
    }

    private static WindowsIdentity LogonUser(
      string userName,
      string domainName,
      string password)
    {
      IntPtr zero1 = IntPtr.Zero;
      if (!WindowsSecurityContext.LogonUser(userName, domainName, password, 2, 0, ref zero1))
      {
        NativeError lastError = NativeError.GetLastError();
        throw new Exception("Failed to LogonUser [" + userName + "] in Domain [" + domainName + "]. Error: " + lastError.ToString());
      }
      IntPtr zero2 = IntPtr.Zero;
      if (!WindowsSecurityContext.DuplicateToken(zero1, 2, ref zero2))
      {
        NativeError lastError = NativeError.GetLastError();
        if (zero1 != IntPtr.Zero)
          WindowsSecurityContext.CloseHandle(zero1);
        throw new Exception("Failed to DuplicateToken after LogonUser. Error: " + lastError.ToString());
      }
      WindowsIdentity windowsIdentity = new WindowsIdentity(zero2);
      if (zero2 != IntPtr.Zero)
        WindowsSecurityContext.CloseHandle(zero2);
      if (zero1 != IntPtr.Zero)
        WindowsSecurityContext.CloseHandle(zero1);
      return windowsIdentity;
    }

    [DllImport("advapi32.dll", SetLastError = true)]
    private static extern bool LogonUser(
      string lpszUsername,
      string lpszDomain,
      string lpszPassword,
      int dwLogonType,
      int dwLogonProvider,
      ref IntPtr phToken);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
    private static extern bool CloseHandle(IntPtr handle);

    [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern bool DuplicateToken(
      IntPtr ExistingTokenHandle,
      int SECURITY_IMPERSONATION_LEVEL,
      ref IntPtr DuplicateTokenHandle);

    public enum ImpersonationMode
    {
      User,
      Process,
    }

    private sealed class DisposableImpersonationContext : IDisposable
    {
      private readonly WindowsImpersonationContext m_impersonationContext;

      public DisposableImpersonationContext(WindowsImpersonationContext impersonationContext) => this.m_impersonationContext = impersonationContext;

      public void Dispose() => this.m_impersonationContext.Undo();
    }
  }
}
