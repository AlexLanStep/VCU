
using ETAS.EAS.Util.log4net;
using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Threading;

namespace ETAS.EAS.Util
{
  [Serializable]
  public class EASStatusCode
  {
    private static readonly IEASLog logger = EASLogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private int m_CodeValue;
    protected int m_CodeValueLowerBound = int.MinValue;
    protected int m_CodeValueUpperBound = int.MaxValue;
    private string m_resourceID;
    private string m_description;
    private int m_paramsCount;
    protected static int counter = 0;
    protected string m_resourceFileName = (string)null;
    public static EASStatusCode NONE = new EASStatusCode(EASStatusCode.counter, nameof(NONE), "No error code specified.");
    protected CultureInfo[] supportedCultures = new CultureInfo[2]
    {
      new CultureInfo("en"),
      new CultureInfo("de")
    };

    public EASStatusCode(string resourceID, string description)
      : this(++EASStatusCode.counter, resourceID, description)
    {
    }

    public EASStatusCode(string resourceID)
      : this(++EASStatusCode.counter, resourceID, (string)null)
    {
    }

    public EASStatusCode(int codeValue, string resourceID, string description)
    {
      if (codeValue < this.m_CodeValueLowerBound || codeValue > this.m_CodeValueUpperBound)
        throw new Exception("Code Value specified for StatusCode-Object exceeds lower bound or upper bound ranges ! ");
      this.m_CodeValue = codeValue;
      this.m_resourceID = resourceID;
      this.m_description = description;
      this.m_paramsCount = this.getNumberOfFormatParams(this.m_description);
      this.m_resourceFileName = this.getResourceFileName();
    }

    protected virtual string getResourceFileName() => this.GetType().FullName + "Description";

    public override bool Equals(object o)
    {
      bool flag = false;
      if (o != null && o is EASStatusCode)
      {
        EASStatusCode easStatusCode = (EASStatusCode)o;
        flag = this.getCodeValue() == easStatusCode.getCodeValue();
      }
      return flag;
    }

    private int getNumberOfFormatParams(string msg)
    {
      int numberOfFormatParams = 0;
      int startIndex = 0;
      while (true)
      {
        startIndex = msg.IndexOf("{" + (object)numberOfFormatParams + "}", startIndex);
        if (startIndex >= 0)
          ++numberOfFormatParams;
        else
          break;
      }
      return numberOfFormatParams;
    }

    public override int GetHashCode() => this.m_CodeValue;

    public CultureInfo[] getSupportedCultures() => this.supportedCultures;

    public string getDescription(object[] param) => this.getDescription(Thread.CurrentThread.CurrentCulture, param);

    public string getDescription(CultureInfo currentLocale, object[] param)
    {
      EASStatusCode.logger.entering("getDescription(CultureInfo, Object[])");
      ResourceSet resourceSet = (ResourceSet)null;
      try
      {
        ResourceManager resourceManager = new ResourceManager(this.m_resourceFileName, this.GetType().Assembly);
        if (resourceManager != null)
          resourceSet = resourceManager.GetResourceSet(currentLocale, false, false);
      }
      catch (Exception ex)
      {
        EASStatusCode.logger.warning("getDescription(CultureInfo, Object[])", (object)ex.Message);
      }
      string format = this.getDescription();
      if (resourceSet != null)
      {
        EASStatusCode.logger.debug("getDescription(CultureInfo, Object[])", (object)"Retrieving from Resourcefile");
        try
        {
          format = resourceSet.GetString(this.m_resourceID);
        }
        catch (Exception ex)
        {
          EASStatusCode.logger.throwing("getDescription(CultureInfo, Object[])", ex);
          format = this.getDescription();
        }
        try
        {
          if (param != null)
            format = string.Format(format, param);
        }
        catch (Exception ex)
        {
          EASStatusCode.logger.throwing("getDescription(CultureInfo, Object[])", ex);
          format = this.getDescription();
        }
      }
      else
      {
        EASStatusCode.logger.debug("getDescription(CultureInfo, Object[])", (object)"Retrieving from description property");
        try
        {
          if (param != null)
            format = string.Format(format, param);
        }
        catch (Exception ex)
        {
          EASStatusCode.logger.throwing("getDescription(CultureInfo, Object[])", ex);
          format = this.getDescription();
        }
      }
      EASStatusCode.logger.exiting("getDescription(CultureInfo, Object[])");
      return format;
    }

    public int getCodeValue() => this.m_CodeValue;

    public string getResourceID() => this.m_resourceID;

    public string getDescription() => this.m_description;

    public void setDescription(string description) => this.m_description = description;

    public void setResourceID(string resID) => this.m_resourceID = resID;

    public void setCodeValue(int codeValue) => this.m_CodeValue = codeValue;

    public int getParamsCount() => this.m_paramsCount;
  }
}
