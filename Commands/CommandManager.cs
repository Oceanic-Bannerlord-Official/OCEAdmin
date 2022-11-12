using OCEAdmin.Commands;
using NetworkMessages.FromServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using OCEAdmin.Core;

namespace OCEAdmin.Commands
{
    class CommandManager
    {
        private static CommandManager instance;
        public static CommandManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CommandManager();
                }
                return instance;
            }
        }

        public Dictionary<string, Command> commands;

        public bool Execute(NetworkCommunicator networkPeer, string command, string[] args) {
            Command executableCommand; 
            bool exists = commands.TryGetValue(command, out executableCommand);
            if (!exists) {
                // networkPeer.
                GameNetwork.BeginModuleEventAsServer(networkPeer);
                GameNetwork.WriteMessage(new ServerMessage("This command does not exist.", false));
                GameNetwork.EndModuleEventAsServer();
                return false;
            }
            if (!executableCommand.CanUse(networkPeer)) {
                GameNetwork.BeginModuleEventAsServer(networkPeer);
                GameNetwork.WriteMessage(new ServerMessage("You are not authorised to run this command.", false));
                GameNetwork.EndModuleEventAsServer();
                return false;
            }
            return executableCommand.Execute(networkPeer, args);
        }

        public void Initialize() {
            this.commands = new Dictionary<string, Command>();
            foreach (Type mytype in System.Reflection.Assembly.GetExecutingAssembly().GetTypes()
                 .Where(mytype => mytype.GetInterfaces().Contains(typeof(Command))))
            {
                Command command = (Command) Activator.CreateInstance(mytype);
                if (!commands.ContainsKey(command.Command())) {
                    MPUtil.WriteToConsole("** Chat Command " + command.Command() + " has been loaded!");
                    commands.Add(command.Command(), command);
                }
            }

        }
    }
}
