using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using OCEAdmin.Patches;
using OCEAdmin.Commands;
using OCEAdmin.Core;
using System.Threading;
using OCEAdmin.Features;

namespace OCEAdmin
{
    public class OCEAdminSubModule : MBSubModuleBase
    {
        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            // Loads the configuration for OCEAdmin variables.
            ConfigManager.Instance.LoadConfig();

            // Creates a new instance of the in-game command manager.
            CommandManager.Instance.Initialize();

            // This handles all the hotfixes or game code edits
            PatchManager.LoadPatches();

            // Commences the uniform service and it's update service.
            if (ConfigManager.Instance.GetConfig().UniformSettings.Enabled)
            {
                UniformManager.Instance.Initialise();
            }
        }

        protected override void OnSubModuleUnloaded() { }

        public override void OnMultiplayerGameStart(Game game, object starterObject) 
        {
            game.AddGameHandler<OCEAdminHandler>();

            if (ConfigManager.Instance.GetConfig().SpecialistSettings.Enabled)
            {
                game.AddGameHandler<SpecialistLimitGameHandler>();
            }
        }

        public override void OnBeforeMissionBehaviorInitialize(Mission mission)
        {
            base.OnBeforeMissionBehaviorInitialize(mission);

            if (ConfigManager.Instance.GetConfig().SpecialistSettings.Enabled)
            {
                Mission.Current.AddMissionBehavior(new SpecialistLimitMissionBehavior());
            }

            if (ConfigManager.Instance.GetConfig().AutoAdminSettings.DismountSystemEnabled)
            {
                Mission.Current.AddMissionBehavior(new CavalryDismountMissionBehavior());
            }
        }

        public override void OnGameEnd(Game game)
        {
            game.RemoveGameHandler<OCEAdminHandler>();

            if (ConfigManager.Instance.GetConfig().SpecialistSettings.Enabled)
            {
                game.RemoveGameHandler<SpecialistLimitGameHandler>();
            }
        }

    }
}
