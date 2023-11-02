using OCEAdmin.Plugins.Admin;
using OCEAdmin.Plugins.Mutes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace OCEAdmin.Core.Permissions
{
    internal class PlayerGameHandler : GameHandler
    {
        public override void OnAfterSave() { }

        public override void OnBeforeSave() { }

        protected override void OnPlayerConnect(VirtualPlayer peer)
        {
            base.OnPlayerConnect(peer);

            NetworkCommunicator networkPeer = (NetworkCommunicator)peer.Communicator;

            networkPeer.Setup();

            SetupRole(networkPeer);
            CheckMute(networkPeer);
        }

        public void SetupRole(NetworkCommunicator networkPeer)
        {
            Player player = networkPeer.GetPlayer();
            Role role = Role.Player;

            AdminPlugin admins = OCEAdminSubModule.GetPlugin<AdminPlugin>();

            foreach (AdminPerms admin in admins.GetAdmins())
            {
                if (admin.PlayerId.Contains(networkPeer.VirtualPlayer.Id.Id2.ToString()))
                {
                    Role.TryParse(admin.PermType, out role);
                }
            }

            player.UpdateRole(role);

            MPUtil.WriteToConsole($"Role '{role}' added to " + networkPeer.GetUsername());
        }

        public void CheckMute(NetworkCommunicator networkPeer)
        {
            MutesPlugin mutes = OCEAdminSubModule.GetPlugin<MutesPlugin>();

            foreach (Mute mute in mutes.GetMutes())
            {
                if (networkPeer.VirtualPlayer.Id.ToString().Contains(mute.playerID))
                {
                    networkPeer.GetPlayer().Mute();
                }
            }
        }

        protected override void OnPlayerDisconnect(VirtualPlayer peer)
        {
            base.OnPlayerDisconnect(peer);

            NetworkCommunicator networkPeer = (NetworkCommunicator)peer.Communicator;
            networkPeer.DisposePlayer();
        }

    }
}
