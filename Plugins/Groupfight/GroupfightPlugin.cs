using OCEAdmin.Core.Plugin;
using OCEAdmin.Plugins.Commands;
using System.Threading.Tasks;
using TaleWorlds.Core;

namespace OCEAdmin.Plugins.Groupfight
{
    public class GroupfightPlugin : PluginBase
    {
        public override string Name => "Groupfighting";

        public override string Description => "Allows for groupfight mode to disable ranged and cavalry.";

        public override bool IsCore => false; 

        public GroupfightPlugin() { }

        public override async Task Load()
        {
            await base.Load();

            CommandsPlugin commands = OCEAdminSubModule.GetPlugin<CommandsPlugin>();

            if(commands != null)
            {
                await commands.Register(new Groupfight());
            }
        }

        public override void OnMultiplayerGameStart(Game game, object starterObject)
        {
            base.OnMultiplayerGameStart(game, starterObject);
            game.AddGameHandler<GroupfightGameHandler>();
        }

        public override void OnGameEnd(Game game)
        {
            base.OnGameEnd(game);
            game.RemoveGameHandler<GroupfightGameHandler>();
        }
    }
}
