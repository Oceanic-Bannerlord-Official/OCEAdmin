using NetworkMessages.FromServer;
using OCEAdmin.Core;
using OCEAdmin.Plugins.Commands;
using OCEAdmin.Plugins.Mutes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.DedicatedCustomServer;

namespace OCEAdmin.Plugins.Bans
{
    class BanCommand : PeerSearchCommand
    {
        public override Role CanUse() => Role.Admin;

        public override string Command() => "!ban";

        public override string Description() =>  "Bans a player. First user that contains the provided input will be banned. Usage !ban <player name>";

        public override CommandFeedback OnRunAction(NetworkCommunicator networkPeer, NetworkCommunicator targetPeer)
        {
            BansPlugin bans = OCEAdminSubModule.GetPlugin<BansPlugin>();

            bans.Handler.OnAddBan(new Ban(steamid: targetPeer.VirtualPlayer.Id.ToString(),
                bannerID: networkPeer.VirtualPlayer.Id.ToString(),
                nickname: targetPeer.GetUsername()));

            DedicatedCustomServerSubModule.Instance.DedicatedCustomGameServer.KickPlayer(targetPeer.VirtualPlayer.Id, false);

            return new CommandFeedback(CommandLogType.BroadcastToAdmins,
                msg: string.Format("** Command ** {0} has banned {1} from the server.", networkPeer.GetUsername(), targetPeer.GetUsername()));
        }
    }
}
