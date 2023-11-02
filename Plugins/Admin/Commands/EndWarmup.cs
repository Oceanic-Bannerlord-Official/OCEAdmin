using NetworkMessages.FromServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.DedicatedCustomServer;
using OCEAdmin.Core;
using OCEAdmin.Core.Missions;
using OCEAdmin.Plugins.Commands;
using OCEAdmin.Core.Permissions;

namespace OCEAdmin.Plugins.Admin
{

    class EndWarmup : ICommand
    {
        public Role CanUse() => Role.Admin;

        public string Command() => "!endwarmup";
        public string Description() => "Ends warmup mode <optional int time>";

        public CommandFeedback Execute(NetworkCommunicator networkPeer, string[] args)
        {
            MissionHooks.Instance.EndWarmup();

            return new CommandFeedback(CommandLogType.BroadcastToAdmins, msg: string.Format("** Command ** {0} has ended warmup.", networkPeer.GetUsername()),
                peer: networkPeer);
        }
    }
}
