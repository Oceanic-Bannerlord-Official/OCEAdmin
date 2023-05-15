using NetworkMessages.FromServer;
using OCEAdmin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade;

namespace OCEAdmin.Commands
{
    class Unmute : ICommand
    {
        public Role CanUse() => Role.Admin;

        public string Command() => "!unmute";

        public string Description() => "Unmutes a player. Usage !unmute <Player Name>";

        public CommandFeedback Execute(NetworkCommunicator networkPeer, string[] args)
        {
            if (args.Length == 0)
            {
                return new CommandFeedback(CommandLogType.Player, msg: "Please provide a username.",
                    peer: networkPeer);
            }

            string username = string.Join(" ", args);

            foreach (OCEAdmin.Mute mute in MuteManager.GetMutes())
            {
                if (mute.nickname == username)
                {
                    MuteManager.RemoveMute(mute.playerID);

                    return new CommandFeedback(CommandLogType.BroadcastToAdmins,
                        msg: string.Format("** Command ** {0} has unmuted {1}.", networkPeer.UserName, mute.nickname));
                }
            }

            return new CommandFeedback(CommandLogType.Player, msg: "That player is not muted!",
                 peer: networkPeer);
        }
    }
}
