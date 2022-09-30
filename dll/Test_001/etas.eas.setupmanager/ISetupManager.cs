// Decompiled with JetBrains decompiler
// Type: ETAS.EAS.ISetupManager
// Assembly: etas.eas.setupmanager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=24eada690b1476fd
// MVID: 6F2445EF-C18D-4C24-B748-E3B896DC125F
// Assembly location: E:\LabCar\BasaDll\etas.eas.setupmanager.dll

namespace ETAS.EAS
{
  public interface ISetupManager
  {
    string ProductName { get; }

    string CurrentLCAVersion { get; }

    string CurrentTauTesterVersion { get; }

    string CurrentTestStandVersion { get; }

    string InstallationDir { get; }

    string DataDir { get; }

    string ManualsDir { get; }

    string FullVersion { get; }

    string GetFullVersionForAddon(string AddOn);

    string GetLicenseCodeForAddon(string AddOn);

    string GetLicenseKeyForProduct(string LicenseCode);

    string StartMenuFolder { get; }

    string ConfigurationDir { get; }

    string MakePathAbsolute(string inputpath, string referencepath);

    string MakePathRelative(string inputpath, string referencepath);

    string[] InstalledTools { get; }

    string ETASDir { get; }

    bool OpenUsersGuide();

    string ADFFile { get; }

    string BinDir { get; }

    string[] Binaries { get; }

    string[] BinaryVersions { get; }

    string BinaryInfoString { get; }
  }
}
