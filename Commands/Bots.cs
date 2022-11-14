﻿using NetworkMessages.FromServer;
using OCEAdmin.Core;
using System;
using TaleWorlds.MountAndBlade;


namespace OCEAdmin.Commands
{
    // To Test:
    // First/Second arg is not a number
    // First/Second Command less than 0

    class Bots : Command
    {
        public bool CanUse(NetworkCommunicator networkPeer)
        {
            bool isAdmin = false;
            bool isExists = AdminManager.Admins.TryGetValue(networkPeer.VirtualPlayer.Id.ToString(), out isAdmin);
            return isExists && isAdmin;
        }

        public string Command()
        {
            return "!bots";
        }

        public string Description()
        {
            return "Changes the number of bots. !bots <num bots team1> <num bots team2>";
        }

        public bool Execute(NetworkCommunicator networkPeer, string[] args)
        {
            // Obligatory argument check
            if (args.Length != 2)
            {
                GameNetwork.BeginModuleEventAsServer(networkPeer);
                GameNetwork.WriteMessage(new ServerMessage("Invalid number of arguments. There should only be two."));
                GameNetwork.EndModuleEventAsServer();
                return true;
            }
            int numBotsTeam1 = -1;
            if (!Int32.TryParse(args[0], out numBotsTeam1) || numBotsTeam1 < 0)
            {
                GameNetwork.BeginModuleEventAsServer(networkPeer);
                GameNetwork.WriteMessage(new ServerMessage("Team 1 cannot be a negative number."));
                GameNetwork.EndModuleEventAsServer();
                return true;
            }

            int numBotsTeam2 = -1;
            if (!Int32.TryParse(args[1], out numBotsTeam2) || numBotsTeam2 < 0)
            {
                GameNetwork.BeginModuleEventAsServer(networkPeer);
                GameNetwork.WriteMessage(new ServerMessage("Team 2 cannot be a negative number."));
                GameNetwork.EndModuleEventAsServer();
                return true;
            }

            AdminPanel.Instance.SetBots(numBotsTeam1, numBotsTeam2);

            GameNetwork.BeginModuleEventAsServer(networkPeer);
            GameNetwork.WriteMessage(new ServerMessage("Team1 Bots: "+numBotsTeam1.ToString()));
            GameNetwork.EndModuleEventAsServer();

            GameNetwork.BeginModuleEventAsServer(networkPeer);
            GameNetwork.WriteMessage(new ServerMessage("Team2 Bots: " + numBotsTeam2.ToString()));
            GameNetwork.EndModuleEventAsServer();

            return true;
        }
    }
}