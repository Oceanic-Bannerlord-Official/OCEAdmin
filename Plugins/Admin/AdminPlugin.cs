using OCEAdmin.Core.Extensions;
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

            if (commands != null)
            {
                await commands.Register(new AdminChat());
                await commands.Register(new Bots());
                await commands.Register(new Bring());
                await commands.Register(new ChangeMap());
                await commands.Register(new ChangeMapFacs());
                await commands.Register(new ChangeMission());
                await commands.Register(new EndWarmup());
                await commands.Register(new Factions());
                await commands.Register(new GetSpecs());
                await commands.Register(new GodMode());
                await commands.Register(new Goto());
                await commands.Register(new Heal());
                await commands.Register(new Help());
                await commands.Register(new Kick());
                await commands.Register(new Kill());
                await commands.Register(new Login());
                await commands.Register(new Maps());
                await commands.Register(new MapTime());
                await commands.Register(new Reset());
                await commands.Register(new WarmupTime());
            }

            _admins = Config.Get().Admins;

            MPUtil.WriteToConsole("Loading admins from the config.");

            foreach (AdminPerms admin in _admins)
            {
                MPUtil.WriteToConsole($"Adding '{admin.PlayerId}' from the config.");
            }
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
