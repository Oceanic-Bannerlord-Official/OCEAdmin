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
    class AdminChat : ICommand
    {
        public Role CanUse() => Role.Admin;

        public string Command() => "!a";

        public string Description() => "Type in admin chat. !a <string text>";

        public CommandFeedback Execute(NetworkCommunicator networkPeer, string[] args)
        {
            String text = string.Join(" ", args);

            return new CommandFeedback(CommandLogType.BroadcastToAdmins,
                msg: string.Format("(Admin) {0}: {1}", networkPeer.VirtualPlayer.UserName.ToString(), text));
        }
    }
}
