// Decompiled with JetBrains decompiler
// Type: ETAS.EAS.SetupManager
// Assembly: etas.eas.setupmanager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=24eada690b1476fd
// MVID: 6F2445EF-C18D-4C24-B748-E3B896DC125F
// Assembly location: E:\LabCar\BasaDll\etas.eas.setupmanager.dll

using Microsoft.Win32;
using System;
using System.Collections;
using System.Diagnostics;
using System.IO;

namespace ETAS.EAS
{
  public class SetupManager : ISetupManager
  {
    private const string M_REGISTRYROOT = "Software\\ETAS\\LABCAR-AUTOMATION";
    private const string M_PRODUCTNAME = "LABCAR-AUTOMATION";
    private const string M_LCLICENSESKEY = "Software\\ETAS\\General\\LCLicenses";
    private const string M_MANUALSKEY = "Software\\ETAS\\General";
    private const string M_CurrentTauTesterVersion = "CurrentTauTesterVersion";
    private const string M_CurrentTSVersion = "CurrentTSVersion";
    private const string M_CurrentVersion = "CurrentVersion";
    private const string NotInstalled = "none";
    private static BinaryVersionInfo binVersionInfo = new BinaryVersionInfo();

    private SetupManager()
    {
    }

    public static ISetupManager Instance => SetupManager.Nested.s_instance;

    public string ProductName => "LABCAR-AUTOMATION";

    public string CurrentLCAVersion
    {
      get
      {
        try
        {
          RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\ETAS\\LABCAR-AUTOMATION");
          string currentLcaVersion = (string) registryKey.GetValue("CurrentVersion", (object) "-1");
          registryKey.Close();
          return currentLcaVersion;
        }
        catch (Exception ex)
        {
          return "Product not found in Registry. " + ex.Message;
        }
      }
    }

    public string CurrentTauTesterVersion
    {
      get
      {
        try
        {
          RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\ETAS\\LABCAR-AUTOMATION");
          string tauTesterVersion = (string) registryKey.GetValue(nameof (CurrentTauTesterVersion), (object) "none");
          registryKey.Close();
          return tauTesterVersion;
        }
        catch (Exception ex)
        {
          return "Product not found in Registry. " + ex.Message;
        }
      }
    }

    public string CurrentTestStandVersion
    {
      get
      {
        try
        {
          RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\ETAS\\LABCAR-AUTOMATION");
          string testStandVersion = (string) registryKey.GetValue("CurrentTSVersion", (object) "none");
          registryKey.Close();
          return testStandVersion;
        }
        catch (Exception ex)
        {
          return "Product not found in Registry. " + ex.Message;
        }
      }
    }

    public string InstallationDir
    {
      get
      {
        try
        {
          RegistryKey localMachine = Registry.LocalMachine;
          if (this.CurrentLCAVersion == "none")
            return "none";
          RegistryKey registryKey = localMachine.OpenSubKey("Software\\ETAS\\LABCAR-AUTOMATION\\" + SetupManager.Instance.CurrentLCAVersion);
          string installationDir = (string) registryKey.GetValue("Path", (object) "none");
          registryKey.Close();
          return installationDir;
        }
        catch (Exception ex)
        {
          return "Product not found in Registry. " + ex.Message;
        }
      }
    }

    public string DataDir
    {
      get
      {
        try
        {
          RegistryKey localMachine = Registry.LocalMachine;
          if (this.CurrentLCAVersion == "none")
            return "none";
          RegistryKey registryKey = localMachine.OpenSubKey("Software\\ETAS\\LABCAR-AUTOMATION\\" + this.CurrentLCAVersion);
          string dataDir = (string) registryKey.GetValue("ProductData", (object) "none");
          registryKey.Close();
          return dataDir;
        }
        catch (Exception ex)
        {
          return "Product not found in Registry. " + ex.Message;
        }
      }
    }

    public string ManualsDir
    {
      get
      {
        try
        {
          RegistryKey localMachine = Registry.LocalMachine;
          if (this.CurrentLCAVersion == "none")
            return "none";
          RegistryKey registryKey = localMachine.OpenSubKey("Software\\ETAS\\General");
          string str = (string) registryKey.GetValue("ETASManuals", (object) "none");
          registryKey.Close();
          return str + "\\" + this.ProductName + this.CurrentLCAVersion;
        }
        catch (Exception ex)
        {
          return "Product not found in Registry. " + ex.Message;
        }
      }
    }

    public string FullVersion
    {
      get
      {
        try
        {
          RegistryKey localMachine = Registry.LocalMachine;
          if (this.CurrentLCAVersion == "none")
            return "none";
          RegistryKey registryKey = localMachine.OpenSubKey("Software\\ETAS\\LABCAR-AUTOMATION\\" + this.CurrentLCAVersion);
          string fullVersion = (string) registryKey.GetValue("Version", (object) "none");
          registryKey.Close();
          return fullVersion;
        }
        catch (Exception ex)
        {
          return "Product not found in Registry. " + ex.Message;
        }
      }
    }

    public string GetFullVersionForAddon(string AddOn)
    {
      try
      {
        RegistryKey localMachine = Registry.LocalMachine;
        if (this.CurrentLCAVersion == "none")
          return "none";
        RegistryKey registryKey1 = localMachine.OpenSubKey("Software\\ETAS\\LABCAR-AUTOMATION\\" + this.CurrentLCAVersion);
        string fullVersionForAddon = "none";
        foreach (string subKeyName in registryKey1.GetSubKeyNames())
        {
          if (AddOn == subKeyName)
          {
            RegistryKey registryKey2 = registryKey1.OpenSubKey(subKeyName);
            fullVersionForAddon = (string) registryKey2.GetValue("Version", (object) "none");
            registryKey2.Close();
            break;
          }
        }
        registryKey1.Close();
        return fullVersionForAddon;
      }
      catch (Exception ex)
      {
        return "Add-on not found in Registry. " + ex.Message;
      }
    }

    public string GetLicenseCodeForAddon(string AddOn)
    {
      try
      {
        RegistryKey localMachine = Registry.LocalMachine;
        if (this.CurrentLCAVersion == "none")
          return "none";
        RegistryKey registryKey1 = localMachine.OpenSubKey("Software\\ETAS\\LABCAR-AUTOMATION\\" + this.CurrentLCAVersion);
        string licenseCodeForAddon = "none";
        foreach (string subKeyName in registryKey1.GetSubKeyNames())
        {
          if (AddOn == subKeyName)
          {
            RegistryKey registryKey2 = registryKey1.OpenSubKey(subKeyName);
            licenseCodeForAddon = (string) registryKey2.GetValue("LicenseCode", (object) "none");
            registryKey2.Close();
            break;
          }
        }
        registryKey1.Close();
        return licenseCodeForAddon;
      }
      catch (Exception ex)
      {
        return "Add-on not found in Registry. " + ex.Message;
      }
    }

    public string GetLicenseKeyForProduct(string LicenseCode)
    {
      try
      {
        RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\ETAS\\General\\LCLicenses");
        string licenseKeyForProduct = (string) registryKey.GetValue(LicenseCode, (object) "none");
        registryKey.Close();
        return licenseKeyForProduct;
      }
      catch (Exception ex)
      {
        return "License code not found in Registry. " + ex.Message;
      }
    }

    public string StartMenuFolder
    {
      get
      {
        try
        {
          RegistryKey localMachine = Registry.LocalMachine;
          if (this.CurrentLCAVersion == "none")
            return "none";
          RegistryKey registryKey = localMachine.OpenSubKey("Software\\ETAS\\LABCAR-AUTOMATION\\" + this.CurrentLCAVersion);
          string startMenuFolder = (string) registryKey.GetValue(nameof (StartMenuFolder), (object) "none");
          registryKey.Close();
          return startMenuFolder;
        }
        catch (Exception ex)
        {
          return "Product not found in Registry. " + ex.Message;
        }
      }
    }

    public string ConfigurationDir
    {
      get
      {
        try
        {
          RegistryKey localMachine = Registry.LocalMachine;
          if (this.CurrentLCAVersion == "none")
            return "none";
          RegistryKey registryKey = localMachine.OpenSubKey("Software\\ETAS\\LABCAR-AUTOMATION\\" + this.CurrentLCAVersion);
          string configurationDir = (string) registryKey.GetValue("Path", (object) "none");
          if (configurationDir != "none")
            configurationDir += "\\TestTools\\Conf";
          registryKey.Close();
          return configurationDir;
        }
        catch (Exception ex)
        {
          return "Product not found in Registry. " + ex.Message;
        }
      }
    }

    public string MakePathAbsolute(string inputpath, string referencepath) => (!Path.IsPathRooted(inputpath) ? Path.Combine(referencepath, inputpath) : inputpath).Replace('/', '\\');

    public string MakePathRelative(string inputpath, string referencepath) => !Path.IsPathRooted(inputpath) ? inputpath.Replace('/', '\\') : (!inputpath.ToLower().StartsWith(referencepath.ToLower()) ? inputpath : inputpath.Remove(0, referencepath.Length)).Replace('/', '\\').TrimStart("\\".ToCharArray());

    public string[] InstalledTools
    {
      get
      {
        ArrayList arrayList = new ArrayList();
        try
        {
          RegistryKey registryKey1 = Registry.LocalMachine.OpenSubKey("Software\\ETAS\\LABCAR-AUTOMATION");
          if (registryKey1 != null)
          {
            arrayList.Add((object) "LABCAR-AUTOMATION");
            string name1 = (string) registryKey1.GetValue("CurrentVersion");
            RegistryKey registryKey2 = registryKey1.OpenSubKey(name1);
            if (registryKey2 != null)
            {
              string[] subKeyNames = registryKey2.GetSubKeyNames();
              arrayList.AddRange((ICollection) subKeyNames);
              foreach (string name2 in subKeyNames)
              {
                RegistryKey registryKey3 = registryKey2.OpenSubKey(name2);
                string str = (string) registryKey3.GetValue("", (object) "none");
                registryKey3.Close();
                if (str == "none")
                  arrayList.Remove((object) name2);
              }
              registryKey2.Close();
            }
            registryKey1.Close();
          }
        }
        catch (Exception ex)
        {
          arrayList.Add((object) ex.Message);
        }
        return (string[]) arrayList.ToArray(typeof (string));
      }
    }

    public string ETASDir
    {
      get
      {
        string etasDir = this.InstallationDir;
        int length = etasDir.LastIndexOf('\\');
        if (length > 0)
          etasDir = etasDir.Substring(0, length);
        return etasDir;
      }
    }

    public bool OpenUsersGuide()
    {
      try
      {
        Process.Start("IExplore.exe", this.ManualsDir + "\\pages\\home.htm");
      }
      catch
      {
        return false;
      }
      return true;
    }

    public string ADFFile => this.ConfigurationDir + "\\EAS.adf";

    public string BinDir => SetupManager.Instance.InstallationDir + "\\TestTools\\bin";

    public string[] Binaries => SetupManager.binVersionInfo.LCABinaries;

    public string[] BinaryVersions => SetupManager.binVersionInfo.LCABinaryVersions;

    public string BinaryInfoString => SetupManager.binVersionInfo.InfoString;

    private class Nested
    {
      public static readonly ISetupManager s_instance = (ISetupManager) new SetupManager();

      private Nested()
      {
      }
    }
  }
}
