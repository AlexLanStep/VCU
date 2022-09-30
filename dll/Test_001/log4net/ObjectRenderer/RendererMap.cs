// Decompiled with JetBrains decompiler
// Type: log4net.ObjectRenderer.RendererMap
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using log4net.Util;
using System;
using System.Collections;
using System.Globalization;
using System.IO;

namespace log4net.ObjectRenderer
{
  public class RendererMap
  {
    private Hashtable m_map;
    private Hashtable m_cache = new Hashtable();
    private static IObjectRenderer s_defaultRenderer = (IObjectRenderer) new log4net.ObjectRenderer.DefaultRenderer();

    public RendererMap() => this.m_map = Hashtable.Synchronized(new Hashtable());

    public string FindAndRender(object obj)
    {
      if (obj is string andRender)
        return andRender;
      StringWriter writer = new StringWriter((IFormatProvider) CultureInfo.InvariantCulture);
      this.FindAndRender(obj, (TextWriter) writer);
      return writer.ToString();
    }

    public void FindAndRender(object obj, TextWriter writer)
    {
      if (obj == null)
        writer.Write(SystemInfo.NullText);
      else if (obj is string str)
      {
        writer.Write(str);
      }
      else
      {
        try
        {
          this.Get(obj.GetType()).RenderObject(this, obj, writer);
        }
        catch (Exception ex)
        {
          LogLog.Error("RendererMap: Exception while rendering object of type [" + obj.GetType().FullName + "]", ex);
          string str1 = "";
          if (obj != null && obj.GetType() != null)
            str1 = obj.GetType().FullName;
          writer.Write("<log4net.Error>Exception rendering object type [" + str1 + "]");
          if (ex != null)
          {
            string str2 = (string) null;
            try
            {
              str2 = ex.ToString();
            }
            catch
            {
            }
            writer.Write("<stackTrace>" + str2 + "</stackTrace>");
          }
          writer.Write("</log4net.Error>");
        }
      }
    }

    public IObjectRenderer Get(object obj) => obj == null ? (IObjectRenderer) null : this.Get(obj.GetType());

    public IObjectRenderer Get(Type type)
    {
      IObjectRenderer objectRenderer = type != null ? (IObjectRenderer) this.m_cache[(object) type] : throw new ArgumentNullException(nameof (type));
      if (objectRenderer == null)
      {
        for (Type type1 = type; type1 != null; type1 = type1.BaseType)
        {
          objectRenderer = this.SearchTypeAndInterfaces(type1);
          if (objectRenderer != null)
            break;
        }
        if (objectRenderer == null)
          objectRenderer = RendererMap.s_defaultRenderer;
        this.m_cache[(object) type] = (object) objectRenderer;
      }
      return objectRenderer;
    }

    private IObjectRenderer SearchTypeAndInterfaces(Type type)
    {
      IObjectRenderer objectRenderer1 = (IObjectRenderer) this.m_map[(object) type];
      if (objectRenderer1 != null)
        return objectRenderer1;
      foreach (Type type1 in type.GetInterfaces())
      {
        IObjectRenderer objectRenderer2 = this.SearchTypeAndInterfaces(type1);
        if (objectRenderer2 != null)
          return objectRenderer2;
      }
      return (IObjectRenderer) null;
    }

    public IObjectRenderer DefaultRenderer => RendererMap.s_defaultRenderer;

    public void Clear()
    {
      this.m_map.Clear();
      this.m_cache.Clear();
    }

    public void Put(Type typeToRender, IObjectRenderer renderer)
    {
      this.m_cache.Clear();
      if (typeToRender == null)
        throw new ArgumentNullException(nameof (typeToRender));
      this.m_map[(object) typeToRender] = renderer != null ? (object) renderer : throw new ArgumentNullException(nameof (renderer));
    }
  }
}
