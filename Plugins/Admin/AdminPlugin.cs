using OCEAdmin.Core.Permissions;
using OCEAdmin.Core.Plugin;
using OCEAdmin.Plugins.Commands;
using OCEAdmin.Plugins.Groupfight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;

namespace OCEAdmin.Plugins.Admin
{
    public class AdminPlugin : PluginBase
    { 
        public override string Name => "Admin";

        public AdminPlugin() { }

        private List<AdminPerms> _admins = new List<AdminPerms>();

        public override async Task Load()
        {
            await base.Load();

            CommandsPlugin commands = OCEAdminSubModule.GetPlugin<CommandsPlugin>();

            ICommand[] registerList = new ICommand[]
            {
                new AdminChat(),
                new Bots(),
                new Bring(),
                new ChangeMap(),
                new ChangeMapFacs(),
                new ChangeMission(),
                new EndWarmup(),
                new Factions(),
                new GetSpecs(),
                new GodMode(),
                new Goto(),
                new Heal(),
                new Help(),
                new Kick(),
                new Kill(),
                new Login(),
                new Maps(),
                new MapTime(),
                new Reset(),
                new WarmupTime()
            };

            await commands.Register(registerList);

            _admins = Config.Get().Admins;

            MPUtil.WriteToConsole("Loading admins from the config.");

            foreach (AdminPerms admin in _admins)
            {
                MPUtil.WriteToConsole($"Adding '{admin.PlayerId}' from the config.");
            }
        }

        public override void OnMultiplayerGameStart(Game game, object starterObject)
        {
            base.OnMultiplayerGameStart(game, starterObject);
            game.AddGameHandler<PlayerGameHandler>();
        }

        public override void OnGameEnd(Game game)
        {
            base.OnGameEnd(game);
            game.RemoveGameHandler<PlayerGameHandler>();
        }

        public void AddAdmin(AdminPerms admin)
        {
            _admins.Add(admin);
        }

        public List<AdminPerms> GetAdmins()
        {
            return _admins;
        }
    }
}
