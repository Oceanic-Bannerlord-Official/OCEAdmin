﻿using NetworkMessages.FromServer;
using OCEAdmin.Core;
using System;
using TaleWorlds.MountAndBlade;


namespace OCEAdmin.Commands
{
    class ChangeMap : Command
    {
        public bool CanUse(NetworkCommunicator networkPeer)
        {
            bool isAdmin = false;
            bool isExists = AdminManager.Admins.TryGetValue(networkPeer.VirtualPlayer.Id.ToString(), out isAdmin);
            return isExists && isAdmin;
        }

        public string Command()
        {
            return "!changemap";
        }

        public string Description()
        {
            return "Changes the map. Use !maps to see available map IDs. !chagemap <partial map id>";
        }

        public bool Execute(NetworkCommunicator networkPeer, string[] args)
        {
            // Obligatory argument check
            if (args.Length != 1)
            {
                GameNetwork.BeginModuleEventAsServer(networkPeer);
                GameNetwork.WriteMessage(new ServerMessage("Invalid number of arguments."));
                GameNetwork.EndModuleEventAsServer();
                return true;
            }
             
            string searchString = args[0];
            Tuple<bool, string> searchResult = AdminPanel.Instance.FindSingleMap(searchString);

            if(searchResult.Item1)
            {
                MPUtil.BroadcastToAdmins(string.Format("** Command ** {0} has initiated a map change to: {1}.", networkPeer.UserName, searchResult.Item2));

                AdminPanel.Instance.ChangeMap(searchResult.Item2);
            }
            else
            {
                GameNetwork.BeginModuleEventAsServer(networkPeer);
                GameNetwork.WriteMessage(new ServerMessage(searchResult.Item2));
                GameNetwork.EndModuleEventAsServer();
            }

            return true;
        }
    }
}