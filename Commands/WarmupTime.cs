using NetworkMessages.FromServer;
using OCEAdmin.Core;
using System;
using TaleWorlds.MountAndBlade;


namespace OCEAdmin.Commands
{
    class WarmupTime : Command
    {
        public bool CanUse(NetworkCommunicator networkPeer)
        {
            bool isAdmin = false;
            bool isExists = AdminManager.Admins.TryGetValue(networkPeer.VirtualPlayer.Id.ToString(), out isAdmin);
            return isExists && isAdmin;
        }

        public string Command()
        {
            return "!warmuptime";
        }

        public string Description()
        {
            return "Changes warmuptime time. !warmuptime <new round time in minutes>";
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

            MPUtil.BroadcastToAdmins(string.Format("** Command ** {0} has adjusted the warmup timer to {1} minute(s).", networkPeer.UserName, args[0]));

            AdminPanel.Instance.SetWarmupTime(newRoundTime);

            return true;
        }
    }
}