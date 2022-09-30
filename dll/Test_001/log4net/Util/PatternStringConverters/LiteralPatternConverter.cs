// Decompiled with JetBrains decompiler
// Type: log4net.Util.PatternStringConverters.LiteralPatternConverter
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using System;
using System.IO;

namespace log4net.Util.PatternStringConverters
{
  internal class LiteralPatternConverter : PatternConverter
  {
    public override PatternConverter SetNext(PatternConverter pc)
    {
      if (!(pc is LiteralPatternConverter patternConverter))
        return base.SetNext(pc);
      this.Option += patternConverter.Option;
      return (PatternConverter) this;
    }

    public override void Format(TextWriter writer, object state) => writer.Write(this.Option);

    protected override void Convert(TextWriter writer, object state) => throw new InvalidOperationException("Should never get here because of the overridden Format method");
  }
}
