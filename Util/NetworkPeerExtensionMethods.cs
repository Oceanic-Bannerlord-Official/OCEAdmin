using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade;

namespace OCEAdmin
{
    public static class NetworkPeerExtensionMethods
    {
        public static string GetUsername(this NetworkCommunicator peer)
        {
            return peer.VirtualPlayer.UserName.ToString();
        }
    }
}
