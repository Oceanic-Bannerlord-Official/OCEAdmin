using NetworkMessages.FromServer;
using System;
using TaleWorlds.MountAndBlade;


namespace OCEAdmin.Commands
{

    class Id : Command
    {
        public Permissions CanUse() => Permissions.Player;
        public string Command() => "!id";

        public string Description() => "Returns your unique ID into the chatbox.";

        public CommandFeedback Execute(NetworkCommunicator networkPeer, string[] args)
        {
            return new CommandFeedback(CommandLogType.Player, msg: networkPeer.PlayerConnectionInfo.PlayerID.ToString(),
                peer: networkPeer);
        }
    }
}