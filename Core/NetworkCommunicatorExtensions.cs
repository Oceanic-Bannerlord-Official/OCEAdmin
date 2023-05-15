using OCEAdmin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade;

namespace OCEAdmin
{
    public static class NetworkCommunicatorExtensions
    {
        public static PlayerExtensionComponent GetPlayerExtensionComponent(this NetworkCommunicator networkPeer)
        {
            return PlayerExtensionComponent.GetFor(networkPeer);
        }

        public static void RemovePlayerExtensionComponent(this NetworkCommunicator networkPeer)
        {
            PlayerExtensionComponent.RemoveAt(networkPeer);
        }

        public static PlayerExtensionComponent AddPlayerExtensionComponent(this NetworkCommunicator networkPeer)
        {
            return PlayerExtensionComponent.Create(networkPeer);
        }

        public static bool IsBanned(this NetworkCommunicator networkPeer)
        {
            return BanManager.IsBanned(networkPeer);
        }

        public static bool IsMuted(this NetworkCommunicator networkPeer)
        {
            return networkPeer.GetPlayerExtensionComponent().IsMuted();
        }
    }
}
