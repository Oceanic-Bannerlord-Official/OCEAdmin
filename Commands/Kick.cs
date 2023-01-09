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
    class OCEAdmin : ICommand
    {
        public Permissions CanUse() => Permissions.Admin;

        public string Command() => "!kick";

        public string Description() => "Kicks a player. First username that contains the provided input will be kicked. Usage !kick <player name>";

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

            DedicatedCustomServerSubModule.Instance.DedicatedCustomGameServer.KickPlayer(targetPeer.VirtualPlayer.Id, false);

            return new CommandFeedback(CommandLogType.BroadcastToAdmins,
                msg: string.Format("** Command ** {0} has kicked {1} from the server.", networkPeer.UserName, targetPeer.UserName));
        }
    }
}
