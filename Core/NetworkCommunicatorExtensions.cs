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
        public static RoleComponent GetRoleComponent(this NetworkCommunicator networkPeer)
        {
            return RoleComponent.GetFor(networkPeer);
        }

        public static void RemoveRoleComponent(this NetworkCommunicator networkPeer)
        {
            RoleComponent.RemoveAt(networkPeer);
        }

        public static RoleComponent AddRoleComponent(this NetworkCommunicator networkPeer)
        {
            return RoleComponent.Create(networkPeer);
        }

        public static bool IsBanned(this NetworkCommunicator networkPeer)
        {
            return BanManager.IsBanned(networkPeer);
        }
    }
}
