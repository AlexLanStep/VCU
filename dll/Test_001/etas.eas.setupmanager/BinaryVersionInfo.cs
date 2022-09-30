// Decompiled with JetBrains decompiler
// Type: ETAS.EAS.BinaryVersionInfo
// Assembly: etas.eas.setupmanager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=24eada690b1476fd
// MVID: 6F2445EF-C18D-4C24-B748-E3B896DC125F
// Assembly location: E:\LabCar\BasaDll\etas.eas.setupmanager.dll

using System.Collections;
using System.IO;
using System.Reflection;

namespace ETAS.EAS
{
  public class BinaryVersionInfo
  {
    private ArrayList _LCABinaries;
    private ArrayList _LCABinaryVersions;
    private bool _isInitialized;

    public string[] LCABinaries
    {
      get
      {
        this.Init();
        return (string[]) this._LCABinaries.ToArray(typeof (string));
      }
    }

    public string[] LCABinaryVersions
    {
      get
      {
        this.Init();
        return (string[]) this._LCABinaryVersions.ToArray(typeof (string));
      }
    }

    public BinaryVersionInfo()
    {
      this._isInitialized = false;
      this._LCABinaries = new ArrayList();
      this._LCABinaryVersions = new ArrayList();
    }

    public bool Init()
    {
      if (!this._isInitialized)
      {
        this._LCABinaries = new ArrayList();
        this._LCABinaryVersions = new ArrayList();
        foreach (string file in Directory.GetFiles(SetupManager.Instance.BinDir, "*.dll"))
        {
          string fileName = Path.GetFileName(file.ToLower());
          if (!fileName.StartsWith("interop") && !fileName.StartsWith("syncfusion") && !fileName.StartsWith("log4net") && !fileName.StartsWith("mshtml") && !fileName.StartsWith("nunit") && !fileName.StartsWith("axinterop"))
            this._LCABinaries.Add((object) file);
        }
        foreach (string file in Directory.GetFiles(SetupManager.Instance.BinDir, "*.exe"))
        {
          string fileName = Path.GetFileName(file.ToLower());
          if (!fileName.StartsWith("regasm") && !fileName.StartsWith("lcagnu"))
            this._LCABinaries.Add((object) file);
        }
        foreach (string lcaBinary in this._LCABinaries)
        {
          try
          {
            object[] customAttributes = Assembly.LoadFrom(lcaBinary).GetCustomAttributes(typeof (AssemblyDescriptionAttribute), false);
            if (customAttributes.Length > 0)
              this._LCABinaryVersions.Add((object) (customAttributes[0] as AssemblyDescriptionAttribute).Description);
            else
              this._LCABinaryVersions.Add((object) "-");
          }
          catch
          {
            this._LCABinaryVersions.Add((object) "<None>");
          }
        }
        this._isInitialized = true;
      }
      return this._isInitialized;
    }

    public string InfoString
    {
      get
      {
        this.Init();
        string infoString = "LABCAR-AUTOMATION Binary Information\r\n\r\nBinary\tInfo\r\n\r\n";
        try
        {
          for (int index = 0; index < this.LCABinaries.Length; ++index)
          {
            infoString = infoString + this.LCABinaries[index] + "\t";
            infoString = infoString + this.LCABinaryVersions[index] + "\r\n";
          }
        }
        catch
        {
        }
        return infoString;
      }
    }
  }
}
