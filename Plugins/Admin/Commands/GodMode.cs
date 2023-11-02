using NetworkMessages.FromServer;
using OCEAdmin.Core;
using OCEAdmin.Plugins.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace OCEAdmin.Plugins.Admin
{
    class GodMode : ICommand
    {
        public Role CanUse() => Role.Admin;

        public string Command() => "!godmode";

        public string Description() => "Makes you immune to all damage.";

        public CommandFeedback Execute(NetworkCommunicator networkPeer, string[] args)
        {
            if (networkPeer.ControlledAgent != null) {
                networkPeer.ControlledAgent.BaseHealthLimit = 2000;
                networkPeer.ControlledAgent.HealthLimit = 2000;
                networkPeer.ControlledAgent.Health = 2000;
                networkPeer.ControlledAgent.SetMinimumSpeed(10);
                networkPeer.ControlledAgent.SetMaximumSpeedLimit(10, false);           
            }

            return new CommandFeedback(CommandLogType.Player, "God mode enabled.", peer: networkPeer);
        }
    }
}
