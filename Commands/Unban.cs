using NetworkMessages.FromServer;
using OCEAdmin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade;

namespace OCEAdmin.Commands
{
    class Unban : ICommand
    {
        public Permissions CanUse() => Permissions.Admin;

        public string Command() => "!unban";

        public string Description() => "Unbans a player. Usage !unban <Player Name>";

        public CommandFeedback Execute(NetworkCommunicator networkPeer, string[] args)
        {
            if (args.Length == 0)
            {
                return new CommandFeedback(CommandLogType.Player, msg: "Please provide a username.",
                    peer: networkPeer);
            }

            string[] banlist = BanManager.BanList();
            string username = string.Join(" ", args);
            int index = -1;
            for (int i = 0; i < banlist.Length; i++) {
                string ban = banlist[i];
                if (ban.Contains(username)) {
                    index = i;
                    break;
                }
            }

            if (index == -1) {
                return new CommandFeedback(CommandLogType.Player, msg: "Username not found on banlist!",
                    peer: networkPeer);
            }
            string[] newBanlist = banlist.Where((val, idx) => idx != index).ToArray();
            BanManager.UpdateList(newBanlist);

            return new CommandFeedback(CommandLogType.BroadcastToAdmins, 
                msg: string.Format("** Command ** {0} has unbanned {1}.", networkPeer.UserName, username));
        }
    }
}
