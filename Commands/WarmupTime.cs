using NetworkMessages.FromServer;
using System;
using TaleWorlds.MountAndBlade;


namespace ChatCommands.Commands
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

            AdminPanel.Instance.SetWarmupTime(newRoundTime);

            return true;
        }
    }
}