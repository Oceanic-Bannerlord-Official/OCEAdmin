using NetworkMessages.FromServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using OCEAdmin.Core;
using TaleWorlds.Diamond;

namespace OCEAdmin.Commands
{
    class Kill : PeerSearchCommand
    {
        public override Role CanUse() => Role.Admin;

        public override string Command() => "!kill";

        public override string Description() => "Kills a provided username. Usage !kill <player name>";

        public override CommandFeedback OnRunAction(NetworkCommunicator networkPeer, NetworkCommunicator targetPeer)
        {
            if (targetPeer.ControlledAgent != null)
            {
                MPUtil.Slay(targetPeer.ControlledAgent);

                return new CommandFeedback(CommandLogType.BroadcastToAdminsAndTarget,
                    msg: string.Format("** Command ** {0} has slayed {1}.", networkPeer.UserName, targetPeer.UserName),
                    targetMsg: "** Command ** You have been slain.", targetPeer: targetPeer);
            }

            return new CommandFeedback(CommandLogType.Player, msg: "Target is no longer alive!", peer: networkPeer);
        }
    }
}
