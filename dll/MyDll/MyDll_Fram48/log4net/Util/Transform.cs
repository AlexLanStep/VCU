// Decompiled with JetBrains decompiler
// Type: log4net.Util.Transform
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using System.Text.RegularExpressions;
using System.Xml;

namespace log4net.Util
{
  public sealed class Transform
  {
    private const string CDATA_END = "]]>";
    private const string CDATA_UNESCAPABLE_TOKEN = "]]";
    private static Regex INVALIDCHARS = new Regex("[^\\x09\\x0A\\x0D\\x20-\\xFF\\u00FF-\\u07FF\\uE000-\\uFFFD]", RegexOptions.Compiled);

    private Transform()
    {
    }

    public static void WriteEscapedXmlString(
      XmlWriter writer,
      string textData,
      string invalidCharReplacement)
    {
      string text = Transform.MaskXmlInvalidCharacters(textData, invalidCharReplacement);
      int num1 = 12 * (1 + Transform.CountSubstrings(text, "]]>"));
      if (3 * (Transform.CountSubstrings(text, "<") + Transform.CountSubstrings(text, ">")) + 4 * Transform.CountSubstrings(text, "&") <= num1)
      {
        writer.WriteString(text);
      }
      else
      {
        int num2 = text.IndexOf("]]>");
        if (num2 < 0)
        {
          writer.WriteCData(text);
        }
        else
        {
          int startIndex = 0;
          for (; num2 > -1; num2 = text.IndexOf("]]>", startIndex))
          {
            writer.WriteCData(text.Substring(startIndex, num2 - startIndex));
            if (num2 == text.Length - 3)
            {
              startIndex = text.Length;
              writer.WriteString("]]>");
              break;
            }
            writer.WriteString("]]");
            startIndex = num2 + 2;
          }
          if (startIndex >= text.Length)
            return;
          writer.WriteCData(text.Substring(startIndex));
        }
      }
    }

    public static string MaskXmlInvalidCharacters(string textData, string mask) => Transform.INVALIDCHARS.Replace(textData, mask);

    private static int CountSubstrings(string text, string substring)
    {
      int num1 = 0;
      int startIndex = 0;
      int length1 = text.Length;
      int length2 = substring.Length;
      if (length1 == 0 || length2 == 0)
        return 0;
      int num2;
      for (; startIndex < length1; startIndex = num2 + length2)
      {
        num2 = text.IndexOf(substring, startIndex);
        if (num2 != -1)
          ++num1;
        else
          break;
      }
      return num1;
    }
  }
}
