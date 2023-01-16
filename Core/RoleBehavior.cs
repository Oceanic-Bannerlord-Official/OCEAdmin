using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade;

namespace OCEAdmin
{
    public class RoleBehavior : MissionNetwork
    { 
        protected override void HandleNewClientAfterSynchronized(NetworkCommunicator networkPeer)
        {
            base.HandleNewClientAfterSynchronized(networkPeer);

            RoleComponent component = networkPeer.AddRoleComponent();
            Role role = Role.Player;

            foreach (AdminPerms admin in ConfigManager.Instance.GetConfig().Admins)
            {
                if(networkPeer.VirtualPlayer.Id.ToString() == admin.PlayerId)
                {
                    Role.TryParse(admin.PermType, out role);
                }
            }

            component.UpdateRole(role);
        }

        protected override void HandlePlayerDisconnect(NetworkCommunicator networkPeer)
        {
            base.HandlePlayerDisconnect(networkPeer);
            networkPeer.RemoveRoleComponent();
        }
    }
}
