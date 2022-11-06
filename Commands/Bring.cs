using NetworkMessages.FromServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace OCEAdmin.Commands
{
    class Bring : Command
    {
        public bool CanUse(NetworkCommunicator networkPeer)
        {
            bool isAdmin = false;
            bool isExists = AdminManager.Admins.TryGetValue(networkPeer.VirtualPlayer.Id.ToString(), out isAdmin);
            return isExists && isAdmin;
        }

        public string Command()
        {
            return "!bring";
        }

        public string Description()
        {
            return "Brings another player to you. Usage !tp <Target User>";
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

            NetworkCommunicator targetPeer = null;
            foreach (NetworkCommunicator peer in GameNetwork.NetworkPeers)
            {
                if (peer.UserName.ToLower().Contains(string.Join(" ", args).ToLower()))
                {
                    targetPeer = peer;
                    break;
                }
            }
            if (targetPeer == null)
            {
                GameNetwork.BeginModuleEventAsServer(networkPeer);
                GameNetwork.WriteMessage(new ServerMessage("Target player not found"));
                GameNetwork.EndModuleEventAsServer();
                return true;
            }


            if (networkPeer.ControlledAgent != null && targetPeer.ControlledAgent != null) {
                Vec3 targetPos = networkPeer.ControlledAgent.Position;
                targetPos.x = targetPos.x + 1;
                targetPeer.ControlledAgent.TeleportToPosition( targetPos );

                MPUtil.SendChatMessage(targetPeer, string.Format("** Command ** {0} has brought you to them.", networkPeer.UserName));
                MPUtil.SendChatMessage(networkPeer, string.Format("** Command ** You have brought {0} to you.", targetPeer.UserName));
            }


            return true;
        }
    }
}
