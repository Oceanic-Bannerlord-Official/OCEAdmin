using NetworkMessages.FromServer;
using OCEAdmin.Core;
using System;
using TaleWorlds.MountAndBlade;


namespace OCEAdmin.Commands
{
    class WarmupTime : ICommand
    {
        public Permissions CanUse() => Permissions.Admin;

        public string Command() => "!warmuptime";

        public string Description() => "Changes warmuptime time. !warmuptime <new round time in minutes>";

        public CommandFeedback Execute(NetworkCommunicator networkPeer, string[] args)
        {
            // Obligatory argument check
            if (args.Length != 1)
            {
                return new CommandFeedback(CommandLogType.Player, msg: "Invalid number of arguments.",
                    peer: networkPeer);
            }

            int newRoundTime = -1;
            Int32.TryParse(args[0], out newRoundTime);
            if (newRoundTime < 1)
            {
                return new CommandFeedback(CommandLogType.Player, msg: "Time provided must be larger than zero.",
                    peer: networkPeer);
            }

            AdminPanel.Instance.SetWarmupTime(newRoundTime);

            return new CommandFeedback(CommandLogType.BroadcastToAdmins, 
                msg: string.Format("** Command ** {0} has adjusted the warmup timer to {1} minute(s).", networkPeer.UserName, args[0]));
        }
    }
}