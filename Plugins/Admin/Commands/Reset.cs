using NetworkMessages.FromServer;
using OCEAdmin.Core;
using OCEAdmin.Core.Missions;
using OCEAdmin.Core.Permissions;
using OCEAdmin.Plugins.Commands;
using System;
using TaleWorlds.MountAndBlade;


namespace OCEAdmin.Plugins.Admin
{
    class Reset : ICommand
    {
        public Role CanUse() => Role.Admin;

        public string Command() => "!reset";

        public string Description() => "Resets the current mission. !reset";

        public CommandFeedback Execute(NetworkCommunicator networkPeer, string[] args)
        {
            MissionHooks.Instance.ResetMission();

            return new CommandFeedback(CommandLogType.BroadcastToAdmins,
                msg: string.Format("** Command ** {0} has reset the map.", networkPeer.GetUsername()));
        }
    }
}