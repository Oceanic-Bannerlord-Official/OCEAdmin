using NetworkMessages.FromServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.DedicatedCustomServer;
using OCEAdmin.Core;

namespace OCEAdmin.Commands
{
    class Kick : PeerSearchCommand
    {
        public override Role CanUse() => Role.Admin;

        public override string Command() => "!kick";

        public override string Description() => "Kicks a player. First username that contains the provided input will be kicked. Usage !kick <player name>";

        public override CommandFeedback OnRunAction(NetworkCommunicator networkPeer, NetworkCommunicator targetPeer)
        {
            DedicatedCustomServerSubModule.Instance.DedicatedCustomGameServer.KickPlayer(targetPeer.VirtualPlayer.Id, false);

            return new CommandFeedback(CommandLogType.BroadcastToAdmins,
                msg: string.Format("** Command ** {0} has kicked {1} from the server.", networkPeer.UserName, targetPeer.UserName));
        }
    }
}
