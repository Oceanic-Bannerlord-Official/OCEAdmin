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
    class Bring : ICommand
    {
        public Permissions CanUse() => Permissions.Admin;

        public string Command() => "!bring";

        public string Description() => "Brings another player to you. Usage !tp <Target User>";

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
                return new CommandFeedback(CommandLogType.Player, msg: "Target was not found!",
                    peer: networkPeer);
            }

            if (networkPeer.ControlledAgent != null && targetPeer.ControlledAgent != null) {
                Vec3 targetPos = networkPeer.ControlledAgent.Position;
                targetPos.x = targetPos.x + 1;
                targetPeer.ControlledAgent.TeleportToPosition( targetPos );

                return new CommandFeedback(CommandLogType.Both, 
                    msg: string.Format("** Command ** You have brought {0} to you.", targetPeer.UserName), peer: networkPeer,
                    targetMsg: string.Format("** Command ** {0} has brought you to them.", networkPeer.UserName), targetPeer: targetPeer);
            }

            return new CommandFeedback(CommandLogType.Player, msg: "Player is not alive. Can't bring.",
                    peer: networkPeer);
        }
    }
}
