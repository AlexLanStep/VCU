// Decompiled with JetBrains decompiler
// Type: ETAS.EAS.Util.Remoting.RemotingUtil
// Assembly: etas.eas.util, Version=1.0.0.0, Culture=neutral, PublicKeyToken=24eada690b1476fd
// MVID: C6C1AF2C-DE3A-40A9-BDCC-7E76498937A9
// Assembly location: E:\LabCar\BasaDll\etas.eas.util.dll

using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;

namespace ETAS.EAS.Util.Remoting
{
  public class RemotingUtil
  {
    public static bool IsChannelRegistered(TcpChannel channel)
    {
      foreach (IChannel registeredChannel in ChannelServices.RegisteredChannels)
      {
        if (registeredChannel == channel || registeredChannel.ChannelName.Equals(channel.ChannelName))
          return true;
      }
      return false;
    }
  }
}
