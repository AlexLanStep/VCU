// Decompiled with JetBrains decompiler
// Type: log4net.Layout.XmlLayoutSchemaLog4j
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using log4net.Core;
using log4net.Util;
using System;
using System.Collections;
using System.Xml;

namespace log4net.Layout
{
  public class XmlLayoutSchemaLog4j : XmlLayoutBase
  {
    private static readonly DateTime s_date1970 = new DateTime(1970, 1, 1);

    public XmlLayoutSchemaLog4j()
    {
    }

    public XmlLayoutSchemaLog4j(bool locationInfo)
      : base(locationInfo)
    {
    }

    public string Version
    {
      get => "1.2";
      set
      {
        if (value != "1.2")
          throw new ArgumentException("Only version 1.2 of the log4j schema is currently supported");
      }
    }

    protected override void FormatXml(XmlWriter writer, LoggingEvent loggingEvent)
    {
      if (loggingEvent.LookupProperty("log4net:HostName") != null && loggingEvent.LookupProperty("log4jmachinename") == null)
        loggingEvent.GetProperties()["log4jmachinename"] = loggingEvent.LookupProperty("log4net:HostName");
      if (loggingEvent.LookupProperty("log4japp") == null && loggingEvent.Domain != null && loggingEvent.Domain.Length > 0)
        loggingEvent.GetProperties()["log4japp"] = (object) loggingEvent.Domain;
      if (loggingEvent.Identity != null && loggingEvent.Identity.Length > 0 && loggingEvent.LookupProperty("log4net:Identity") == null)
        loggingEvent.GetProperties()["log4net:Identity"] = (object) loggingEvent.Identity;
      if (loggingEvent.UserName != null && loggingEvent.UserName.Length > 0 && loggingEvent.LookupProperty("log4net:UserName") == null)
        loggingEvent.GetProperties()["log4net:UserName"] = (object) loggingEvent.UserName;
      writer.WriteStartElement("log4j:event");
      writer.WriteAttributeString("logger", loggingEvent.LoggerName);
      TimeSpan timeSpan = loggingEvent.TimeStamp.ToUniversalTime() - XmlLayoutSchemaLog4j.s_date1970;
      writer.WriteAttributeString("timestamp", XmlConvert.ToString((long) timeSpan.TotalMilliseconds));
      writer.WriteAttributeString("level", loggingEvent.Level.DisplayName);
      writer.WriteAttributeString("thread", loggingEvent.ThreadName);
      writer.WriteStartElement("log4j:message");
      Transform.WriteEscapedXmlString(writer, loggingEvent.RenderedMessage, this.InvalidCharReplacement);
      writer.WriteEndElement();
      object obj = loggingEvent.LookupProperty("NDC");
      if (obj != null)
      {
        string andRender = loggingEvent.Repository.RendererMap.FindAndRender(obj);
        if (andRender != null && andRender.Length > 0)
        {
          writer.WriteStartElement("log4j:NDC");
          Transform.WriteEscapedXmlString(writer, andRender, this.InvalidCharReplacement);
          writer.WriteEndElement();
        }
      }
      PropertiesDictionary properties = loggingEvent.GetProperties();
      if (properties.Count > 0)
      {
        writer.WriteStartElement("log4j:properties");
        foreach (DictionaryEntry dictionaryEntry in (IEnumerable) properties)
        {
          writer.WriteStartElement("log4j:data");
          writer.WriteAttributeString("name", (string) dictionaryEntry.Key);
          string andRender = loggingEvent.Repository.RendererMap.FindAndRender(dictionaryEntry.Value);
          writer.WriteAttributeString("value", andRender);
          writer.WriteEndElement();
        }
        writer.WriteEndElement();
      }
      string exceptionString = loggingEvent.GetExceptionString();
      if (exceptionString != null && exceptionString.Length > 0)
      {
        writer.WriteStartElement("log4j:throwable");
        Transform.WriteEscapedXmlString(writer, exceptionString, this.InvalidCharReplacement);
        writer.WriteEndElement();
      }
      if (this.LocationInfo)
      {
        LocationInfo locationInformation = loggingEvent.LocationInformation;
        writer.WriteStartElement("log4j:locationInfo");
        writer.WriteAttributeString("class", locationInformation.ClassName);
        writer.WriteAttributeString("method", locationInformation.MethodName);
        writer.WriteAttributeString("file", locationInformation.FileName);
        writer.WriteAttributeString("line", locationInformation.LineNumber);
        writer.WriteEndElement();
      }
      writer.WriteEndElement();
    }
  }
}
