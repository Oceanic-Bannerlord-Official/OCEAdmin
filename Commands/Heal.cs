using NetworkMessages.FromServer;
using OCEAdmin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace OCEAdmin.Commands
{
    class Heal : ICommand
    {
        public Permissions CanUse() => Permissions.Admin;

        public string Command() => "!heal";

        public string Description() => "Heals a player. Use * to heal all.";

        public CommandFeedback Execute(NetworkCommunicator networkPeer, string[] args)
        {
            if(args[0] == "*")
            {
                foreach (NetworkCommunicator peer in GameNetwork.NetworkPeers)
                {
                    if (peer.ControlledAgent != null)
                    {
                        peer.ControlledAgent.Health = peer.ControlledAgent.HealthLimit;
                    }
                }

                return new CommandFeedback(CommandLogType.BroadcastToAdmins, 
                    msg: string.Format("** Command ** {0} has healed all players.", networkPeer.UserName));
            }

            NetworkCommunicator targetPeer = MPUtil.GetPeerFromName(string.Join(" ", args));

            if (targetPeer == null)
            {
                return new CommandFeedback(CommandLogType.Player, msg: "Target was not found!",
                    peer: networkPeer);
            }

            if (targetPeer.ControlledAgent != null) {
                targetPeer.ControlledAgent.Health = targetPeer.ControlledAgent.HealthLimit;

                if(targetPeer.UserName != networkPeer.UserName)
                {
                    return new CommandFeedback(CommandLogType.BroadcastToAdminsAndTarget,
                        msg: string.Format("** Command ** {0} has healed {1}.", networkPeer.UserName, targetPeer.UserName),
                        targetMsg: string.Format("** Command ** {0} has healed you.", networkPeer.UserName), targetPeer: targetPeer);
                }
                else
                {
                    return new CommandFeedback(CommandLogType.BroadcastToAdmins, msg: string.Format("** Command ** {0} has healed themself.", networkPeer.UserName));
                }
            }

            return new CommandFeedback(CommandLogType.None);
        }
    }
}
