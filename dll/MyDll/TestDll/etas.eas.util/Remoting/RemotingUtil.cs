//using System.Runtime.Remoting.Channels;
//using System.Runtime.Remoting.Channels.Tcp;
//using System.Threading.Channels;
//using System.Threading.Channels;

using System.Net.Sockets;

namespace ETAS.EAS.Util.Remoting;

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
