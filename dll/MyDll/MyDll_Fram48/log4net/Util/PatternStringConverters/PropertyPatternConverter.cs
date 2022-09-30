// Decompiled with JetBrains decompiler
// Type: log4net.Util.PatternStringConverters.PropertyPatternConverter
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using log4net.Repository;
using System.Collections;
using System.IO;

namespace log4net.Util.PatternStringConverters
{
  internal sealed class PropertyPatternConverter : PatternConverter
  {
    protected override void Convert(TextWriter writer, object state)
    {
      CompositeProperties compositeProperties = new CompositeProperties();
      PropertiesDictionary properties1 = LogicalThreadContext.Properties.GetProperties(false);
      if (properties1 != null)
        compositeProperties.Add((ReadOnlyPropertiesDictionary) properties1);
      PropertiesDictionary properties2 = ThreadContext.Properties.GetProperties(false);
      if (properties2 != null)
        compositeProperties.Add((ReadOnlyPropertiesDictionary) properties2);
      compositeProperties.Add(GlobalContext.Properties.GetReadOnlyProperties());
      if (this.Option != null)
        PatternConverter.WriteObject(writer, (ILoggerRepository) null, compositeProperties[this.Option]);
      else
        PatternConverter.WriteDictionary(writer, (ILoggerRepository) null, (IDictionary) compositeProperties.Flatten());
    }
  }
}
