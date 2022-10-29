using NetworkMessages.FromServer;
using System;
using TaleWorlds.MountAndBlade;


namespace OCEAdmin.Commands
{
    class MapTime : Command
    {
        public bool CanUse(NetworkCommunicator networkPeer)
        {
            bool isAdmin = false;
            bool isExists = AdminManager.Admins.TryGetValue(networkPeer.VirtualPlayer.Id.ToString(), out isAdmin);
            return isExists && isAdmin;
        }

        public string Command()
        {
            return "!maptime";
        }

        public string Description()
        {
            return "Changes map time. Used in TDM and Duel. !maptime <new round time in minutes>";
        }

        public bool Execute(NetworkCommunicator networkPeer, string[] args)
        {
            // Obligatory argument check
            if (args.Length != 1)
            {
                GameNetwork.BeginModuleEventAsServer(networkPeer);
                GameNetwork.WriteMessage(new ServerMessage("Invalid number of arguments"));
                GameNetwork.EndModuleEventAsServer();
                return true;
            }

            int newRoundTime = -1;
            Int32.TryParse(args[0], out newRoundTime);
            if (newRoundTime < 1)
            {
                GameNetwork.BeginModuleEventAsServer(networkPeer);
                GameNetwork.WriteMessage(new ServerMessage("Time provided is not a number greater than zero"));
                GameNetwork.EndModuleEventAsServer();
                return true;
            }

            AdminPanel.Instance.SetMapTime(newRoundTime);

            MPUtil.BroadcastToAdmins(string.Format("** Command ** {0} has adjusted the map time to {1} minute(s).", networkPeer.UserName, args[0]));

            return true;
        }
    }
}