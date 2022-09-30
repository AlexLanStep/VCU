// Decompiled with JetBrains decompiler
// Type: ETAS.EAS.SetupManagerException
// Assembly: etas.eas.setupmanager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=24eada690b1476fd
// MVID: 6F2445EF-C18D-4C24-B748-E3B896DC125F
// Assembly location: E:\LabCar\BasaDll\etas.eas.setupmanager.dll

using ETAS.EAS.Util;
using System;
using System.Runtime.Serialization;

namespace ETAS.EAS
{
  [Serializable]
  public class SetupManagerException : EASException
  {
    public SetupManagerException()
    {
    }

    public SetupManagerException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }

    public SetupManagerException(Exception innerException)
      : base(innerException)
    {
    }

    public SetupManagerException(SetupManagerErrorCode errorCode)
      : base((EASStatusCode) errorCode, (Exception) null, (object[]) null)
    {
    }

    public SetupManagerException(SetupManagerErrorCode errorCode, params object[] args)
      : base((EASStatusCode) errorCode, (Exception) null, args)
    {
    }

    public SetupManagerException(SetupManagerErrorCode errorCode, Exception innerException)
      : base((EASStatusCode) errorCode, innerException, (object[]) null)
    {
    }

    public SetupManagerException(
      SetupManagerErrorCode errorCode,
      Exception innerException,
      params object[] args)
      : base((EASStatusCode) errorCode, innerException, args)
    {
    }
  }
}
