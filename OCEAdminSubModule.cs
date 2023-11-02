using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using System.Threading.Tasks;
using System;
using OCEAdmin.Core;
using OCEAdmin.Core.Extensions;
using OCEAdmin.Core.Permissions;
using OCEAdmin.Patches;
using HarmonyLib;
using OCEAdmin.Plugins.Logging;
using OCEAdmin.Plugins.Groupfight;
using OCEAdmin.Plugins.Commands;
using OCEAdmin.Plugins.NameExploitFix;
using OCEAdmin.Plugins.Bans;
using OCEAdmin.Plugins.Admin;
using OCEAdmin.Plugins.Mutes;

namespace OCEAdmin
{
    public class OCEAdminSubModule : MBSubModuleBase
    {
        protected static OCEAdminSubModule Instance;
        public static OCEAdminSubModule Get()
        {
            return Instance;
        }

        protected PluginManager _plugins;
        public PluginManager GetPluginManager() => _plugins;

        private static Harmony _harmony;

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            Instance = this;

            try
            {
                Load();
            }
            catch(Exception error)
            {
                MPUtil.WriteToConsole("OCEAdmin experienced an error while trying to start. Bannerlord server start will not continue.", true);
                MPUtil.WriteToConsole($"Error: {error.Message}", true);
            }
        }

        protected async void Load()
        {
            // Loads the configuration for OCEAdmin variables.
            await Config.Load();

            // Load variables that have been predefined from the config
            // into the session. These won't be saved after change.
            await SessionManager.UpdateFromConfig();

            await _plugins.RegisterPlugin(new BansPlugin());
            await _plugins.RegisterPlugin(new MutesPlugin());
            await _plugins.RegisterPlugin(new AdminPlugin());
            await _plugins.RegisterPlugin(new CommandsPlugin());
            await _plugins.RegisterPlugin(new LoggingPlugin());
            await _plugins.RegisterPlugin(new GroupfightPlugin());
            await _plugins.RegisterPlugin(new NameExploitFixPlugin());
        }

        public static T GetPlugin<T>() where T : IPluginBase
        {
            OCEAdminSubModule instance = Get();

            if (instance != null)
            {
                return instance.GetPluginManager().GetPlugin<T>();
            }
            else
            {
                return default(T);
            }
        }

        protected override void OnSubModuleUnloaded() { }

        public override void OnBeforeMissionBehaviorInitialize(TaleWorlds.MountAndBlade.Mission mission)
        {
            base.OnBeforeMissionBehaviorInitialize(mission);
        }

        public Task OnPatch()
        {
            Harmony.DEBUG = true;
            _harmony = new Harmony("OCEAdmin.Bannerlord");

            foreach (IPluginBase plugin in _plugins.GetPlugins())
            {
                plugin.OnPatch(_harmony);
            }

            return Task.CompletedTask;
        }

        public override void OnMultiplayerGameStart(Game game, object starterObject) 
        {
            try
            {
                MPUtil.WriteToConsole("Loading game handlers...");

                game.AddGameHandler<PlayerGameHandler>();

                foreach(IPluginBase plugin in _plugins.GetPlugins())
                {
                    plugin.OnMultiplayerGameStart(game, starterObject);
                }
            }
            catch(Exception ex)
            {
                MPUtil.WriteToConsole(ex.ToString(), true);
            }
        }

        public override void OnGameEnd(Game game)
        {
            try
            {
                MPUtil.WriteToConsole("Unloading game handlers...");

                game.RemoveGameHandler<PlayerGameHandler>();

                foreach (IPluginBase plugin in _plugins.GetPlugins())
                {
                    plugin.OnGameEnd(game);
                }
            }
            catch(Exception ex) 
            {
                MPUtil.WriteToConsole(ex.ToString(), true);
            }
        }
    }
}
