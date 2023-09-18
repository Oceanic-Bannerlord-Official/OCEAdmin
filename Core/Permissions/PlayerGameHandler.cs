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

            foreach (AdminPerms admin in AdminManager.GetAdmins())
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
            foreach (Mute mute in MuteManager.GetMutes())
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
