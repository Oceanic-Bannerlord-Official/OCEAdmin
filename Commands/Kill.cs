using NetworkMessages.FromServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using OCEAdmin.Core;

namespace OCEAdmin.Commands
{
    class Kill : Command
    {
        public Permissions CanUse() => Permissions.Admin;

        public string Command() => "!kill";

        public string Description() => "Kills a provided username. Usage !kill <player name>";

        public CommandFeedback Execute(NetworkCommunicator networkPeer, string[] args)
        {
            if (args.Length == 0)
            {
                return new CommandFeedback(CommandLogType.Player, msg: "Please provide a username.",
                    peer: networkPeer);
            }

            NetworkCommunicator targetPeer = MPUtil.GetPeerFromName(string.Join(" ", args));

            if (targetPeer == null)
            {
                return new CommandFeedback(CommandLogType.Player, msg: "Target player was not found!",
                    peer: networkPeer);
            }

            if (!targetPeer.ControlledAgent.Equals(null)) {
                MPUtil.Slay(targetPeer);

                return new CommandFeedback(CommandLogType.BroadcastToAdminsAndTarget, 
                    msg: string.Format("** Command ** {0} has slayed {1}.", networkPeer.UserName, targetPeer.UserName),
                    targetMsg: "** Command ** You have been slain.", targetPeer: targetPeer);
            }

            return new CommandFeedback(CommandLogType.Player, msg: "Target is not alive!", peer: networkPeer);
        }
    }
}
