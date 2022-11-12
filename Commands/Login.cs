using NetworkMessages.FromServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade;
using OCEAdmin.Core;

namespace OCEAdmin.Commands
{
    class Login : Command
    {
        public bool CanUse(NetworkCommunicator networkPeer)
        {
            return true;
        }

        public string Command()
        {
            return "!login";
        }

        public string Description()
        {
            return "Used to login with the admin password. Usage !login <password>";
        }

        public bool Execute(NetworkCommunicator networkPeer, string[] args)
        {
            if(!ConfigManager.Instance.AllowLoginCommand)
            {
                MPUtil.SendChatMessage(networkPeer, 
                    "** Command ** The login command has been disabled from the config file.");
                return true;
            }

            if (args.Length == 0 || args.Length > 1) {
                GameNetwork.BeginModuleEventAsServer(networkPeer);
                GameNetwork.WriteMessage(new ServerMessage("Please only provide a password. Usage: !login <password>"));
                GameNetwork.EndModuleEventAsServer();
            }
            String password = args[0];
            Config config = ConfigManager.Instance;
            if (!password.Equals(config.AdminPassword)) {
                GameNetwork.BeginModuleEventAsServer(networkPeer);
                GameNetwork.WriteMessage(new ServerMessage("Incorrect password."));
                GameNetwork.EndModuleEventAsServer();
                return true;
            }

            MPUtil.BroadcastToAdmins(string.Format("** Login ** {0} has logged in with the admin password!", networkPeer.UserName));
            
            AdminManager.Admins.Add(networkPeer.VirtualPlayer.Id.ToString(), true);
            GameNetwork.BeginModuleEventAsServer(networkPeer);
            GameNetwork.WriteMessage(new ServerMessage("Login successful. Welcome!"));
            GameNetwork.EndModuleEventAsServer();
            return true;
        }
    }
}
