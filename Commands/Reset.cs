using NetworkMessages.FromServer;
using OCEAdmin.Core;
using System;
using TaleWorlds.MountAndBlade;


namespace OCEAdmin.Commands
{
    class Reset : ICommand
    {
        public Permissions CanUse() => Permissions.Admin;

        public string Command() => "!reset";

        public string Description() => "Resets the current mission. !reset";

        public CommandFeedback Execute(NetworkCommunicator networkPeer, string[] args)
        {
            AdminPanel.Instance.ResetMission();

            return new CommandFeedback(CommandLogType.BroadcastToAdmins,
                msg: string.Format("** Command ** {0} has reset the map.", networkPeer.UserName));
        }
    }
}