using NetworkMessages.FromServer;
using OCEAdmin.Core;
using OCEAdmin.Plugins.Commands;
using OCEAdmin.Plugins.Mutes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade;

namespace OCEAdmin.Plugins.Bans
{
    class Unban : ICommand
    {
        public Role CanUse() => Role.Admin;

        public string Command() => "!unban";

        public string Description() => "Unbans a player. Usage !unban <Player Name>";

        public CommandFeedback Execute(NetworkCommunicator networkPeer, string[] args)
        {
            if (args.Length == 0)
            {
                return new CommandFeedback(CommandLogType.Player, msg: "Please provide a username.",
                    peer: networkPeer);
            }

            string username = string.Join(" ", args);
            BansPlugin bans = OCEAdminSubModule.GetPlugin<BansPlugin>();

            foreach (Ban ban in bans.GetBans())
            {
                if(ban.nickname == username)
                {
                    bans.Handler.OnRemoveBan(ban.steamid);

                    return new CommandFeedback(CommandLogType.BroadcastToAdmins,
                        msg: string.Format("** Command ** {0} has unbanned {1}.", networkPeer.GetUsername(), ban.nickname));
                }
            }

            return new CommandFeedback(CommandLogType.Player, msg: "Username was not found on banlist!",
                 peer: networkPeer);
        }
    }
}
