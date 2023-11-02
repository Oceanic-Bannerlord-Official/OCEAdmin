using NetworkMessages.FromServer;
using OCEAdmin.Core;
using OCEAdmin.Core.Permissions;
using OCEAdmin.Plugins.Bans;
using OCEAdmin.Plugins.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade;

namespace OCEAdmin.Plugins.Mutes
{
    class Unmute : PeerSearchCommand
    {
        public override Role CanUse() => Role.Admin;

        public override string Command() => "!unmute";

        public override string Description() => "Unmutes a player. Usage !unmute <Player Name>";

        public override CommandFeedback OnRunAction(NetworkCommunicator networkPeer, NetworkCommunicator targetPeer)
        {
            if (targetPeer.IsMuted()) {
                OCEAdminSubModule.GetPlugin<MutesPlugin>().RemoveMute(targetPeer.VirtualPlayer.Id.ToString());

                return new CommandFeedback(CommandLogType.BroadcastToAdmins,
                    msg: string.Format("** Command ** {0} has unmuted {1}.", 
                    networkPeer.GetUsername(), targetPeer.GetUsername()));
            }

            return new CommandFeedback(CommandLogType.Player, msg: "That player is not muted!",
                 peer: networkPeer);
        }

    }
}
