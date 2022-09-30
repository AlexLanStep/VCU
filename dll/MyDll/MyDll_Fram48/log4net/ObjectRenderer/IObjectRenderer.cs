// Decompiled with JetBrains decompiler
// Type: log4net.ObjectRenderer.IObjectRenderer
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using System.IO;

namespace log4net.ObjectRenderer
{
  public interface IObjectRenderer
  {
    void RenderObject(RendererMap rendererMap, object obj, TextWriter writer);
  }
}
