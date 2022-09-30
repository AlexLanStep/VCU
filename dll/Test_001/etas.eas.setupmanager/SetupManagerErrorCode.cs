// Decompiled with JetBrains decompiler
// Type: ETAS.EAS.SetupManagerErrorCode
// Assembly: etas.eas.setupmanager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=24eada690b1476fd
// MVID: 6F2445EF-C18D-4C24-B748-E3B896DC125F
// Assembly location: E:\LabCar\BasaDll\etas.eas.setupmanager.dll

using ETAS.EAS.Util;
using System;

namespace ETAS.EAS
{
  [Serializable]
  public class SetupManagerErrorCode : EASStatusCode
  {
    public static SetupManagerErrorCode NO_FILE = new SetupManagerErrorCode(nameof (NO_FILE), "The Configuration File EASConfig.xml was not found!");
    public static SetupManagerErrorCode XML_SAVE_ERROR = new SetupManagerErrorCode(nameof (XML_SAVE_ERROR), "Could not save EASConfig.xml!");
    public static SetupManagerErrorCode XML_READ_ERROR = new SetupManagerErrorCode(nameof (XML_READ_ERROR), "Error while reading data from EASConfig.xml!");
    public static SetupManagerErrorCode ATTRIBUTE_MISSING = new SetupManagerErrorCode(nameof (ATTRIBUTE_MISSING), "Could not save EASConfig.xml! The xml attribute 'Key' is missing for a <StringProperty>.");
    public static SetupManagerErrorCode ELEMENT_MISSING = new SetupManagerErrorCode(nameof (ELEMENT_MISSING), "Could not save EASConfig.xml! A <StringProperty> xml element is missing.");
    public static SetupManagerErrorCode NOT_UNIQUE_CATEGORY = new SetupManagerErrorCode(nameof (NOT_UNIQUE_CATEGORY), "The Category with the name {0} in this EASConfig.xml file is not unique");
    public static SetupManagerErrorCode NOT_UNIQUE_STRINGPROPERTY = new SetupManagerErrorCode("NOT_UNIQUE_STRINGPROPERTY ", "The Stringproperty is in this EASConfig.xml file not unique");
    public static SetupManagerErrorCode XPATH_CATEGORY = new SetupManagerErrorCode(nameof (XPATH_CATEGORY), "An Error occurs when processing the XPath expression in the Category");
    public static SetupManagerErrorCode XPATH_STRINGPROPERTY = new SetupManagerErrorCode(nameof (XPATH_STRINGPROPERTY), "An Error occurs when processing the XPath expression in the StringProperty");

    public SetupManagerErrorCode(string name, string description)
      : base(name, description)
    {
    }
  }
}
