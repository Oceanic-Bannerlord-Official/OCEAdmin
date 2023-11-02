using OCEAdmin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade.DedicatedCustomServer;
using TaleWorlds.MountAndBlade;
using OCEAdmin.Plugins.Commands;
using OCEAdmin.Core.Permissions;

namespace OCEAdmin.Plugins.Mutes
{
    class TempMute : PeerSearchCommand
    {
        public override Role CanUse() => Role.Admin;

        public override string Command() => "!tempmute";

        public override string Description() => "Temporarily mutes a player. Usage !tempmute <player name>";

        public override CommandFeedback OnRunAction(NetworkCommunicator networkPeer, NetworkCommunicator targetPeer)
        {
            MutesPlugin mutes = OCEAdminSubModule.GetPlugin<MutesPlugin>();

            // Mute the player for the session only.
            mutes.TempMute(new Mute(playerID: targetPeer.VirtualPlayer.Id.ToString(),
                adminID: networkPeer.VirtualPlayer.Id.ToString(),
                nickname: targetPeer.GetUsername()));

            return new CommandFeedback(CommandLogType.BroadcastToAdminsAndTarget,
                msg: string.Format("** Command ** {0} has muted {1} for the session.", networkPeer.GetUsername(), targetPeer.GetUsername()),
                targetMsg: string.Format("** Command ** {0} has muted you for the session.", networkPeer.GetUsername()), targetPeer: targetPeer);
        }
    }
}
