using NetworkMessages.FromServer;
using OCEAdmin.Core;
using OCEAdmin.Core.Permissions;
using OCEAdmin.Plugins.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.DedicatedCustomServer;

namespace OCEAdmin.Plugins.Admin
{
    class Bring : PeerSearchCommand
    {
        public override Role CanUse() => Role.Admin;

        public override string Command() => "!bring";

        public override string Description() => "Brings another player to you. Usage !bring <Target User>";

        public override CommandFeedback OnRunAction(NetworkCommunicator networkPeer, NetworkCommunicator targetPeer)
        {
            if (networkPeer.ControlledAgent != null && targetPeer.ControlledAgent != null)
            {
                Vec3 targetPos = networkPeer.ControlledAgent.Position;
                targetPos.x = targetPos.x + 1;
                targetPeer.ControlledAgent.TeleportToPosition(targetPos);

                return new CommandFeedback(CommandLogType.Both,
                    msg: string.Format("** Command ** You have brought {0} to you.", targetPeer.GetUsername()), peer: networkPeer,
                    targetMsg: string.Format("** Command ** {0} has brought you to them.", networkPeer.GetUsername()), targetPeer: targetPeer);
            }

            return new CommandFeedback(CommandLogType.Player, msg: "Player is not alive. Can't bring.",
                    peer: networkPeer);
        }
    }
}
