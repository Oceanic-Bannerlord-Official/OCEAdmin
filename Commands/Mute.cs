using OCEAdmin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade.DedicatedCustomServer;
using TaleWorlds.MountAndBlade;

namespace OCEAdmin.Commands
{
    class Mute : PeerSearchCommand
    {
        public override Role CanUse() => Role.Admin;

        public override string Command() => "!mute";

        public override string Description() => "Temporarily mutes a player. Usage !tempmute <player name>";

        public override CommandFeedback OnRunAction(NetworkCommunicator networkPeer, NetworkCommunicator targetPeer)
        {
            // Mute the player for the session only.
            MuteManager.AddMute(new OCEAdmin.Mute(playerID: targetPeer.VirtualPlayer.Id.ToString(),
                adminID: networkPeer.VirtualPlayer.Id.ToString(),
                nickname: targetPeer.GetUsername()));

            return new CommandFeedback(CommandLogType.BroadcastToAdminsAndTarget,
                msg: string.Format("** Command ** {0} has muted {1} indefinitely.", networkPeer.GetUsername(), targetPeer.GetUsername()),
                targetMsg: string.Format("** Command ** {0} has muted you indefinitely.", networkPeer.GetUsername()), targetPeer: targetPeer);
        }
    }
}
