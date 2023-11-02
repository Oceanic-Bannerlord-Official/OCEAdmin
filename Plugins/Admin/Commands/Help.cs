using NetworkMessages.FromServer;
using OCEAdmin.Core.Permissions;
using OCEAdmin.Plugins.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade;

namespace OCEAdmin.Plugins.Admin
{
    class Help : ICommand
    {
        public Role CanUse() => Role.Player;
        public string Command() => "!help";

        public string Description() => "Help message. Returns all the avaliable commands.";

        public CommandFeedback Execute(NetworkCommunicator networkPeer, string[] args)
        {
            CommandsPlugin plugin = OCEAdminSubModule.GetPlugin<CommandsPlugin>();
            string[] commands = plugin.commands.Keys.ToArray();

            networkPeer.SendChatMessage("<< Command List >>");

            Player player = networkPeer.GetPlayer();

            foreach (string command in commands) {
                ICommand commandExecutable = plugin.commands[command];

                if(player.HasPermission(commandExecutable.CanUse()))
                {
                    MPUtil.SendChatMessage(networkPeer, string.Format("{0}: {1}", command, commandExecutable.Description()));
                }
            }
            return new CommandFeedback(CommandLogType.None);
        }
    }
}
