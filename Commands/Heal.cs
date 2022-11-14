using NetworkMessages.FromServer;
using OCEAdmin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace OCEAdmin.Commands
{
    class Heal : Command
    {
        public bool CanUse(NetworkCommunicator networkPeer)
        {
            bool isAdmin = false;
            bool isExists = AdminManager.Admins.TryGetValue(networkPeer.VirtualPlayer.Id.ToString(), out isAdmin);
            return isExists && isAdmin;
        }

        public string Command()
        {
            return "!heal";
        }

        public string Description()
        {
            return "Heals a player. Use * to heal all.";
        }

        public bool Execute(NetworkCommunicator networkPeer, string[] args)
        {

            if (args.Length == 0)
            {
                GameNetwork.BeginModuleEventAsServer(networkPeer);
                GameNetwork.WriteMessage(new ServerMessage("Please provide a username. Player that contains provided input will be healed."));
                GameNetwork.EndModuleEventAsServer();
                return true;
            }

            if(args[0] == "*")
            {
                foreach (NetworkCommunicator peer in GameNetwork.NetworkPeers)
                {
                    if (peer.ControlledAgent != null)
                    {
                        peer.ControlledAgent.Health = peer.ControlledAgent.HealthLimit;
                    }
                }

                MPUtil.BroadcastToAdmins(string.Format("** Command ** {0} has healed all players.", networkPeer.UserName));

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
                GameNetwork.WriteMessage(new ServerMessage("Target player was not found!"));
                GameNetwork.EndModuleEventAsServer();
                return true;
            }

            if (targetPeer.ControlledAgent != null) {
                targetPeer.ControlledAgent.Health = targetPeer.ControlledAgent.HealthLimit;

                MPUtil.BroadcastToAdmins(string.Format("** Command ** {0} has healed {1}.", networkPeer.UserName, targetPeer.UserName));

                MPUtil.SendChatMessage(targetPeer, string.Format("** Command ** {0} has healed you.", networkPeer.UserName));
            }

            return true;
        }
    }
}
