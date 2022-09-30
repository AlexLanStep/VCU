
using System.Runtime.Remoting.Channels;
using System.Net.Sockets;
using System.Net.Cache;

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
