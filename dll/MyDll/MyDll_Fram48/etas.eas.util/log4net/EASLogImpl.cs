
using log4net;
using log4net.Core;
using System;
using System.Collections;
using System.Reflection.Emit;
using System.Text;

namespace ETAS.EAS.Util.log4net
{
  public class EASLogImpl : LogImpl, IEASLog, ILog, ILoggerWrapper
  {
    private Type targetType;

    public EASLogImpl(ILogger logger)
      : base(logger)
    {
    }

    private Type FullName => this.targetType;

    private string getObjectArrayString(object[] list)
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (list == null)
        return (string)null;
      for (int index = 0; index < list.Length; ++index)
      {
        if (index != 0)
          stringBuilder.Append(", ");
        if (list[index] != null)
        {
          if (list[index] is IDictionary)
          {
            IDictionaryEnumerator enumerator = ((IDictionary)list[index]).GetEnumerator();
            stringBuilder.Append("[");
            int num = 0;
            while (enumerator.MoveNext())
            {
              if (num != 0)
                stringBuilder.Append(", ");
              stringBuilder.Append(enumerator.Value);
              ++num;
            }
            stringBuilder.Append("]");
          }
          else
            stringBuilder.Append(list[index]);
        }
        else
          stringBuilder.Append("null");
      }
      return stringBuilder.ToString();
    }

    private int getNumberOfFormatParam(string msg)
    {
      int numberOfFormatParam = 0;
      int startIndex = 0;
      while (true)
      {
        startIndex = msg.IndexOf("{" + (object)numberOfFormatParam + "}", startIndex);
        if (startIndex >= 0)
          ++numberOfFormatParam;
        else
          break;
      }
      return numberOfFormatParam;
    }

    private string produceMessage(string sourceMethod, object msg, object[] list)
    {
      string str1 = "";
      object[] objArray = (object[])null;
      string str2 = str1 + sourceMethod;
      if (msg != null)
      {
        if (msg is string)
        {
          int numberOfFormatParam = this.getNumberOfFormatParam((string)msg);
          if (numberOfFormatParam != 0 && list != null)
          {
            if (numberOfFormatParam < list.Length)
            {
              object[] instance = (object[])Array.CreateInstance(typeof(object), numberOfFormatParam);
              objArray = (object[])Array.CreateInstance(typeof(object), list.Length - numberOfFormatParam);
              Array.Copy((Array)list, 0, (Array)instance, 0, numberOfFormatParam);
              Array.Copy((Array)list, numberOfFormatParam, (Array)objArray, 0, list.Length - numberOfFormatParam);
              str2 = str2 + ": " + string.Format((string)msg, instance);
            }
            else if (numberOfFormatParam == list.Length)
            {
              str2 = str2 + ": " + string.Format((string)msg, list);
            }
            else
            {
              object[] instance = (object[])Array.CreateInstance(typeof(object), numberOfFormatParam);
              Array.Copy((Array)list, 0, (Array)instance, 0, list.Length);
              for (int length = list.Length; length < numberOfFormatParam; ++length)
                instance.SetValue((object)"<undefined>", length);
              str2 = str2 + ": " + string.Format((string)msg, instance);
            }
          }
          else
          {
            str2 = str2 + ": " + msg;
            objArray = list;
          }
        }
        else
        {
          str2 = str2 + ": " + msg;
          objArray = list;
        }
      }
      else
        objArray = list;
      if (objArray != null)
        str2 = str2 + " (" + this.getObjectArrayString(objArray) + ")";
      return str2;
    }

    public void entering(string sourceMethod) => this.entering(sourceMethod, (object[])null);

    public void entering(string sourceMethod, params object[] arguments)
    {
      string msg = "ENTRY";
      this.Logger.Log(this.FullName, Level.Finer, (object)this.produceMessage(sourceMethod, (object)msg, arguments), (Exception)null);
    }

    public void exiting(string sourceMethod) => this.exiting(sourceMethod, (object[])null);

    public void exiting(string sourceMethod, params object[] arguments)
    {
      string msg = "RETURN";
      this.Logger.Log(this.FullName, Level.Finer, (object)this.produceMessage(sourceMethod, (object)msg, arguments), (Exception)null);
    }

    public void throwing(string sourceMethod, Exception t)
    {
      string msg = "THROWING";
      this.Logger.Log(this.FullName, Level.Severe, (object)this.produceMessage(sourceMethod, (object)msg, (object[])null), t);
    }

    public void logp(Level level, string sourceMethod, object msg, params object[] arguments)
    {
      if (!this.Logger.IsEnabledFor(level))
        return;
      this.Logger.Log(this.FullName, level, (object)this.produceMessage(sourceMethod, msg, arguments), (Exception)null);
    }

    public void finest(string sourceMethod, object msg)
    {
      if (!this.Logger.IsEnabledFor(Level.Finest))
        return;
      this.Logger.Log(this.FullName, Level.Finest, (object)this.produceMessage(sourceMethod, msg, (object[])null), (Exception)null);
    }

    public void finest(string sourceMethod, object msg, params object[] arguments)
    {
      if (!this.Logger.IsEnabledFor(Level.Finest))
        return;
      this.Logger.Log(this.FullName, Level.Finest, (object)this.produceMessage(sourceMethod, msg, arguments), (Exception)null);
    }

    public void finer(string sourceMethod, object msg)
    {
      if (!this.Logger.IsEnabledFor(Level.Finer))
        return;
      this.Logger.Log(this.FullName, Level.Finer, (object)this.produceMessage(sourceMethod, msg, (object[])null), (Exception)null);
    }

    public void finer(string sourceMethod, object msg, params object[] arguments)
    {
      if (!this.Logger.IsEnabledFor(Level.Finer))
        return;
      this.Logger.Log(this.FullName, Level.Finer, (object)this.produceMessage(sourceMethod, msg, arguments), (Exception)null);
    }

    public void fine(string sourceMethod, object msg)
    {
      if (!this.Logger.IsEnabledFor(Level.Fine))
        return;
      this.Logger.Log(this.FullName, Level.Fine, (object)this.produceMessage(sourceMethod, msg, (object[])null), (Exception)null);
    }

    public void fine(string sourceMethod, object msg, params object[] arguments)
    {
      if (!this.Logger.IsEnabledFor(Level.Fine))
        return;
      this.Logger.Log(this.FullName, Level.Fine, (object)this.produceMessage(sourceMethod, msg, arguments), (Exception)null);
    }

    public void debug(string sourceMethod, object msg)
    {
      if (!this.Logger.IsEnabledFor(Level.Debug))
        return;
      this.Logger.Log(this.FullName, Level.Debug, (object)this.produceMessage(sourceMethod, msg, (object[])null), (Exception)null);
    }

    public void debug(string sourceMethod, object msg, params object[] arguments)
    {
      if (!this.Logger.IsEnabledFor(Level.Debug))
        return;
      this.Logger.Log(this.FullName, Level.Debug, (object)this.produceMessage(sourceMethod, msg, arguments), (Exception)null);
    }

    public void info(string sourceMethod, object msg)
    {
      if (!this.Logger.IsEnabledFor(Level.Info))
        return;
      this.Logger.Log(this.FullName, Level.Info, (object)this.produceMessage(sourceMethod, msg, (object[])null), (Exception)null);
    }

    public void info(string sourceMethod, object msg, params object[] arguments)
    {
      if (!this.Logger.IsEnabledFor(Level.Info))
        return;
      this.Logger.Log(this.FullName, Level.Info, (object)this.produceMessage(sourceMethod, msg, arguments), (Exception)null);
    }

    public void warning(string sourceMethod, object msg)
    {
      if (!this.Logger.IsEnabledFor(Level.Warn))
        return;
      this.Logger.Log(this.FullName, Level.Warn, (object)this.produceMessage(sourceMethod, msg, (object[])null), (Exception)null);
    }

    public void warning(string sourceMethod, object msg, params object[] arguments)
    {
      if (!this.Logger.IsEnabledFor(Level.Warn))
        return;
      this.Logger.Log(this.FullName, Level.Warn, (object)this.produceMessage(sourceMethod, msg, arguments), (Exception)null);
    }

    public void severe(string sourceMethod, object msg)
    {
      if (!this.Logger.IsEnabledFor(Level.Severe))
        return;
      this.Logger.Log(this.FullName, Level.Severe, (object)this.produceMessage(sourceMethod, msg, (object[])null), (Exception)null);
    }

    public void severe(string sourceMethod, object msg, params object[] arguments)
    {
      if (!this.Logger.IsEnabledFor(Level.Severe))
        return;
      this.Logger.Log(this.FullName, Level.Severe, (object)this.produceMessage(sourceMethod, msg, arguments), (Exception)null);
    }

    public bool isFinestEnabled() => this.Logger.IsEnabledFor(Level.Finest);

    public bool isFinerEnabled() => this.Logger.IsEnabledFor(Level.Finer);

    public bool isFineEnabled() => this.Logger.IsEnabledFor(Level.Fine);

    public bool isSevereEnabled() => this.Logger.IsEnabledFor(Level.Severe);
  }
}
