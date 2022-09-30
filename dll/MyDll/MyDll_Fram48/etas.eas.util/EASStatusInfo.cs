
using System.Globalization;
using System.Threading;

namespace ETAS.EAS.Util
{
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
}
