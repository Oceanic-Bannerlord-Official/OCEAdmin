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
    class AdminChat : Command
    {
        public bool CanUse(NetworkCommunicator networkPeer)
        {
            bool isAdmin = false;
            bool isExists = AdminManager.Admins.TryGetValue(networkPeer.VirtualPlayer.Id.ToString(), out isAdmin);
            return isExists && isAdmin;
        }

        public string Command()
        {
            return "!a";
        }

        public string Description()
        {
            return "Type in admin chat. !a <string text>";
        }
        
        public bool Execute(NetworkCommunicator networkPeer, string[] args)
        {
            String text = string.Join(" ", args);

            foreach (NetworkCommunicator peer in GameNetwork.NetworkPeers)
            {
                bool isAdmin = false;
                bool isExists = AdminManager.Admins.TryGetValue(peer.VirtualPlayer.Id.ToString(), out isAdmin);

                if(isAdmin && isExists)
                {
                    string senderName = networkPeer.VirtualPlayer.UserName.ToString();

                    if (peer.IsSynchronized) {
                        GameNetwork.BeginModuleEventAsServer(peer);
                        GameNetwork.WriteMessage(new ServerMessage(string.Format("(Admin) {0}: {1}", senderName, text)));
                        GameNetwork.EndModuleEventAsServer();
                    }
                }
            }

            return true;
        }
    }
}
