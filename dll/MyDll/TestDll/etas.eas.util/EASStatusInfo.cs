// Decompiled with JetBrains decompiler
// Type: ETAS.EAS.Util.EASStatusInfo
// Assembly: etas.eas.util, Version=1.0.0.0, Culture=neutral, PublicKeyToken=24eada690b1476fd
// MVID: C6C1AF2C-DE3A-40A9-BDCC-7E76498937A9
// Assembly location: E:\LabCar\BasaDll\etas.eas.util.dll


namespace ETAS.EAS.Util;

public class EASStatusInfo
{
  private EASStatusCode easStatusCode;
  private object[] args;

  public EASStatusInfo()
  {
  }

  public EASStatusInfo(EASStatusCode easStatusCode, params object[] args)
  {
    this.easStatusCode = easStatusCode;
    this.args = args;
  }

  public EASStatusInfo(EASStatusCode easStatusCode)
    : this(easStatusCode, (object[])null)
  {
  }

  public string getMessage() => this.easStatusCode.getDescription();

  public string getLocalizedMessage() => this.getLocalizedMessage(Thread.CurrentThread.CurrentCulture);

  public string getLocalizedMessage(CultureInfo currentLocale) => this.easStatusCode.getDescription(currentLocale, this.args);

  public void setStatusCode(EASStatusCode easStatusCode)
  {
    this.easStatusCode = easStatusCode;
    this.args = (object[])null;
  }

  public EASStatusCode getStatusCode() => this.easStatusCode;

  public void setStatusCode(EASStatusCode easStatusCode, params object[] args)
  {
    this.easStatusCode = easStatusCode;
    this.args = args;
  }

  public void setArguments(params object[] args) => this.args = args;

  public object[] getArguments() => this.args;
}
