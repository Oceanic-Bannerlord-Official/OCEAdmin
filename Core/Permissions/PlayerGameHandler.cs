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

            Player player = networkPeer.GetPlayer();
            Role role = Role.Player;

            foreach (AdminPerms admin in AdminManager.GetAdmins())
            {
                if (networkPeer.VirtualPlayer.Id.ToString() == admin.PlayerId)
                {
                    Role.TryParse(admin.PermType, out role);
                }
            }

            foreach (Mute mute in MuteManager.GetMutes())
            {
                if (peer.Id.ToString().Contains(mute.playerID))
                {
                    player.Mute();
                }
            }

            player.UpdateRole(role);
            MPUtil.WriteToConsole($"Role '{role}' added to " + networkPeer.GetUsername());
        }

        protected override void OnPlayerDisconnect(VirtualPlayer peer)
        {
            base.OnPlayerDisconnect(peer);

            NetworkCommunicator networkPeer = (NetworkCommunicator)peer.Communicator;
            networkPeer.DisposePlayer();
        }

    }
}
