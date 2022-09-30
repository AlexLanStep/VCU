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
      string str1 = (string)path.Clone();
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
