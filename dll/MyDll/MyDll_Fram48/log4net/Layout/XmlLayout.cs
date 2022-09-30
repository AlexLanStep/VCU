// Decompiled with JetBrains decompiler
// Type: log4net.Layout.XmlLayout
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using log4net.Core;
using log4net.Util;
using System;
using System.Collections;
using System.Text;
using System.Xml;

namespace log4net.Layout
{
  public class XmlLayout : XmlLayoutBase
  {
    private const string PREFIX = "log4net";
    private const string ELM_EVENT = "event";
    private const string ELM_MESSAGE = "message";
    private const string ELM_PROPERTIES = "properties";
    private const string ELM_GLOBAL_PROPERTIES = "global-properties";
    private const string ELM_DATA = "data";
    private const string ELM_EXCEPTION = "exception";
    private const string ELM_LOCATION = "locationInfo";
    private const string ATTR_LOGGER = "logger";
    private const string ATTR_TIMESTAMP = "timestamp";
    private const string ATTR_LEVEL = "level";
    private const string ATTR_THREAD = "thread";
    private const string ATTR_DOMAIN = "domain";
    private const string ATTR_IDENTITY = "identity";
    private const string ATTR_USERNAME = "username";
    private const string ATTR_CLASS = "class";
    private const string ATTR_METHOD = "method";
    private const string ATTR_FILE = "file";
    private const string ATTR_LINE = "line";
    private const string ATTR_NAME = "name";
    private const string ATTR_VALUE = "value";
    private string m_prefix = "log4net";
    private string m_elmEvent = "event";
    private string m_elmMessage = "message";
    private string m_elmData = "data";
    private string m_elmProperties = "properties";
    private string m_elmException = "exception";
    private string m_elmLocation = "locationInfo";
    private bool m_base64Message = false;
    private bool m_base64Properties = false;

    public XmlLayout()
    {
    }

    public XmlLayout(bool locationInfo)
      : base(locationInfo)
    {
    }

    public string Prefix
    {
      get => this.m_prefix;
      set => this.m_prefix = value;
    }

    public bool Base64EncodeMessage
    {
      get => this.m_base64Message;
      set => this.m_base64Message = value;
    }

    public bool Base64EncodeProperties
    {
      get => this.m_base64Properties;
      set => this.m_base64Properties = value;
    }

    public override void ActivateOptions()
    {
      base.ActivateOptions();
      if (this.m_prefix == null || this.m_prefix.Length <= 0)
        return;
      this.m_elmEvent = this.m_prefix + ":event";
      this.m_elmMessage = this.m_prefix + ":message";
      this.m_elmProperties = this.m_prefix + ":properties";
      this.m_elmData = this.m_prefix + ":data";
      this.m_elmException = this.m_prefix + ":exception";
      this.m_elmLocation = this.m_prefix + ":locationInfo";
    }

    protected override void FormatXml(XmlWriter writer, LoggingEvent loggingEvent)
    {
      writer.WriteStartElement(this.m_elmEvent);
      writer.WriteAttributeString("logger", loggingEvent.LoggerName);
      writer.WriteAttributeString("timestamp", XmlConvert.ToString(loggingEvent.TimeStamp));
      writer.WriteAttributeString("level", loggingEvent.Level.DisplayName);
      writer.WriteAttributeString("thread", loggingEvent.ThreadName);
      if (loggingEvent.Domain != null && loggingEvent.Domain.Length > 0)
        writer.WriteAttributeString("domain", loggingEvent.Domain);
      if (loggingEvent.Identity != null && loggingEvent.Identity.Length > 0)
        writer.WriteAttributeString("identity", loggingEvent.Identity);
      if (loggingEvent.UserName != null && loggingEvent.UserName.Length > 0)
        writer.WriteAttributeString("username", loggingEvent.UserName);
      writer.WriteStartElement(this.m_elmMessage);
      if (!this.Base64EncodeMessage)
      {
        Transform.WriteEscapedXmlString(writer, loggingEvent.RenderedMessage, this.InvalidCharReplacement);
      }
      else
      {
        byte[] bytes = Encoding.UTF8.GetBytes(loggingEvent.RenderedMessage);
        string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
        Transform.WriteEscapedXmlString(writer, base64String, this.InvalidCharReplacement);
      }
      writer.WriteEndElement();
      PropertiesDictionary properties = loggingEvent.GetProperties();
      if (properties.Count > 0)
      {
        writer.WriteStartElement(this.m_elmProperties);
        foreach (DictionaryEntry dictionaryEntry in (IEnumerable) properties)
        {
          writer.WriteStartElement(this.m_elmData);
          writer.WriteAttributeString("name", Transform.MaskXmlInvalidCharacters((string) dictionaryEntry.Key, this.InvalidCharReplacement));
          string str;
          if (!this.Base64EncodeProperties)
          {
            str = Transform.MaskXmlInvalidCharacters(loggingEvent.Repository.RendererMap.FindAndRender(dictionaryEntry.Value), this.InvalidCharReplacement);
          }
          else
          {
            byte[] bytes = Encoding.UTF8.GetBytes(loggingEvent.Repository.RendererMap.FindAndRender(dictionaryEntry.Value));
            str = Convert.ToBase64String(bytes, 0, bytes.Length);
          }
          writer.WriteAttributeString("value", str);
          writer.WriteEndElement();
        }
        writer.WriteEndElement();
      }
      string exceptionString = loggingEvent.GetExceptionString();
      if (exceptionString != null && exceptionString.Length > 0)
      {
        writer.WriteStartElement(this.m_elmException);
        Transform.WriteEscapedXmlString(writer, exceptionString, this.InvalidCharReplacement);
        writer.WriteEndElement();
      }
      if (this.LocationInfo)
      {
        LocationInfo locationInformation = loggingEvent.LocationInformation;
        writer.WriteStartElement(this.m_elmLocation);
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
