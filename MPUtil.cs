using NetworkMessages.FromServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade;

namespace ChatCommands
{
    public class MPUtil
    {
        public static void SendChatMessage(NetworkCommunicator networkPeer, string text) {
            GameNetwork.BeginModuleEventAsServer(networkPeer);
            GameNetwork.WriteMessage(new ServerMessage(text));
            GameNetwork.EndModuleEventAsServer();
        }

        public static string GetClanTag(NetworkCommunicator networkPeer)
        {
            string username = networkPeer.VirtualPlayer.UserName;
            return username.Substring(username.IndexOf("[") + 1, username.IndexOf("]") - 1);
        }

        public static bool IsInClan(NetworkCommunicator networkPeer)
        {
            if (networkPeer.VirtualPlayer.UserName.Substring(0, 1) == "[")
            {
                return true;
            }

            return false;
        }

        public static string GetPlayerID(NetworkCommunicator networkPeer)
        {
            return networkPeer.VirtualPlayer.Id.ToString();
        }
    }
}
