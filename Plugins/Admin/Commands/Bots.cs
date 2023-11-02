using NetworkMessages.FromServer;
using OCEAdmin.Core;
using OCEAdmin.Core.Missions;
using OCEAdmin.Plugins.Commands;
using System;
using TaleWorlds.MountAndBlade;


namespace OCEAdmin.Plugins.Admin
{
    class Bots : ICommand
    {
        public Role CanUse() => Role.Admin;
        public string Command() => "!bots";

        public string Description() => "Changes the number of bots. !bots <num bots team1> <num bots team2>";

        public CommandFeedback Execute(NetworkCommunicator networkPeer, string[] args)
        {
            // Obligatory argument check
            if (args.Length != 2)
            {
                return new CommandFeedback(CommandLogType.Player, msg: "Invalid number of arguments.",
                    peer: networkPeer);
            }

            int numBotsTeam1 = -1;
            if (!Int32.TryParse(args[0], out numBotsTeam1) || numBotsTeam1 < 0)
            {
                return new CommandFeedback(CommandLogType.Player, msg: "Team 1 cannot be a negative number.",
                    peer: networkPeer);
            }

            int numBotsTeam2 = -1;
            if (!Int32.TryParse(args[1], out numBotsTeam2) || numBotsTeam2 < 0)
            {
                return new CommandFeedback(CommandLogType.Player, msg: "Team 2 cannot be a negative number.",
                    peer: networkPeer);
            }

            MissionHooks.Instance.SetBots(numBotsTeam1, numBotsTeam2);

            MPUtil.SendChatMessage(networkPeer, string.Format("Team 1 Bots: {0}", numBotsTeam1.ToString()));
            MPUtil.SendChatMessage(networkPeer, string.Format("Team 2 Bots: {0}", numBotsTeam2.ToString()));

            // We handle the feedback here instead.
            return new CommandFeedback(CommandLogType.None);
        }
    }
}