using NetworkMessages.FromServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade;
using OCEAdmin.Core;

namespace OCEAdmin.Commands
{
    class Help : ICommand
    {
        public Role CanUse() => Role.Player;
        public string Command() => "!help";

        public string Description() => "Help message. Returns all the avaliable commands.";

        public CommandFeedback Execute(NetworkCommunicator networkPeer, string[] args)
        {
            string[] commands = CommandManager.commands.Keys.ToArray();

            networkPeer.SendChatMessage("<< Command List >>");

            Player player = networkPeer.GetPlayer();

            foreach (string command in commands) {
                ICommand commandExecutable = CommandManager.commands[command];

                if(player.HasPermission(commandExecutable.CanUse()))
                {
                    MPUtil.SendChatMessage(networkPeer, string.Format("{0}: {1}", command, commandExecutable.Description()));
                }
            }
            return new CommandFeedback(CommandLogType.None);
        }
    }
}
