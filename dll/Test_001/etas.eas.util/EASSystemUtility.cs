// Decompiled with JetBrains decompiler
// Type: ETAS.EAS.Util.EASSystemUtility
// Assembly: etas.eas.util, Version=1.0.0.0, Culture=neutral, PublicKeyToken=24eada690b1476fd
// MVID: C6C1AF2C-DE3A-40A9-BDCC-7E76498937A9
// Assembly location: E:\LabCar\BasaDll\etas.eas.util.dll

using System;
using System.Text.RegularExpressions;

namespace ETAS.EAS.Util
{
  public sealed class EASSystemUtility
  {
    private EASSystemUtility()
    {
    }

    public static string resolveEnvironmentPath(string path)
    {
      string str1 = (string) path.Clone();
      if (str1.StartsWith("%") || str1.StartsWith("${"))
      {
        string input = str1.Replace("\\", "/");
        Match match = new Regex(!input.StartsWith("%") ? "^\\$\\{(?<envvariable>.*)\\}(?<remaining>.*)" : "^%(?<envvariable>.*)%(?<remaining>.*)").Match(input);
        if (match.Success)
        {
          string environmentVariable = Environment.GetEnvironmentVariable(match.Result("${envvariable}"));
          if (environmentVariable != null && !environmentVariable.Equals(""))
          {
            string str2 = match.Result("${remaining}");
            str1 = environmentVariable + str2;
          }
          else
            str1 = path;
        }
        else
          str1 = path;
      }
      return str1;
    }
  }
}
