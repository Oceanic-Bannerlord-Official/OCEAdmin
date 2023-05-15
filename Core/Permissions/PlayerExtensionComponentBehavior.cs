using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade;

namespace OCEAdmin
{
    public class PlayerExtensionComponentBehavior : MissionNetwork
    { 
        protected override void HandleNewClientAfterSynchronized(NetworkCommunicator networkPeer)
        {
            base.HandleNewClientAfterSynchronized(networkPeer);

            PlayerExtensionComponent component = networkPeer.AddPlayerExtensionComponent();
            Role role = Role.Player;

            foreach (AdminPerms admin in ConfigManager.Instance.GetConfig().Admins)
            {
                if(networkPeer.VirtualPlayer.Id.ToString() == admin.PlayerId)
                {
                    Role.TryParse(admin.PermType, out role);
                }
            }

            foreach(Mute mute in MuteManager.GetMutes())
            {
                if(networkPeer.VirtualPlayer.Id.ToString().Contains(mute.playerID))
                {
                    component.Mute();
                }
            }

            component.UpdateRole(role);
            MPUtil.WriteToConsole($"Role '{role}' added to " + networkPeer.UserName);
        }

        protected override void HandlePlayerDisconnect(NetworkCommunicator networkPeer)
        {
            base.HandlePlayerDisconnect(networkPeer);
            networkPeer.RemovePlayerExtensionComponent();
        }
    }
}
