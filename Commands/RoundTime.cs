using NetworkMessages.FromServer;
using System;
using TaleWorlds.MountAndBlade;


namespace OCEAdmin.Commands
{
    class RoundTime : Command
    {
        public bool CanUse(NetworkCommunicator networkPeer)
        {
            bool isAdmin = false;
            bool isExists = AdminManager.Admins.TryGetValue(networkPeer.VirtualPlayer.Id.ToString(), out isAdmin);
            return isExists && isAdmin;
        }

        public string Command()
        {
            return "!roundtime";
        }

        public string Description()
        {
            return "Changes round time. !roundtime <new round time in minutes>";
        }

        public bool Execute(NetworkCommunicator networkPeer, string[] args)
        {
            MissionData curMission = AdminPanel.Instance.getMultiplayerOptionsState();

            if (curMission.gameType == "Duel" || curMission.gameType == "TeamDeathmatch")
            {
                GameNetwork.BeginModuleEventAsServer(networkPeer);
                GameNetwork.WriteMessage(new ServerMessage("You can't use this command in Duel or TDM."));
                GameNetwork.EndModuleEventAsServer();

                return true;
            }

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
            if(newRoundTime < 1)
            {
                GameNetwork.BeginModuleEventAsServer(networkPeer);
                GameNetwork.WriteMessage(new ServerMessage("Round time is not a number greater than zero"));
                GameNetwork.EndModuleEventAsServer();
                return true;
            }

            AdminPanel.Instance.SetRoundTime(newRoundTime);
            MPUtil.BroadcastToAdmins(string.Format("** Command ** {0} has adjusted the round time to {1} minute(s).", networkPeer.GetUsername(), args[0]));

            return true;
        }
    }
}