// Decompiled with JetBrains decompiler
// Type: ETAS.LCA.Logging.LogImpl
// Assembly: ETAS.LCA.Logging, Version=1.0.0.0, Culture=neutral, PublicKeyToken=24eada690b1476fd
// MVID: 3E0EA91C-5947-4488-8BAF-799929BC7052
// Assembly location: E:\LabCar\BasaDll\ETAS.LCA.Logging.dll

using log4net.Core;
using log4net.Repository;
using log4net.Util;
using System;
using System.Globalization;

namespace ETAS.LCA.Logging
{
  public class LogImpl : log4net.Core.LogImpl, ILog
  {
    protected const string EnteringIndicator = "ENTERING ";
    protected const string ExitingIndicator = "EXITING ";
    private static readonly Type ThisDeclaringType = typeof (LogImpl);
    private Level m_levelFine;
    private Level m_levelFiner;
    private Level m_levelFinest;

    public LogImpl(ILogger logger)
      : base(logger)
    {
    }

    protected override void ReloadLevels(ILoggerRepository repository)
    {
      base.ReloadLevels(repository);
      LevelMap levelMap = repository.LevelMap;
      this.m_levelFine = levelMap.LookupWithDefault(Level.Fine);
      this.m_levelFiner = levelMap.LookupWithDefault(Level.Finer);
      this.m_levelFinest = levelMap.LookupWithDefault(Level.Finest);
    }

    public bool IsFineEnabled => this.Logger.IsEnabledFor(this.m_levelFine);

    public bool IsFinerEnabled => this.Logger.IsEnabledFor(this.m_levelFiner);

    public bool IsFinestEnabled => this.Logger.IsEnabledFor(this.m_levelFinest);

    public void Entering(string method) => this.Fine((object) ("ENTERING " + method));

    public void EnteringFiner(string method) => this.Finer((object) ("ENTERING " + method));

    public void Exiting(string method) => this.Fine((object) ("EXITING " + method));

    public void ExitingFiner(string method) => this.Finer((object) ("EXITING " + method));

    public void EnteringFinest(string method) => this.Finest((object) ("ENTERING " + method));

    public void ExitingFinest(string method) => this.Finest((object) ("EXITING " + method));

    public void Fine(object message) => this.Logger.Log(LogImpl.ThisDeclaringType, this.m_levelFine, message, (Exception) null);

    public void Fine(object message, Exception exception) => this.Logger.Log(LogImpl.ThisDeclaringType, this.m_levelFine, message, exception);

    public void FineFormat(IFormatProvider provider, string format, params object[] args)
    {
      if (!this.IsFineEnabled)
        return;
      this.Logger.Log(LogImpl.ThisDeclaringType, this.m_levelFine, (object) new SystemStringFormat(provider, format, args), (Exception) null);
    }

    public void FineFormat(string format, params object[] args)
    {
      if (!this.IsFineEnabled)
        return;
      this.Logger.Log(LogImpl.ThisDeclaringType, this.m_levelFine, (object) new SystemStringFormat((IFormatProvider) CultureInfo.InvariantCulture, format, args), (Exception) null);
    }

    public void FineFormat(string format, object arg0)
    {
      if (!this.IsFineEnabled)
        return;
      this.Logger.Log(LogImpl.ThisDeclaringType, this.m_levelFine, (object) new SystemStringFormat((IFormatProvider) CultureInfo.InvariantCulture, format, new object[1]
      {
        arg0
      }), (Exception) null);
    }

    public void FineFormat(string format, object arg0, object arg1)
    {
      if (!this.IsFineEnabled)
        return;
      this.Logger.Log(LogImpl.ThisDeclaringType, this.m_levelFine, (object) new SystemStringFormat((IFormatProvider) CultureInfo.InvariantCulture, format, new object[2]
      {
        arg0,
        arg1
      }), (Exception) null);
    }

    public void FineFormat(string format, object arg0, object arg1, object arg2)
    {
      if (!this.IsFineEnabled)
        return;
      this.Logger.Log(LogImpl.ThisDeclaringType, this.m_levelFine, (object) new SystemStringFormat((IFormatProvider) CultureInfo.InvariantCulture, format, new object[3]
      {
        arg0,
        arg1,
        arg2
      }), (Exception) null);
    }

    public void Finer(object message) => this.Logger.Log(LogImpl.ThisDeclaringType, this.m_levelFiner, message, (Exception) null);

    public void Finer(object message, Exception exception) => this.Logger.Log(LogImpl.ThisDeclaringType, this.m_levelFiner, message, exception);

    public void FinerFormat(IFormatProvider provider, string format, params object[] args)
    {
      if (!this.IsFinerEnabled)
        return;
      this.Logger.Log(LogImpl.ThisDeclaringType, this.m_levelFiner, (object) new SystemStringFormat(provider, format, args), (Exception) null);
    }

    public void FinerFormat(string format, params object[] args)
    {
      if (!this.IsFinerEnabled)
        return;
      this.Logger.Log(LogImpl.ThisDeclaringType, this.m_levelFiner, (object) new SystemStringFormat((IFormatProvider) CultureInfo.InvariantCulture, format, args), (Exception) null);
    }

    public void FinerFormat(string format, object arg0)
    {
      if (!this.IsFinerEnabled)
        return;
      this.Logger.Log(LogImpl.ThisDeclaringType, this.m_levelFiner, (object) new SystemStringFormat((IFormatProvider) CultureInfo.InvariantCulture, format, new object[1]
      {
        arg0
      }), (Exception) null);
    }

    public void FinerFormat(string format, object arg0, object arg1)
    {
      if (!this.IsFinerEnabled)
        return;
      this.Logger.Log(LogImpl.ThisDeclaringType, this.m_levelFiner, (object) new SystemStringFormat((IFormatProvider) CultureInfo.InvariantCulture, format, new object[2]
      {
        arg0,
        arg1
      }), (Exception) null);
    }

    public void FinerFormat(string format, object arg0, object arg1, object arg2)
    {
      if (!this.IsFinerEnabled)
        return;
      this.Logger.Log(LogImpl.ThisDeclaringType, this.m_levelFiner, (object) new SystemStringFormat((IFormatProvider) CultureInfo.InvariantCulture, format, new object[3]
      {
        arg0,
        arg1,
        arg2
      }), (Exception) null);
    }

    public void Finest(object message) => this.Logger.Log(LogImpl.ThisDeclaringType, this.m_levelFinest, message, (Exception) null);

    public void Finest(object message, Exception exception) => this.Logger.Log(LogImpl.ThisDeclaringType, this.m_levelFinest, message, exception);

    public void FinestFormat(IFormatProvider provider, string format, params object[] args)
    {
      if (!this.IsFinestEnabled)
        return;
      this.Logger.Log(LogImpl.ThisDeclaringType, this.m_levelFinest, (object) new SystemStringFormat(provider, format, args), (Exception) null);
    }

    public void FinestFormat(string format, params object[] args)
    {
      if (!this.IsFinestEnabled)
        return;
      this.Logger.Log(LogImpl.ThisDeclaringType, this.m_levelFinest, (object) new SystemStringFormat((IFormatProvider) CultureInfo.InvariantCulture, format, args), (Exception) null);
    }

    public void FinestFormat(string format, object arg0)
    {
      if (!this.IsFinestEnabled)
        return;
      this.Logger.Log(LogImpl.ThisDeclaringType, this.m_levelFinest, (object) new SystemStringFormat((IFormatProvider) CultureInfo.InvariantCulture, format, new object[1]
      {
        arg0
      }), (Exception) null);
    }

    public void FinestFormat(string format, object arg0, object arg1)
    {
      if (!this.IsFinestEnabled)
        return;
      this.Logger.Log(LogImpl.ThisDeclaringType, this.m_levelFinest, (object) new SystemStringFormat((IFormatProvider) CultureInfo.InvariantCulture, format, new object[2]
      {
        arg0,
        arg1
      }), (Exception) null);
    }

    public void FinestFormat(string format, object arg0, object arg1, object arg2)
    {
      if (!this.IsFinestEnabled)
        return;
      this.Logger.Log(LogImpl.ThisDeclaringType, this.m_levelFinest, (object) new SystemStringFormat((IFormatProvider) CultureInfo.InvariantCulture, format, new object[3]
      {
        arg0,
        arg1,
        arg2
      }), (Exception) null);
    }
  }
}
