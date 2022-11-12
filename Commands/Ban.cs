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
        public bool CanUse(NetworkCommunicator networkPeer)
        {
            bool isAdmin = false;
            bool isExists = AdminManager.Admins.TryGetValue(networkPeer.VirtualPlayer.Id.ToString(), out isAdmin);
            return isExists && isAdmin;
        }

        public string Command()
        {
            return "!ban";
        }

        public string Description()
        {
            return "Bans a player. First user that contains the provided input will be banned. Usage !ban <player name>";
        }

        public bool Execute(NetworkCommunicator networkPeer, string[] args)
        {
            if (args.Length == 0) {
                GameNetwork.BeginModuleEventAsServer(networkPeer);
                GameNetwork.WriteMessage(new ServerMessage("Please provide a username. Any player that contains the provided input will be banned."));
                GameNetwork.EndModuleEventAsServer();
                return true;
            }

            NetworkCommunicator targetPeer = null;
            foreach (NetworkCommunicator peer in GameNetwork.NetworkPeers) {
                if(peer.UserName.ToLower().Contains(string.Join(" ", args).ToLower())) {
                    targetPeer = peer;
                    break;
                }
            }
            if (targetPeer == null) {
                GameNetwork.BeginModuleEventAsServer(networkPeer);
                GameNetwork.WriteMessage(new ServerMessage("Target player was not found."));
                GameNetwork.EndModuleEventAsServer();
                return true;
            }
            
            using (StreamWriter sw = File.AppendText(BanManager.BanListPath()))
            {
                sw.WriteLine(targetPeer.UserName + "|" + targetPeer.VirtualPlayer.Id.ToString());
            }

            MPUtil.BroadcastToAdmins(string.Format("** Command ** {0} has banned {1} from the server.", networkPeer.UserName, targetPeer.UserName));

            DedicatedCustomServerSubModule.Instance.DedicatedCustomGameServer.KickPlayer(targetPeer.VirtualPlayer.Id, false);
            return true;
            // throw new NotImplementedException();
        }
    }
}
