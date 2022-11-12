using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using OCEAdmin.Patches;
using OCEAdmin.Commands;
using OCEAdmin.Core;

namespace OCEAdmin
{
    public class OCEAdminSubModule : MBSubModuleBase
    {
        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            // This handles all the hotfixes or game code edits
            PatchManager.LoadPatches();

            // Loads the configuration for OCEAdmin variables.
            ConfigManager.Instance.LoadConfig();

            // Creates a new instance of the in-game command manager.
            CommandManager.Instance.Initialize();

            // Commences the uniform service and it's update service.
            if (ConfigManager.Instance.GetConfig().UniformSettings.Enabled)
            {
                UniformManager.Instance.Initialise();
            }
        }

        protected override void OnSubModuleUnloaded() 
        {
            MPUtil.WriteToConsole("Unloading...");
        }

        public override void OnMultiplayerGameStart(Game game, object starterObject) 
        {
            game.AddGameHandler<OCEAdminHandler>();
        }

        public override void OnGameEnd(Game game) {
            game.RemoveGameHandler<OCEAdminHandler>();
        }

    }
}
