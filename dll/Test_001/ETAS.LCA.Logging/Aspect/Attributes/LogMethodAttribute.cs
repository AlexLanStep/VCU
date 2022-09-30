// Decompiled with JetBrains decompiler
// Type: ETAS.LCA.Logging.Aspect.Attributes.LogMethodAttribute
// Assembly: ETAS.LCA.Logging, Version=1.0.0.0, Culture=neutral, PublicKeyToken=24eada690b1476fd
// MVID: 3E0EA91C-5947-4488-8BAF-799929BC7052
// Assembly location: E:\LabCar\BasaDll\ETAS.LCA.Logging.dll

using System;
using System.Collections;
using System.Runtime.Remoting.Messaging;

namespace ETAS.LCA.Logging.Aspect.Attributes
{
  [AttributeUsage(AttributeTargets.Method)]
  public class LogMethodAttribute : Attribute
  {
    public void log(IMethodMessage methodMsg)
    {
      Console.WriteLine("Begin aspect injection");
      IDictionaryEnumerator enumerator = methodMsg.Properties.GetEnumerator();
      while (enumerator.MoveNext())
      {
        string str = enumerator.Key.ToString();
        object obj = enumerator.Value;
        if (str == "__Args")
        {
          object[] objArray = (object[]) obj;
          for (int index = 0; index < objArray.Length; ++index)
            Console.WriteLine("Arg [{0}] = {1}", (object) index, (object) objArray[index].ToString());
        }
        if (str == "__MethodSignature" && obj != null)
        {
          object[] objArray = (object[]) obj;
          for (int index = 0; index < objArray.Length; ++index)
            Console.WriteLine("Param [{0}] = {1}", (object) index, (object) objArray[index].ToString());
        }
      }
      Console.WriteLine("End aspect injection");
    }
  }
}
