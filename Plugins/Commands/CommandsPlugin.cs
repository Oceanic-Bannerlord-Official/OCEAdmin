using OCEAdmin.Core.Permissions;
using OCEAdmin.Core.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace OCEAdmin.Plugins.Commands
{
    public class CommandsPlugin : PluginBase
    {
        public override string Name => "Commands";

        public Dictionary<string, ICommand> commands = new Dictionary<string, ICommand>();

        public CommandsPlugin() { }

        public override async Task Load()
        {
            await base.Load();         
        }

        public CommandFeedback Execute(NetworkCommunicator networkPeer, string command, string[] args)
        {
            ICommand executableCommand;
            bool exists = commands.TryGetValue(command, out executableCommand);

            Player player = networkPeer.GetPlayer();

            if (!exists)
            {
                return new CommandFeedback(CommandLogType.Player, "This command does not exist.", peer: networkPeer);
            }
            if (!player.HasPermission(executableCommand.CanUse()))
            {
                return new CommandFeedback(CommandLogType.Player, "You are not authorised to run this command.", peer: networkPeer);
            }

            MPUtil.WriteToConsole(networkPeer.GetUsername() + " attempted to run the " + command + " command.", true);

            return executableCommand.Execute(networkPeer, args);
        }

        public Task Register(ICommand[] commands)
        {
            foreach(ICommand command in commands)
            {
                Register(command);
            }

            return Task.CompletedTask;
        }

        public Task Register(ICommand command)
        {
            MPUtil.WriteToConsole("** Chat Command " + command.Command() + " has been loaded!");
            commands.Add(command.Command(), command);

            return Task.CompletedTask;
        }

        public override void OnMultiplayerGameStart(Game game, object starterObject)
        {
            base.OnMultiplayerGameStart(game, starterObject);
            game.AddGameHandler<CommandsGameHandler>();
        }

        public override void OnGameEnd(Game game)
        {
            base.OnGameEnd(game);
            game.RemoveGameHandler<CommandsGameHandler>();
        }
    }
}
