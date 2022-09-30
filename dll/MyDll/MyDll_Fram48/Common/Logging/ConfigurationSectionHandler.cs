using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.Xml;


using Common.Logging.Simple;


namespace Common.Logging
{

  public class ConfigurationSectionHandler : IConfigurationSectionHandler
  {
    private static readonly string LOGFACTORYADAPTER_ELEMENT = "factoryAdapter";
    private static readonly string LOGFACTORYADAPTER_ELEMENT_TYPE_ATTRIB = "type";
    private static readonly string ARGUMENT_ELEMENT = "arg";
    private static readonly string ARGUMENT_ELEMENT_KEY_ATTRIB = "key";
    private static readonly string ARGUMENT_ELEMENT_VALUE_ATTRIB = "value";

    private LogSetting ReadConfiguration(XmlNode section)
    {
      XmlNode xmlNode1 = section.SelectSingleNode(ConfigurationSectionHandler.LOGFACTORYADAPTER_ELEMENT);
      string empty1 = string.Empty;
      if (xmlNode1.Attributes[ConfigurationSectionHandler.LOGFACTORYADAPTER_ELEMENT_TYPE_ATTRIB] != null)
        empty1 = xmlNode1.Attributes[ConfigurationSectionHandler.LOGFACTORYADAPTER_ELEMENT_TYPE_ATTRIB].Value;
      if (empty1 == string.Empty)
        throw new LoggingException("Required Attribute '" + ConfigurationSectionHandler.LOGFACTORYADAPTER_ELEMENT_TYPE_ATTRIB + "' not found in element '" + ConfigurationSectionHandler.LOGFACTORYADAPTER_ELEMENT + "'");
      Type factoryAdapterType;
      try
      {
        factoryAdapterType = string.Compare(empty1, "CONSOLE", true) != 0 ? (string.Compare(empty1, "TRACE", true) != 0 ? (string.Compare(empty1, "NOOP", true) != 0 ? Type.GetType(empty1, true, false) : typeof(NoOpLoggerFactoryAdapter)) : typeof(TraceLoggerFactoryAdapter)) : typeof(ConsoleOutLoggerFactoryAdapter);
      }
      catch (Exception ex)
      {
        throw new LoggingException("Unable to create type '" + empty1 + "'", ex);
      }
      XmlNodeList xmlNodeList = xmlNode1.SelectNodes(ConfigurationSectionHandler.ARGUMENT_ELEMENT);
      NameValueCollection properties = new NameValueCollection((IHashCodeProvider)null, (IComparer)new CaseInsensitiveComparer());
      foreach (XmlNode xmlNode2 in xmlNodeList)
      {
        string empty2 = string.Empty;
        string empty3 = string.Empty;
        XmlAttribute attribute1 = xmlNode2.Attributes[ConfigurationSectionHandler.ARGUMENT_ELEMENT_KEY_ATTRIB];
        XmlAttribute attribute2 = xmlNode2.Attributes[ConfigurationSectionHandler.ARGUMENT_ELEMENT_VALUE_ATTRIB];
        string name = attribute1 != null ? attribute1.Value : throw new LoggingException("Required Attribute '" + ConfigurationSectionHandler.ARGUMENT_ELEMENT_KEY_ATTRIB + "' not found in element '" + ConfigurationSectionHandler.ARGUMENT_ELEMENT + "'");
        if (attribute2 != null)
          empty3 = attribute2.Value;
        properties.Add(name, empty3);
      }
      return new LogSetting(factoryAdapterType, properties);
    }

    public object Create(object parent, object configContext, XmlNode section)
    {
      int count = section.SelectNodes(ConfigurationSectionHandler.LOGFACTORYADAPTER_ELEMENT).Count;
      if (count > 1)
        throw new LoggingException("Only one <logFactoryAdapter> element allowed");
      return count == 1 ? (object)this.ReadConfiguration(section) : (object)null;
    }
  }

}