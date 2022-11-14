using NetworkMessages.FromServer;
using OCEAdmin.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.DedicatedCustomServer;

namespace OCEAdmin.Commands
{
    class Ban : Command
    {
        public Permissions CanUse() => Permissions.Admin;

        public string Command() => "!ban";

        public string Description() =>  "Bans a player. First user that contains the provided input will be banned. Usage !ban <player name>";

        public CommandFeedback Execute(NetworkCommunicator networkPeer, string[] args)
        {
            if (args.Length == 0) {
                return new CommandFeedback(CommandLogType.Player,
                    msg: "Please provide a username. Any player that contains the provided input will be banned.", 
                    peer: networkPeer);
            }

            NetworkCommunicator targetPeer = MPUtil.GetPeerFromName(string.Join(" ", args));

            if (targetPeer == null) {
                return new CommandFeedback(CommandLogType.Player, msg: "Target player was not found.", 
                    peer: networkPeer);
            }
            
            using (StreamWriter sw = File.AppendText(BanManager.BanListPath()))
            {
                sw.WriteLine(targetPeer.UserName + "|" + targetPeer.VirtualPlayer.Id.ToString());
            }

            DedicatedCustomServerSubModule.Instance.DedicatedCustomGameServer.KickPlayer(targetPeer.VirtualPlayer.Id, false);

            return new CommandFeedback(CommandLogType.BroadcastToAdmins,
                msg: string.Format("** Command ** {0} has banned {1} from the server.", networkPeer.UserName, targetPeer.UserName));
        }
    }
}
