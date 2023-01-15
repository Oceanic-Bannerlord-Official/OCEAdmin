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

        public Dictionary<string, ICommand> commands;

        public List<CommandSession> commandSessions = new List<CommandSession>();

        public const float commandSessionTimeOut = 15f;

        public CommandSession GetCommandSession(NetworkCommunicator peer, ICommand command)
        {
            foreach(CommandSession session in commandSessions)
            {
                if(session.executor == peer && session.command == command)
                {
                    TimeSpan diff = DateTime.Now - session.timeExecuted;

                    if(diff.TotalSeconds >= commandSessionTimeOut)
                    {
                        commandSessions.Remove(session);

                        return null;
                    }
                    else
                    {
                        return session;
                    }
                }
            }

            return null;
        }

        public CommandSession CreateCommandSession(ICommand command, NetworkCommunicator executor, List<NetworkCommunicator> peersResult)
        {
            CommandSession session = new CommandSession()
            {
                command = command,
                executor = executor,
                peers = peersResult
            };

            commandSessions.Add(session);

            return session;
        }

        public CommandFeedback Execute(NetworkCommunicator networkPeer, string command, string[] args) {
            ICommand executableCommand; 
            bool exists = commands.TryGetValue(command, out executableCommand);

            if (!exists) {
                return new CommandFeedback(CommandLogType.Player, "This command does not exist.", peer: networkPeer);
            }
            if (!HasPermission(networkPeer, executableCommand.CanUse())) {
                return new CommandFeedback(CommandLogType.Player, "You are not authorised to run this command.", peer: networkPeer);
            }

            return executableCommand.Execute(networkPeer, args);
        }

        public bool HasPermission(NetworkCommunicator networkPeer, Permissions perms)
        {
            switch(perms)
            {
                case Permissions.Player:
                    return true;
                default:
                    bool isAdmin = false;
                    bool isExists = AdminManager.Admins.TryGetValue(networkPeer.VirtualPlayer.Id.ToString(), out isAdmin);

                    return isExists && isAdmin;
            }
        }

        public void Initialize() {
            this.commands = new Dictionary<string, ICommand>();
            foreach (Type mytype in System.Reflection.Assembly.GetExecutingAssembly().GetTypes()
                 .Where(mytype => mytype.GetInterfaces().Contains(typeof(ICommand))))
            {
                ICommand command = (ICommand) Activator.CreateInstance(mytype);
                if (!commands.ContainsKey(command.Command())) {
                    MPUtil.WriteToConsole("** Chat Command " + command.Command() + " has been loaded!");
                    commands.Add(command.Command(), command);
                }
            }

        }
    }
}
