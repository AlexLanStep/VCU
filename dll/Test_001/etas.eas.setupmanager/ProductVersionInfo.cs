// Decompiled with JetBrains decompiler
// Type: ETAS.EAS.ProductVersionInfo
// Assembly: etas.eas.setupmanager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=24eada690b1476fd
// MVID: 6F2445EF-C18D-4C24-B748-E3B896DC125F
// Assembly location: E:\LabCar\BasaDll\etas.eas.setupmanager.dll

using System.Collections;

namespace ETAS.EAS
{
  public class ProductVersionInfo
  {
    private string[] _InstalledTools;
    private string[] _InstalledVersions;
    private string[] _LicenseCodeNames;
    private string[] _LicenseKeys;

    public ProductVersionInfo()
    {
      try
      {
        this._InstalledTools = SetupManager.Instance.InstalledTools;
        ArrayList arrayList1 = new ArrayList();
        ArrayList arrayList2 = new ArrayList();
        ArrayList arrayList3 = new ArrayList();
        foreach (string installedTool in this._InstalledTools)
        {
          if (installedTool == "LABCAR-AUTOMATION")
          {
            arrayList1.Add((object) SetupManager.Instance.FullVersion);
            arrayList2.Add((object) "lcau");
            arrayList3.Add((object) SetupManager.Instance.GetLicenseKeyForProduct("lcau"));
          }
          else
          {
            string licenseCodeForAddon = SetupManager.Instance.GetLicenseCodeForAddon(installedTool);
            arrayList1.Add((object) SetupManager.Instance.GetFullVersionForAddon(installedTool));
            arrayList2.Add((object) licenseCodeForAddon);
            arrayList3.Add((object) SetupManager.Instance.GetLicenseKeyForProduct(licenseCodeForAddon));
          }
        }
        this._InstalledVersions = (string[]) arrayList1.ToArray(typeof (string));
        this._LicenseCodeNames = (string[]) arrayList2.ToArray(typeof (string));
        this._LicenseKeys = (string[]) arrayList3.ToArray(typeof (string));
      }
      catch
      {
      }
    }

    public string[] InstalledTools => this._InstalledTools;

    public string[] InstalledVersions => this._InstalledVersions;

    public string[] LicenseCodeNames => this._LicenseCodeNames;

    public string[] LicenseKeys => this._LicenseKeys;

    public string InfoString(ProductVersionInfo.InfoStringContent content)
    {
      string str = "LABCAR-AUTOMATION Product Information\r\n\r\n";
      switch (content)
      {
        case ProductVersionInfo.InfoStringContent.Toolname:
          str += "Product\r\n\r\n";
          break;
        case ProductVersionInfo.InfoStringContent.ToolnameAndVersion:
          str += "Product\t\tVersion\r\n\r\n";
          break;
        case ProductVersionInfo.InfoStringContent.All:
          str += "Product\t\tVersion\t\tLicense Key\r\n\r\n";
          break;
      }
      for (int index = 0; index < this._InstalledTools.Length; ++index)
      {
        switch (content)
        {
          case ProductVersionInfo.InfoStringContent.Toolname:
            str = str + this.InstalledTools[index] + "\r\n";
            break;
          case ProductVersionInfo.InfoStringContent.ToolnameAndVersion:
            str = str + this.InstalledTools[index] + "\t\t" + this.InstalledVersions[index] + "\r\n";
            break;
          case ProductVersionInfo.InfoStringContent.All:
            str = str + this.InstalledTools[index] + "\t\t" + this.InstalledVersions[index] + "\t\t" + this.LicenseKeys[index] + "\r\n";
            break;
        }
      }
      return str;
    }

    public enum InfoStringContent
    {
      Toolname,
      ToolnameAndVersion,
      All,
    }
  }
}
