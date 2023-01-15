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
    class Goto : ICommand
    {
        public Permissions CanUse() => Permissions.Admin;
        public string Command() => "!goto";
        
        public string Description() => "Teleport yourself to another. Usage !tp <Target User>";

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
                Vec3 targetPos = targetPeer.ControlledAgent.Position;
                targetPos.x = targetPos.x + 1;
                networkPeer.ControlledAgent.TeleportToPosition( targetPos );
            }

            return new CommandFeedback(CommandLogType.None);
        }
    }
}
