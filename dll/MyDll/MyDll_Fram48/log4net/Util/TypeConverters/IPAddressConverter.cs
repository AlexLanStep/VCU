// Decompiled with JetBrains decompiler
// Type: log4net.Util.TypeConverters.IPAddressConverter
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using System;
using System.Net;

namespace log4net.Util.TypeConverters
{
  internal class IPAddressConverter : IConvertFrom
  {
    private static readonly char[] validIpAddressChars = new char[27]
    {
      '0',
      '1',
      '2',
      '3',
      '4',
      '5',
      '6',
      '7',
      '8',
      '9',
      'a',
      'b',
      'c',
      'd',
      'e',
      'f',
      'A',
      'B',
      'C',
      'D',
      'E',
      'F',
      'x',
      'X',
      '.',
      ':',
      '%'
    };

    public bool CanConvertFrom(Type sourceType) => sourceType == typeof (string);

    public object ConvertFrom(object source)
    {
      if (source is string str)
      {
        if (str.Length > 0)
        {
          try
          {
            if (str.Trim(IPAddressConverter.validIpAddressChars).Length == 0)
            {
              try
              {
                return (object) IPAddress.Parse(str);
              }
              catch (FormatException ex)
              {
              }
            }
            IPHostEntry hostByName = Dns.GetHostByName(str);
            if (hostByName != null)
            {
              if (hostByName.AddressList != null)
              {
                if (hostByName.AddressList.Length > 0)
                {
                  if (hostByName.AddressList[0] != null)
                    return (object) hostByName.AddressList[0];
                }
              }
            }
          }
          catch (Exception ex)
          {
            throw ConversionNotSupportedException.Create(typeof (IPAddress), source, ex);
          }
        }
      }
      throw ConversionNotSupportedException.Create(typeof (IPAddress), source);
    }
  }
}
