
using ETAS.EAS.Util.log4net;
using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;

namespace ETAS.EAS.Util;

[Serializable]
public class EASException : Exception, ISerializable
{
  private static readonly IEASLog logger = EASLogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
  private EASStatusCode _errorCode;
  private object[] _args;
  private new string _message;

  public EASException(SerializationInfo info, StreamingContext context)
    : base(info, context)
  {
    this._errorCode = (EASStatusCode)info.GetValue(nameof(_errorCode), typeof(EASStatusCode));
    this._args = (object[])info.GetValue(nameof(_args), typeof(object[]));
    this._message = info.GetString(nameof(_message));
  }

  void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
  {
    this.GetObjectData(info, context);
    info.AddValue("_errorCode", (object)this._errorCode);
    info.AddValue("_args", (object)this._args);
    info.AddValue("_message", (object)this._message);
  }

  public EASException()
    : this(EASStatusCode.NONE, (Exception)null, (object[])null)
  {
  }

  public EASException(Exception innerException)
    : this(EASStatusCode.NONE, innerException, (object[])null)
  {
  }

  public EASException(EASStatusCode errorCode)
    : this(errorCode, (Exception)null, (object[])null)
  {
  }

  public EASException(EASStatusCode errorCode, params object[] args)
    : this(errorCode, (Exception)null, args)
  {
  }

  public EASException(EASStatusCode errorCode, Exception innerException)
    : this(errorCode, innerException, (object[])null)
  {
  }

  public EASException(EASStatusCode errorCode, Exception innerException, params object[] args)
    : base((string)null, innerException)
  {
    this._errorCode = errorCode;
    this._args = args;
  }

  public object[] getInformation() => this._args;

  public void setInformation(params object[] args) => this._args = args;

  public void setErrorCode(EASStatusCode errorCode) => this._errorCode = errorCode;

  public void setErrorCode(EASStatusCode errorCode, params object[] args)
  {
    this._errorCode = errorCode;
    this._args = args;
  }

  public EASStatusCode getErrorCode() => this._errorCode;

  public Exception getNestedException() => this.InnerException;

  public override string Message
  {
    get
    {
      if (this._message == null)
        this._message = EASException.getExceptionMsg((Exception)this);
      return this._message;
    }
  }

  public override string ToString()
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append(Environment.NewLine).Append("Exception: ").Append(((object)this).GetType().FullName).Append(Environment.NewLine).Append("Message: ").Append(this.Message).Append(Environment.NewLine);
    if (this.Source != null && this.Source.Length > 0)
      stringBuilder.Append("Source: ").Append(this.Source).Append(Environment.NewLine);
    if (this.StackTrace != null && this.StackTrace.Length > 0)
      stringBuilder.Append(this.StackTrace).Append(Environment.NewLine);
    Exception nestedException = this.getNestedException();
    if (nestedException != null)
      stringBuilder.Append(Environment.NewLine).Append("Nested Exception").Append(nestedException.ToString());
    return stringBuilder.ToString();
  }

  public void printStackTrace()
  {
    Exception nestedException = this.getNestedException();
    if (nestedException != null)
      Console.Error.WriteLine(nestedException.StackTrace);
    else
      Console.Error.WriteLine(this.StackTrace);
    if (this._args == null)
      return;
    Console.Error.WriteLine("Additional information: ");
    for (int index = 0; index < this._args.Length; ++index)
      Console.Error.WriteLine("  " + (object)index + ": " + this._args[index]);
  }

  public void printStackTrace(StreamWriter stream)
  {
    Exception nestedException = this.getNestedException();
    if (nestedException != null)
      stream.WriteLine(nestedException.StackTrace);
    else
      stream.WriteLine((object)stream);
    if (this._args == null)
      return;
    stream.WriteLine("Additional information: ");
    for (int index = 0; index < this._args.Length; ++index)
      stream.WriteLine("  " + (object)index + ": " + this._args[index]);
  }

  public string getMessage()
  {
    Exception nestedException = this.getNestedException();
    return nestedException == null ? (EASStatusCode.NONE.Equals((object)this.getErrorCode()) ? base.ToString() : this.getErrorCode().getDescription()) : nestedException.ToString();
  }

  public string getLocalizedMessage() => this.getLocalizedMessage(Thread.CurrentThread.CurrentCulture);

  public string getLocalizedMessage(CultureInfo currentLocale)
  {
    EASException.logger.entering("getLocalizedMessage(CultureInfo)", (object)currentLocale);
    Exception nestedException = this.getNestedException();
    string localizedMessage = nestedException == null ? (EASStatusCode.NONE.Equals((object)this.getErrorCode()) ? this.getMessage() : this.getErrorCode().getDescription(currentLocale, this.getInformation())) : (!(nestedException is EASException) ? nestedException.ToString() : ((EASException)nestedException).getLocalizedMessage(currentLocale));
    EASException.logger.exiting("getLocalizedMessage(CultureInfo)");
    return localizedMessage;
  }

  public static string getExceptionMsg(Exception e)
  {
    EASException.logger.entering("getExceptionMsg(Exception)");
    EASException.logger.finer("getExceptionMsg(Exception)", (object)("Exception is instance of " + (object)((object)e).GetType()));
    CultureInfo currentUiCulture = Thread.CurrentThread.CurrentUICulture;
    string exceptionMsg;
    switch (e)
    {
      case EASException _:
        EASException.logger.finer("getExceptionMsg(Exception)", (object)"processing EASException");
        exceptionMsg = EASException.resolveEASExceptionMsg((EASException)e);
        break;
      case TargetInvocationException _:
        EASException.logger.finer("getExceptionMsg(Exception)", (object)"processing TargetInvocationException");
        exceptionMsg = EASException.resolveTargetInvocationExceptionMsg((TargetInvocationException)e);
        break;
      default:
        exceptionMsg = EASException.resolveSystemSpecific(e);
        break;
    }
    EASException.logger.exiting("getExceptionMsg(Exception)");
    return exceptionMsg;
  }

  private static string resolveSystemSpecific(Exception e)
  {
    EASException.logger.entering("resolveSystemSpecific(Exception)", (object)((object)e).GetType());
    string str = e.ToString();
    EASException.logger.exiting("resolveSystemSpecific(Exception)");
    return str;
  }

  private static string resolveEASExceptionMsg(EASException e)
  {
    EASException.logger.entering("resolveEASExceptionMsg(EASException)");
    Exception nestedException = e.getNestedException();
    string str = (string)null;
    if (nestedException != null)
      str = EASException.getExceptionMsg(nestedException);
    if (str == null)
    {
      CultureInfo currentUiCulture = Thread.CurrentThread.CurrentUICulture;
      str = e.getLocalizedMessage(currentUiCulture);
    }
    EASException.logger.exiting("resolveEASExceptionMsg(EASException)");
    return str;
  }

  private static string resolveTargetInvocationExceptionMsg(TargetInvocationException e)
  {
    EASException.logger.entering("resolveInvocationTargetExceptionMsg()");
    Exception innerException = e.InnerException;
    string str = innerException == null ? e.ToString() : EASException.getExceptionMsg(innerException);
    EASException.logger.exiting("resolveInvocationTargetExceptionMsg()");
    return str;
  }
}
