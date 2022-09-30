// Decompiled with JetBrains decompiler
// Type: log4net.Util.FormattingInfo
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

namespace log4net.Util
{
  public class FormattingInfo
  {
    private int m_min = -1;
    private int m_max = int.MaxValue;
    private bool m_leftAlign = false;

    public FormattingInfo()
    {
    }

    public FormattingInfo(int min, int max, bool leftAlign)
    {
      this.m_min = min;
      this.m_max = max;
      this.m_leftAlign = leftAlign;
    }

    public int Min
    {
      get => this.m_min;
      set => this.m_min = value;
    }

    public int Max
    {
      get => this.m_max;
      set => this.m_max = value;
    }

    public bool LeftAlign
    {
      get => this.m_leftAlign;
      set => this.m_leftAlign = value;
    }
  }
}
