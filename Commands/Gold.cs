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
    class Gold : ICommand
    {
        public Permissions CanUse() => Permissions.Admin;

        public string Command() => "!gold";

        public string Description() => "Set's a players gold. Usage !gold <player name> <amount>";
       
        public CommandFeedback Execute(NetworkCommunicator networkPeer, string[] args)
        {
            if (args.Length < 2) {
                return new CommandFeedback(CommandLogType.Player, msg: "Please provide a username and an amount.",
                    peer: networkPeer);
            }
            NetworkCommunicator targetPeer = MPUtil.GetPeerFromName(string.Join(" ", args.Take(args.Length -1 )));

            if (targetPeer == null) {
                return new CommandFeedback(CommandLogType.Player, msg: "Target was not found!",
                    peer: networkPeer);
            }

            int goldAmount;
            if(int.TryParse(args[1], out goldAmount))
            {
                MissionPeer mp = targetPeer.GetComponent<MissionPeer>();
                mp.Representative.UpdateGold(goldAmount);
            }

            return new CommandFeedback(CommandLogType.Player, msg: "Gold has been set!",
                peer: networkPeer);
        }
    }
}
