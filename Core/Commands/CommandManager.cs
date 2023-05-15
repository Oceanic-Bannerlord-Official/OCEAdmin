using OCEAdmin.Commands;
using NetworkMessages.FromServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using OCEAdmin.Core;
using System.Threading.Tasks;

namespace OCEAdmin.Commands
{
    public static class CommandManager
    {
        public static Dictionary<string, ICommand> commands;

        public static CommandFeedback Execute(NetworkCommunicator networkPeer, string command, string[] args) {
            ICommand executableCommand; 
            bool exists = commands.TryGetValue(command, out executableCommand);

            Player player = networkPeer.GetPlayer();

            if (!exists) {
                return new CommandFeedback(CommandLogType.Player, "This command does not exist.", peer: networkPeer);
            }
            if (!player.HasPermission(executableCommand.CanUse())) {
                return new CommandFeedback(CommandLogType.Player, "You are not authorised to run this command.", peer: networkPeer);
            }

            return executableCommand.Execute(networkPeer, args);
        }

        public static Task Initialize() {
            MPUtil.WriteToConsole("Loading all chat commands...");

            commands = new Dictionary<string, ICommand>();

            foreach (Type mytype in System.Reflection.Assembly.GetExecutingAssembly().GetTypes()
                 .Where(mytype => mytype.GetInterfaces().Contains(typeof(ICommand))))
            {
                ICommand command = (ICommand) Activator.CreateInstance(mytype);

                if(command.Command() != null)
                {
                    if (!commands.ContainsKey(command.Command()))
                    {
                        MPUtil.WriteToConsole("** Chat Command " + command.Command() + " has been loaded!");
                        commands.Add(command.Command(), command);
                    }
                }
            }

            return Task.CompletedTask;
        }
    }
}
