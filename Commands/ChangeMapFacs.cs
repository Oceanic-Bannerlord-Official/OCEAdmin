using NetworkMessages.FromServer;
using System;
using TaleWorlds.MountAndBlade;
using OCEAdmin.Core;

namespace OCEAdmin.Commands
{
    class ChangeMapFacs : ICommand
    {
        public Role CanUse() => Role.Admin;

        public string Command() => "!mapfacs";

        public string Description() => "Changes the map and the team factions. !mapfacs <map id> <team1 faction> <team2 faction>";

        bool ArgValid(Tuple<bool,string> args, NetworkCommunicator networkPeer, string messagePrefix="")
        {
            if(!args.Item1)
            {
                GameNetwork.BeginModuleEventAsServer(networkPeer);
                GameNetwork.WriteMessage(new ServerMessage(messagePrefix + args.Item2));
                GameNetwork.EndModuleEventAsServer();

                return false;
            }
            return true;
        }

        // Legacy command. Fully implement into command feedback system at later date.
        public CommandFeedback Execute(NetworkCommunicator networkPeer, string[] args)
        {
            // Obligatory argument check
            if (args.Length != 3)
            {
                return new CommandFeedback(CommandLogType.Player, msg: "Invalid number of arguments.", peer: networkPeer);
            }

            string mapSearchString = args[0];
            Tuple<bool, string> mapSearchResult = AdminPanel.Instance.FindSingleMap(mapSearchString);
            if(!ArgValid(mapSearchResult,networkPeer))
            {
                return new CommandFeedback(CommandLogType.None);
            }

            string faction1SearchString = args[1];
            Tuple<bool, string> faction1SearchResult = AdminPanel.Instance.FindSingleFaction(faction1SearchString);
            if (!ArgValid(faction1SearchResult, networkPeer,"Faction1: "))
            {
                return new CommandFeedback(CommandLogType.None);
            }

            string faction2SearchString = args[2];
            Tuple<bool, string> faction2SearchResult = AdminPanel.Instance.FindSingleFaction(faction2SearchString);
            if (!ArgValid(faction2SearchResult, networkPeer, "Faction2: "))
            {
                return new CommandFeedback(CommandLogType.None);
            }

            // All arguments are good, change the map and the factions
            string mapName = mapSearchResult.Item2;
            string faction1 = faction1SearchResult.Item2;
            string faction2 = faction2SearchResult.Item2;

            // Handle all command feedback in this command.
            MPUtil.SendChatMessage(networkPeer, string.Format("Map: {0}", mapName));
            MPUtil.SendChatMessage(networkPeer, string.Format("Faction 1: {0}", faction1));
            MPUtil.SendChatMessage(networkPeer, string.Format("Faction 2: {0}", faction2));

            AdminPanel.Instance.ChangeMapAndFactions(mapName, faction1, faction2);

            return new CommandFeedback(CommandLogType.None);
        }
    }
}