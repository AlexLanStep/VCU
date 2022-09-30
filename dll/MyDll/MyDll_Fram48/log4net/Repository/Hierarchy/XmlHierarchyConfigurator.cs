// Decompiled with JetBrains decompiler
// Type: log4net.Repository.Hierarchy.XmlHierarchyConfigurator
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using log4net.Appender;
using log4net.Core;
using log4net.ObjectRenderer;
using log4net.Util;
using System;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Security;
using System.Xml;

namespace log4net.Repository.Hierarchy
{
  public class XmlHierarchyConfigurator
  {
    private const string CONFIGURATION_TAG = "log4net";
    private const string RENDERER_TAG = "renderer";
    private const string APPENDER_TAG = "appender";
    private const string APPENDER_REF_TAG = "appender-ref";
    private const string PARAM_TAG = "param";
    private const string CATEGORY_TAG = "category";
    private const string PRIORITY_TAG = "priority";
    private const string LOGGER_TAG = "logger";
    private const string NAME_ATTR = "name";
    private const string TYPE_ATTR = "type";
    private const string VALUE_ATTR = "value";
    private const string ROOT_TAG = "root";
    private const string LEVEL_TAG = "level";
    private const string REF_ATTR = "ref";
    private const string ADDITIVITY_ATTR = "additivity";
    private const string THRESHOLD_ATTR = "threshold";
    private const string CONFIG_DEBUG_ATTR = "configDebug";
    private const string INTERNAL_DEBUG_ATTR = "debug";
    private const string CONFIG_UPDATE_MODE_ATTR = "update";
    private const string RENDERING_TYPE_ATTR = "renderingClass";
    private const string RENDERED_TYPE_ATTR = "renderedClass";
    private const string INHERITED = "inherited";
    private Hashtable m_appenderBag;
    private readonly log4net.Repository.Hierarchy.Hierarchy m_hierarchy;

    public XmlHierarchyConfigurator(log4net.Repository.Hierarchy.Hierarchy hierarchy)
    {
      this.m_hierarchy = hierarchy;
      this.m_appenderBag = new Hashtable();
    }

    public void Configure(XmlElement element)
    {
      if (element == null || this.m_hierarchy == null)
        return;
      if (element.LocalName != "log4net")
      {
        LogLog.Error("XmlHierarchyConfigurator: Xml element is - not a <log4net> element.");
      }
      else
      {
        if (!LogLog.InternalDebugging)
        {
          string attribute1 = element.GetAttribute("debug");
          LogLog.Debug("XmlHierarchyConfigurator: debug attribute [" + attribute1 + "].");
          if (attribute1.Length > 0 && attribute1 != "null")
            LogLog.InternalDebugging = OptionConverter.ToBoolean(attribute1, true);
          else
            LogLog.Debug("XmlHierarchyConfigurator: Ignoring debug attribute.");
          string attribute2 = element.GetAttribute("configDebug");
          if (attribute2.Length > 0 && attribute2 != "null")
          {
            LogLog.Warn("XmlHierarchyConfigurator: The \"configDebug\" attribute is deprecated.");
            LogLog.Warn("XmlHierarchyConfigurator: Use the \"debug\" attribute instead.");
            LogLog.InternalDebugging = OptionConverter.ToBoolean(attribute2, true);
          }
        }
        XmlHierarchyConfigurator.ConfigUpdateMode configUpdateMode = XmlHierarchyConfigurator.ConfigUpdateMode.Merge;
        string attribute3 = element.GetAttribute("update");
        if (attribute3 != null)
        {
          if (attribute3.Length > 0)
          {
            try
            {
              configUpdateMode = (XmlHierarchyConfigurator.ConfigUpdateMode) OptionConverter.ConvertStringTo(typeof (XmlHierarchyConfigurator.ConfigUpdateMode), attribute3);
            }
            catch
            {
              LogLog.Error("XmlHierarchyConfigurator: Invalid update attribute value [" + attribute3 + "]");
            }
          }
        }
        LogLog.Debug("XmlHierarchyConfigurator: Configuration update mode [" + configUpdateMode.ToString() + "].");
        if (configUpdateMode == XmlHierarchyConfigurator.ConfigUpdateMode.Overwrite)
        {
          this.m_hierarchy.ResetConfiguration();
          LogLog.Debug("XmlHierarchyConfigurator: Configuration reset before reading config.");
        }
        foreach (XmlNode childNode in element.ChildNodes)
        {
          if (childNode.NodeType == XmlNodeType.Element)
          {
            XmlElement xmlElement = (XmlElement) childNode;
            if (xmlElement.LocalName == "logger")
              this.ParseLogger(xmlElement);
            else if (xmlElement.LocalName == "category")
              this.ParseLogger(xmlElement);
            else if (xmlElement.LocalName == "root")
              this.ParseRoot(xmlElement);
            else if (xmlElement.LocalName == "renderer")
              this.ParseRenderer(xmlElement);
            else if (!(xmlElement.LocalName == "appender"))
              this.SetParameter(xmlElement, (object) this.m_hierarchy);
          }
        }
        string attribute4 = element.GetAttribute("threshold");
        LogLog.Debug("XmlHierarchyConfigurator: Hierarchy Threshold [" + attribute4 + "]");
        if (attribute4.Length <= 0 || !(attribute4 != "null"))
          return;
        Level level = (Level) this.ConvertStringTo(typeof (Level), attribute4);
        if (level != (Level) null)
          this.m_hierarchy.Threshold = level;
        else
          LogLog.Warn("XmlHierarchyConfigurator: Unable to set hierarchy threshold using value [" + attribute4 + "] (with acceptable conversion types)");
      }
    }

    protected IAppender FindAppenderByReference(XmlElement appenderRef)
    {
      string attribute = appenderRef.GetAttribute("ref");
      IAppender appenderByReference = (IAppender) this.m_appenderBag[(object) attribute];
      if (appenderByReference != null)
        return appenderByReference;
      XmlElement appenderElement = (XmlElement) null;
      if (attribute != null && attribute.Length > 0)
      {
        foreach (XmlElement xmlElement in appenderRef.OwnerDocument.GetElementsByTagName("appender"))
        {
          if (xmlElement.GetAttribute("name") == attribute)
          {
            appenderElement = xmlElement;
            break;
          }
        }
      }
      if (appenderElement == null)
      {
        LogLog.Error("XmlHierarchyConfigurator: No appender named [" + attribute + "] could be found.");
        return (IAppender) null;
      }
      IAppender appender = this.ParseAppender(appenderElement);
      if (appender != null)
        this.m_appenderBag[(object) attribute] = (object) appender;
      return appender;
    }

    protected IAppender ParseAppender(XmlElement appenderElement)
    {
      string attribute1 = appenderElement.GetAttribute("name");
      string attribute2 = appenderElement.GetAttribute("type");
      LogLog.Debug("XmlHierarchyConfigurator: Loading Appender [" + attribute1 + "] type: [" + attribute2 + "]");
      try
      {
        IAppender instance = (IAppender) Activator.CreateInstance(SystemInfo.GetTypeFromString(attribute2, true, true));
        instance.Name = attribute1;
        foreach (XmlNode childNode in appenderElement.ChildNodes)
        {
          if (childNode.NodeType == XmlNodeType.Element)
          {
            XmlElement xmlElement = (XmlElement) childNode;
            if (xmlElement.LocalName == "appender-ref")
            {
              string attribute3 = xmlElement.GetAttribute("ref");
              if (instance is IAppenderAttachable appenderAttachable)
              {
                LogLog.Debug("XmlHierarchyConfigurator: Attaching appender named [" + attribute3 + "] to appender named [" + instance.Name + "].");
                IAppender appenderByReference = this.FindAppenderByReference(xmlElement);
                if (appenderByReference != null)
                  appenderAttachable.AddAppender(appenderByReference);
              }
              else
                LogLog.Error("XmlHierarchyConfigurator: Requesting attachment of appender named [" + attribute3 + "] to appender named [" + instance.Name + "] which does not implement log4net.Core.IAppenderAttachable.");
            }
            else
              this.SetParameter(xmlElement, (object) instance);
          }
        }
        if (instance is IOptionHandler optionHandler)
          optionHandler.ActivateOptions();
        LogLog.Debug("XmlHierarchyConfigurator: Created Appender [" + attribute1 + "]");
        return instance;
      }
      catch (Exception ex)
      {
        LogLog.Error("XmlHierarchyConfigurator: Could not create Appender [" + attribute1 + "] of type [" + attribute2 + "]. Reported error follows.", ex);
        return (IAppender) null;
      }
    }

    protected void ParseLogger(XmlElement loggerElement)
    {
      string attribute = loggerElement.GetAttribute("name");
      LogLog.Debug("XmlHierarchyConfigurator: Retrieving an instance of log4net.Repository.Logger for logger [" + attribute + "].");
      Logger logger = this.m_hierarchy.GetLogger(attribute) as Logger;
      lock (logger)
      {
        bool boolean = OptionConverter.ToBoolean(loggerElement.GetAttribute("additivity"), true);
        LogLog.Debug("XmlHierarchyConfigurator: Setting [" + logger.Name + "] additivity to [" + (object) boolean + "].");
        logger.Additivity = boolean;
        this.ParseChildrenOfLoggerElement(loggerElement, logger, false);
      }
    }

    protected void ParseRoot(XmlElement rootElement)
    {
      Logger root = this.m_hierarchy.Root;
      lock (root)
        this.ParseChildrenOfLoggerElement(rootElement, root, true);
    }

    protected void ParseChildrenOfLoggerElement(XmlElement catElement, Logger log, bool isRoot)
    {
      log.RemoveAllAppenders();
      foreach (XmlNode childNode in catElement.ChildNodes)
      {
        if (childNode.NodeType == XmlNodeType.Element)
        {
          XmlElement xmlElement = (XmlElement) childNode;
          if (xmlElement.LocalName == "appender-ref")
          {
            IAppender appenderByReference = this.FindAppenderByReference(xmlElement);
            string attribute = xmlElement.GetAttribute("ref");
            if (appenderByReference != null)
            {
              LogLog.Debug("XmlHierarchyConfigurator: Adding appender named [" + attribute + "] to logger [" + log.Name + "].");
              log.AddAppender(appenderByReference);
            }
            else
              LogLog.Error("XmlHierarchyConfigurator: Appender named [" + attribute + "] not found.");
          }
          else if (xmlElement.LocalName == "level" || xmlElement.LocalName == "priority")
            this.ParseLevel(xmlElement, log, isRoot);
          else
            this.SetParameter(xmlElement, (object) log);
        }
      }
      if (!(log is IOptionHandler optionHandler))
        return;
      optionHandler.ActivateOptions();
    }

    protected void ParseRenderer(XmlElement element)
    {
      string attribute1 = element.GetAttribute("renderingClass");
      string attribute2 = element.GetAttribute("renderedClass");
      LogLog.Debug("XmlHierarchyConfigurator: Rendering class [" + attribute1 + "], Rendered class [" + attribute2 + "].");
      IObjectRenderer renderer = (IObjectRenderer) OptionConverter.InstantiateByClassName(attribute1, typeof (IObjectRenderer), (object) null);
      if (renderer == null)
      {
        LogLog.Error("XmlHierarchyConfigurator: Could not instantiate renderer [" + attribute1 + "].");
      }
      else
      {
        try
        {
          this.m_hierarchy.RendererMap.Put(SystemInfo.GetTypeFromString(attribute2, true, true), renderer);
        }
        catch (Exception ex)
        {
          LogLog.Error("XmlHierarchyConfigurator: Could not find class [" + attribute2 + "].", ex);
        }
      }
    }

    protected void ParseLevel(XmlElement element, Logger log, bool isRoot)
    {
      string str = log.Name;
      if (isRoot)
        str = "root";
      string attribute = element.GetAttribute("value");
      LogLog.Debug("XmlHierarchyConfigurator: Logger [" + str + "] Level string is [" + attribute + "].");
      if ("inherited" == attribute)
      {
        if (isRoot)
        {
          LogLog.Error("XmlHierarchyConfigurator: Root level cannot be inherited. Ignoring directive.");
        }
        else
        {
          LogLog.Debug("XmlHierarchyConfigurator: Logger [" + str + "] level set to inherit from parent.");
          log.Level = (Level) null;
        }
      }
      else
      {
        log.Level = log.Hierarchy.LevelMap[attribute];
        if (log.Level == (Level) null)
          LogLog.Error("XmlHierarchyConfigurator: Undefined level [" + attribute + "] on Logger [" + str + "].");
        else
          LogLog.Debug("XmlHierarchyConfigurator: Logger [" + str + "] level set to [name=\"" + log.Level.Name + "\",value=" + (object) log.Level.Value + "].");
      }
    }

    protected void SetParameter(XmlElement element, object target)
    {
      string name = element.GetAttribute("name");
      if (element.LocalName != "param" || name == null || name.Length == 0)
        name = element.LocalName;
      Type type1 = target.GetType();
      Type type2 = (Type) null;
      MethodInfo methodInfo = (MethodInfo) null;
      PropertyInfo propertyInfo = type1.GetProperty(name, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
      if (propertyInfo != null && propertyInfo.CanWrite)
      {
        type2 = propertyInfo.PropertyType;
      }
      else
      {
        propertyInfo = (PropertyInfo) null;
        methodInfo = this.FindMethodInfo(type1, name);
        if (methodInfo != null)
          type2 = methodInfo.GetParameters()[0].ParameterType;
      }
      if (type2 == null)
      {
        LogLog.Error("XmlHierarchyConfigurator: Cannot find Property [" + name + "] to set object on [" + target.ToString() + "]");
      }
      else
      {
        string str = (string) null;
        if (element.GetAttributeNode("value") != null)
          str = element.GetAttribute("value");
        else if (element.HasChildNodes)
        {
          foreach (XmlNode childNode in element.ChildNodes)
          {
            if (childNode.NodeType == XmlNodeType.CDATA || childNode.NodeType == XmlNodeType.Text)
              str = str != null ? str + childNode.InnerText : childNode.InnerText;
          }
        }
        if (str != null)
        {
          try
          {
            str = OptionConverter.SubstituteVariables(str, Environment.GetEnvironmentVariables());
          }
          catch (SecurityException ex)
          {
            LogLog.Debug("XmlHierarchyConfigurator: Security exception while trying to expand environment variables. Error Ignored. No Expansion.");
          }
          Type targetType = (Type) null;
          string attribute = element.GetAttribute("type");
          if (attribute != null)
          {
            if (attribute.Length > 0)
            {
              try
              {
                Type typeFromString = SystemInfo.GetTypeFromString(attribute, true, true);
                LogLog.Debug("XmlHierarchyConfigurator: Parameter [" + name + "] specified subtype [" + typeFromString.FullName + "]");
                if (!type2.IsAssignableFrom(typeFromString))
                {
                  if (OptionConverter.CanConvertTypeTo(typeFromString, type2))
                  {
                    targetType = type2;
                    type2 = typeFromString;
                  }
                  else
                    LogLog.Error("XmlHierarchyConfigurator: Subtype [" + typeFromString.FullName + "] set on [" + name + "] is not a subclass of property type [" + type2.FullName + "] and there are no acceptable type conversions.");
                }
                else
                  type2 = typeFromString;
              }
              catch (Exception ex)
              {
                LogLog.Error("XmlHierarchyConfigurator: Failed to find type [" + attribute + "] set on [" + name + "]", ex);
              }
            }
          }
          object sourceInstance = this.ConvertStringTo(type2, str);
          if (sourceInstance != null && targetType != null)
          {
            LogLog.Debug("XmlHierarchyConfigurator: Performing additional conversion of value from [" + sourceInstance.GetType().Name + "] to [" + targetType.Name + "]");
            sourceInstance = OptionConverter.ConvertTypeTo(sourceInstance, targetType);
          }
          if (sourceInstance != null)
          {
            if (propertyInfo != null)
            {
              LogLog.Debug("XmlHierarchyConfigurator: Setting Property [" + propertyInfo.Name + "] to " + sourceInstance.GetType().Name + " value [" + sourceInstance.ToString() + "]");
              try
              {
                propertyInfo.SetValue(target, sourceInstance, BindingFlags.SetProperty, (Binder) null, (object[]) null, CultureInfo.InvariantCulture);
              }
              catch (TargetInvocationException ex)
              {
                LogLog.Error("XmlHierarchyConfigurator: Failed to set parameter [" + propertyInfo.Name + "] on object [" + target + "] using value [" + sourceInstance + "]", ex.InnerException);
              }
            }
            else
            {
              if (methodInfo == null)
                return;
              LogLog.Debug("XmlHierarchyConfigurator: Setting Collection Property [" + methodInfo.Name + "] to " + sourceInstance.GetType().Name + " value [" + sourceInstance.ToString() + "]");
              try
              {
                methodInfo.Invoke(target, BindingFlags.InvokeMethod, (Binder) null, new object[1]
                {
                  sourceInstance
                }, CultureInfo.InvariantCulture);
              }
              catch (TargetInvocationException ex)
              {
                LogLog.Error("XmlHierarchyConfigurator: Failed to set parameter [" + name + "] on object [" + target + "] using value [" + sourceInstance + "]", ex.InnerException);
              }
            }
          }
          else
            LogLog.Warn("XmlHierarchyConfigurator: Unable to set property [" + name + "] on object [" + target + "] using value [" + str + "] (with acceptable conversion types)");
        }
        else
        {
          object obj;
          if (type2 == typeof (string) && !this.HasAttributesOrElements(element))
          {
            obj = (object) "";
          }
          else
          {
            Type defaultTargetType = (Type) null;
            if (XmlHierarchyConfigurator.IsTypeConstructible(type2))
              defaultTargetType = type2;
            obj = this.CreateObjectFromXml(element, defaultTargetType, type2);
          }
          if (obj == null)
            LogLog.Error("XmlHierarchyConfigurator: Failed to create object to set param: " + name);
          else if (propertyInfo != null)
          {
            LogLog.Debug("XmlHierarchyConfigurator: Setting Property [" + propertyInfo.Name + "] to object [" + obj + "]");
            try
            {
              propertyInfo.SetValue(target, obj, BindingFlags.SetProperty, (Binder) null, (object[]) null, CultureInfo.InvariantCulture);
            }
            catch (TargetInvocationException ex)
            {
              LogLog.Error("XmlHierarchyConfigurator: Failed to set parameter [" + propertyInfo.Name + "] on object [" + target + "] using value [" + obj + "]", ex.InnerException);
            }
          }
          else
          {
            if (methodInfo == null)
              return;
            LogLog.Debug("XmlHierarchyConfigurator: Setting Collection Property [" + methodInfo.Name + "] to object [" + obj + "]");
            try
            {
              methodInfo.Invoke(target, BindingFlags.InvokeMethod, (Binder) null, new object[1]
              {
                obj
              }, CultureInfo.InvariantCulture);
            }
            catch (TargetInvocationException ex)
            {
              LogLog.Error("XmlHierarchyConfigurator: Failed to set parameter [" + methodInfo.Name + "] on object [" + target + "] using value [" + obj + "]", ex.InnerException);
            }
          }
        }
      }
    }

    private bool HasAttributesOrElements(XmlElement element)
    {
      foreach (XmlNode childNode in element.ChildNodes)
      {
        if (childNode.NodeType == XmlNodeType.Attribute || childNode.NodeType == XmlNodeType.Element)
          return true;
      }
      return false;
    }

    private static bool IsTypeConstructible(Type type)
    {
      if (type.IsClass && !type.IsAbstract)
      {
        ConstructorInfo constructor = type.GetConstructor(new Type[0]);
        if (constructor != null && !constructor.IsAbstract && !constructor.IsPrivate)
          return true;
      }
      return false;
    }

    private MethodInfo FindMethodInfo(Type targetType, string name)
    {
      string strB1 = name;
      string strB2 = "Add" + name;
      foreach (MethodInfo method in targetType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
      {
        if (!method.IsStatic && (string.Compare(method.Name, strB1, true, CultureInfo.InvariantCulture) == 0 || string.Compare(method.Name, strB2, true, CultureInfo.InvariantCulture) == 0) && method.GetParameters().Length == 1)
          return method;
      }
      return (MethodInfo) null;
    }

    protected object ConvertStringTo(Type type, string value)
    {
      if (typeof (Level) != type)
        return OptionConverter.ConvertStringTo(type, value);
      Level level = this.m_hierarchy.LevelMap[value];
      if (level == (Level) null)
        LogLog.Error("XmlHierarchyConfigurator: Unknown Level Specified [" + value + "]");
      return (object) level;
    }

    protected object CreateObjectFromXml(
      XmlElement element,
      Type defaultTargetType,
      Type typeConstraint)
    {
      string attribute = element.GetAttribute("type");
      Type type;
      if (attribute != null)
      {
        if (attribute.Length != 0)
        {
          try
          {
            type = SystemInfo.GetTypeFromString(attribute, true, true);
            goto label_7;
          }
          catch (Exception ex)
          {
            LogLog.Error("XmlHierarchyConfigurator: Failed to find type [" + attribute + "]", ex);
            return (object) null;
          }
        }
      }
      if (defaultTargetType == null)
      {
        LogLog.Error("XmlHierarchyConfigurator: Object type not specified. Cannot create object of type [" + typeConstraint.FullName + "]. Missing Value or Type.");
        return (object) null;
      }
      type = defaultTargetType;
label_7:
      bool flag = false;
      if (typeConstraint != null && !typeConstraint.IsAssignableFrom(type))
      {
        if (OptionConverter.CanConvertTypeTo(type, typeConstraint))
        {
          flag = true;
        }
        else
        {
          LogLog.Error("XmlHierarchyConfigurator: Object type [" + type.FullName + "] is not assignable to type [" + typeConstraint.FullName + "]. There are no acceptable type conversions.");
          return (object) null;
        }
      }
      object obj = (object) null;
      try
      {
        obj = Activator.CreateInstance(type);
      }
      catch (Exception ex)
      {
        LogLog.Error("XmlHierarchyConfigurator: Failed to construct object of type [" + type.FullName + "] Exception: " + ex.ToString());
      }
      foreach (XmlNode childNode in element.ChildNodes)
      {
        if (childNode.NodeType == XmlNodeType.Element)
          this.SetParameter((XmlElement) childNode, obj);
      }
      if (obj is IOptionHandler optionHandler)
        optionHandler.ActivateOptions();
      return flag ? OptionConverter.ConvertTypeTo(obj, typeConstraint) : obj;
    }

    private enum ConfigUpdateMode
    {
      Merge,
      Overwrite,
    }
  }
}
