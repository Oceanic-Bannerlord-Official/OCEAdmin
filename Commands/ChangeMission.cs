using NetworkMessages.FromServer;
using System;
using TaleWorlds.MountAndBlade;
using OCEAdmin.Core;

namespace OCEAdmin.Commands
{
    class ChangeMission : ICommand
    {
        public Permissions CanUse() => Permissions.Admin;

        public string Command() => "!mission";

        public string Description() => "Changes the game type, map, and factions. !mission <game type> <map id> <team1 faction> <team2 faction>";

        bool ArgValid(Tuple<bool, string> args, NetworkCommunicator networkPeer, string messagePrefix = "")
        {
            if (!args.Item1)
            {
                GameNetwork.BeginModuleEventAsServer(networkPeer);
                GameNetwork.WriteMessage(new ServerMessage(messagePrefix + args.Item2));
                GameNetwork.EndModuleEventAsServer();
                return false;
            }
            return true;
        }

        public CommandFeedback Execute(NetworkCommunicator networkPeer, string[] args)
        {
            // Obligatory argument check
            if (args.Length != 4)
            {
                return new CommandFeedback(CommandLogType.Player, msg: "Invalid number of arguments.", peer: networkPeer);
            }

            // Validate the arguments
            string gameTypeSearchString = args[0];
            Tuple<bool, string> gameTypeSearchResult = AdminPanel.Instance.FindSingleGameType(gameTypeSearchString);
            if (!ArgValid(gameTypeSearchResult, networkPeer))
            {
                return new CommandFeedback(CommandLogType.None);
            }

            string mapSearchString = args[1];
            Tuple<bool, string> mapSearchResult = AdminPanel.Instance.FindMapForGameType(gameTypeSearchResult.Item2,mapSearchString);
            if (!ArgValid(mapSearchResult, networkPeer))
            {
                return new CommandFeedback(CommandLogType.None);
            }

            string faction1SearchString = args[2];
            Tuple<bool, string> faction1SearchResult = AdminPanel.Instance.FindSingleFaction(faction1SearchString);
            if (!ArgValid(faction1SearchResult, networkPeer, "Faction1: "))
            {
                return new CommandFeedback(CommandLogType.None);
            }

            string faction2SearchString = args[3];
            Tuple<bool, string> faction2SearchResult = AdminPanel.Instance.FindSingleFaction(faction2SearchString);
            if (!ArgValid(faction2SearchResult, networkPeer, "Faction2: "))
            {
                return new CommandFeedback(CommandLogType.None);
            }

            // All arguments are good, change the map and the factions
            string gameType = gameTypeSearchResult.Item2;
            string mapName = mapSearchResult.Item2;
            string faction1 = faction1SearchResult.Item2;
            string faction2 = faction2SearchResult.Item2;

            // Handle all command feedback in this command.
            MPUtil.SendChatMessage(networkPeer, string.Format("GameType: {0}", gameType));
            MPUtil.SendChatMessage(networkPeer, string.Format("Map: {0}", mapName));
            MPUtil.SendChatMessage(networkPeer, string.Format("Faction 1: {0}", faction1));
            MPUtil.SendChatMessage(networkPeer, string.Format("Faction 2: {0}", faction2));

            AdminPanel.Instance.ChangeGameTypeMapFactions(gameType, mapName, faction1, faction2);

            return new CommandFeedback(CommandLogType.None);
        }
    }
}