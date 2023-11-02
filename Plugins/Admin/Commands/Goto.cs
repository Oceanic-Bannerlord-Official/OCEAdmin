using NetworkMessages.FromServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using OCEAdmin.Core;
using OCEAdmin.Plugins.Commands;
using OCEAdmin.Core.Permissions;

namespace OCEAdmin.Plugins.Admin
{
    class Goto : PeerSearchCommand
    {
        public override Role CanUse() => Role.Admin;
        public override string Command() => "!goto";
        
        public override string Description() => "Teleport yourself to another. Usage !tp <Target User>";

        public override CommandFeedback OnRunAction(NetworkCommunicator networkPeer, NetworkCommunicator targetPeer)
        {
            if (networkPeer.ControlledAgent != null && targetPeer.ControlledAgent != null) {
                Vec3 targetPos = targetPeer.ControlledAgent.Position;
                targetPos.x = targetPos.x + 1;
                networkPeer.ControlledAgent.TeleportToPosition( targetPos );
            }

            return new CommandFeedback(CommandLogType.None);
        }
    }
}
