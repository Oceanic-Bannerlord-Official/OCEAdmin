using NetworkMessages.FromServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace OCEAdmin
{
    public class MPUtil
    {
        private static Random random = new Random();

        public static void WriteToConsole(string text) {
            Debug.Print(string.Format("[OCEAdmin] - " + text), 0, Debug.DebugColor.Green);
        }

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

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

        public static string GetUnitID(NetworkCommunicator networkPeer)
        {
            return networkPeer.ControlledAgent.Character.StringId;
        }
    }
}
