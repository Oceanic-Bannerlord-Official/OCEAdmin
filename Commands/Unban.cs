﻿using NetworkMessages.FromServer;
using OCEAdmin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade;

namespace OCEAdmin.Commands
{
    class Unban : Command
    {
        public bool CanUse(NetworkCommunicator networkPeer)
        {
            bool isAdmin = false;
            bool isExists = AdminManager.Admins.TryGetValue(networkPeer.VirtualPlayer.Id.ToString(), out isAdmin);
            return isExists && isAdmin;
        }

        public string Command()
        {
            return "!unban";
        }

        public string Description()
        {
            return "Unbans a player. Usage !unban <Player Name>";
        }

        public bool Execute(NetworkCommunicator networkPeer, string[] args)
        {
            if (args.Length == 0)
            {
                GameNetwork.BeginModuleEventAsServer(networkPeer);
                GameNetwork.WriteMessage(new ServerMessage("Please provide a username."));
                GameNetwork.EndModuleEventAsServer();
                return true;
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
                GameNetwork.BeginModuleEventAsServer(networkPeer);
                GameNetwork.WriteMessage(new ServerMessage("Username was not found!"));
                GameNetwork.EndModuleEventAsServer();
                return true;
            }
            string[] newBanlist = banlist.Where((val, idx) => idx != index).ToArray();
            BanManager.UpdateList(newBanlist);

            MPUtil.BroadcastToAdmins(string.Format("** Command ** {0} has unbanned {1}.", networkPeer.UserName, username));

            return true;
        }
    }
}
