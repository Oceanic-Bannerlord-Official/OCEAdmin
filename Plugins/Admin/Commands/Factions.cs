using NetworkMessages.FromServer;
using System;
using System.Collections.Generic;
using TaleWorlds.MountAndBlade;
using OCEAdmin.Core;
using OCEAdmin.Core.Missions;
using OCEAdmin.Plugins.Commands;

namespace OCEAdmin.Plugins.Admin
{

    class Factions : ICommand
    {
        public Role CanUse() => Role.Admin;

        public string Command() => "!factions";

        public string Description() => "Lists available factions. !factions";

        public CommandFeedback Execute(NetworkCommunicator networkPeer, string[] args)
        {
            List<string> availableFactions = MissionHooks.Instance.GetAllFactions();

            if(args.Length > 0)
            {
                Tuple<bool,string> found = MissionHooks.Instance.FindSingleFaction(args[0]);
                if(found.Item1)
                {
                    return new CommandFeedback(CommandLogType.Player, found.Item2, peer: networkPeer);
                }
                else
                {
                    return new CommandFeedback(CommandLogType.Player, "No factions have been found.", peer: networkPeer);
                }
                
            }

            MPUtil.SendChatMessage(networkPeer, "Factions:");

            foreach (var faction in availableFactions)
            {
                MPUtil.SendChatMessage(networkPeer, faction);
            }

            string team1Faction = "";
            MultiplayerOptions.Instance.GetOptionFromOptionType(MultiplayerOptions.OptionType.CultureTeam1).GetValue(out team1Faction);

            string team2Faction = "";
            MultiplayerOptions.Instance.GetOptionFromOptionType(MultiplayerOptions.OptionType.CultureTeam2).GetValue(out team2Faction);

            return new CommandFeedback(CommandLogType.Player, 
                msg: string.Format("Current factions: {0} vs {1}", team1Faction, team2Faction),
                peer: networkPeer);
        }
    }
}
