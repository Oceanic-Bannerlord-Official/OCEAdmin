using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using OCEAdmin.Patches;
using OCEAdmin.Commands;
using OCEAdmin.Features;
using SocketIOClient;
using System.Text.Json;
using static OCEAdmin.API.EndPoint;
using OCEAdmin.Core;
using System.Threading.Tasks;
using OCEAdmin.Core.Permissions;
using System.Collections.Generic;
using System;

namespace OCEAdmin
{
    public class OCEAdminSubModule : MBSubModuleBase
    {
        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            try
            {
                LoadDependencies();
            }
            catch(Exception error)
            {
                MPUtil.WriteToConsole("OCEAdmin experienced an error while trying to start. Bannerlord server start will not continue.");
                MPUtil.WriteToConsole($"Error: {error.Message}");
            }
        }

        protected async void LoadDependencies()
        {
            // Loads the configuration for OCEAdmin variables.
            await Config.Load();
            await AdminManager.LoadAdmins();
            await BanManager.LoadBans();
            await MuteManager.LoadMutes();

            // Initialize all the commands for the in-game command manager.
            await CommandManager.Initialize();

            // This handles all the hotfixes or game code edits
            await PatchManager.LoadPatches();
        }

        protected override void OnSubModuleUnloaded() { }

        public override void OnMultiplayerGameStart(Game game, object starterObject) 
        {
            game.AddGameHandler<CommandsGameHandler>();
            game.AddGameHandler<BansGameHandler>();
            game.AddGameHandler<PlayerGameHandler>();
        }

        public override void OnGameEnd(Game game)
        {
            game.RemoveGameHandler<CommandsGameHandler>();
            game.RemoveGameHandler<BansGameHandler>();
            game.RemoveGameHandler<PlayerGameHandler>();
        }
    }
}
