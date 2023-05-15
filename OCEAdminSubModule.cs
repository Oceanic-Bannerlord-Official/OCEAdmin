using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using OCEAdmin.Patches;
using OCEAdmin.Commands;
using OCEAdmin.Features;
using SocketIOClient;
using System.Text.Json;
using static OCEAdmin.API.EndPoint;
using OCEAdmin.Core;

namespace OCEAdmin
{
    public class OCEAdminSubModule : MBSubModuleBase
    {
        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            // Loads the configuration for OCEAdmin variables.
            Config config = ConfigManager.Instance.LoadConfig();

            // Replace the current admins loaded from the xml with
            // the ones from the OCEAdmin API.
            if (config.UseWebAdmin)
            {
                ConfigManager.LoadAdminsFromAPI();
            }

            // Deciding whether to load from the API or local
            // storage.
            if (config.UseWebBans)
            {
                BanManager.Handler = new WebBanTransport();
            }
            else
            {
                BanManager.Handler = new LocalBanTransport();
            }

            // Persistance for punitary actions.
            BanManager.Handler.LoadList();
            MuteManager.LoadMutes();

            // Creates a new instance of the in-game command manager.
            CommandManager.Instance.Initialize();

            // This handles all the hotfixes or game code edits
            PatchManager.LoadPatches();
        }

        protected override void OnSubModuleUnloaded() { }

        public override void OnMultiplayerGameStart(Game game, object starterObject) 
        {
            game.AddGameHandler<OCEAdminHandler>();
            game.AddGameHandler<SpecialistLimitGameHandler>();
        }

        public override void OnBeforeMissionBehaviorInitialize(Mission mission)
        {
            base.OnBeforeMissionBehaviorInitialize(mission);

            if (ConfigManager.Instance.GetConfig().SpecialistSettings.Enabled)
            {
                Mission.Current.AddMissionBehavior(new SpecialistLimitMissionBehavior());
            }
        }

        public override void OnGameEnd(Game game)
        {
            game.RemoveGameHandler<OCEAdminHandler>();
            game.RemoveGameHandler<SpecialistLimitGameHandler>();
        }

    }
}
