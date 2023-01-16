using NetworkMessages.FromServer;
using OCEAdmin.Core;
using System;
using TaleWorlds.MountAndBlade;


namespace OCEAdmin.Commands
{
    class ChangeMap : ICommand
    {
        public Role CanUse() => Role.Admin;

        public string Command() => "!changemap";

        public string Description() => "Changes the map. Use !maps to see available map IDs. !changemap <partial map id>";

        public CommandFeedback Execute(NetworkCommunicator networkPeer, string[] args)
        {
            // Obligatory argument check
            if (args.Length != 1)
            {
                return new CommandFeedback(CommandLogType.Player, msg: "Invalid number of arguments.", peer: networkPeer);
            }
             
            string searchString = args[0];
            Tuple<bool, string> searchResult = AdminPanel.Instance.FindSingleMap(searchString);

            if(searchResult.Item1)
            {
                AdminPanel.Instance.ChangeMap(searchResult.Item2);

                return new CommandFeedback(CommandLogType.BroadcastToAdmins, msg:
                    string.Format("** Command ** {0} has initiated a map change to: {1}.", networkPeer.UserName, searchResult.Item2));
            }

            return new CommandFeedback(CommandLogType.Player, msg: searchResult.Item2, peer: networkPeer);
        }
    }
}