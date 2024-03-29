﻿using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using OCEAdmin.Patches;
using OCEAdmin.Commands;
using OCEAdmin.Features;
using static OCEAdmin.API.EndPoint;
using OCEAdmin.Core;
using System.Threading.Tasks;
using OCEAdmin.Core.Permissions;
using System.Collections.Generic;
using System;
using OCEAdmin.Core.Logging;
using PersistentEmpires;
using OCEAdmin.Core.Extension;

namespace OCEAdmin
{
    public class OCEAdminSubModule : MBSubModuleBase
    {
        private bool _loaded;
        public static OCEAdminSubModule Instance { get; private set; }

        private List<IPlugin> Plugins = new List<IPlugin>();

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            Instance = this;

            try
            {
                LoadDependencies();
            }
            catch(Exception error)
            {
                MPUtil.WriteToConsole("OCEAdmin experienced an error while trying to start. Bannerlord server start will not continue.", true);
                MPUtil.WriteToConsole($"Error: {error.Message}", true);
            }
        }

        public bool IsLoaded() => _loaded;

        protected async void LoadDependencies()
        {
            // Start logging all commands ingame and from the console.
            await LogManager.Start();

            // Loads the configuration for OCEAdmin variables.
            await Config.Load();

            // Load variables that have been predefined from the config
            // into the session. These won't be saved after change.
            await SessionManager.UpdateFromConfig();

            await AdminManager.LoadAdmins();
            await BanManager.LoadBans();
            await MuteManager.LoadMutes();

            // Initialize all the commands for the in-game command manager.
            await CommandManager.Initialize();

            // This handles all the hotfixes or game code edits
            await PatchManager.LoadPatches();
        }

        protected override void OnSubModuleUnloaded() { }

        public override void OnBeforeMissionBehaviorInitialize(Mission mission)
        {
            base.OnBeforeMissionBehaviorInitialize(mission);

            Mission.Current.AddMissionBehavior(new SpecialistLimitMissionBehavior());
        }

        public override void OnMultiplayerGameStart(Game game, object starterObject) 
        {
            try
            {
                MPUtil.WriteToConsole("Loading game handlers...");

                game.AddGameHandler<CommandsGameHandler>();
                game.AddGameHandler<BansGameHandler>();
                game.AddGameHandler<PlayerGameHandler>();
                game.AddGameHandler<GroupfightGameHandler>();
                game.AddGameHandler<SpecialistLimitGameHandler>();
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

                game.RemoveGameHandler<CommandsGameHandler>();
                game.RemoveGameHandler<BansGameHandler>();
                game.RemoveGameHandler<PlayerGameHandler>();
                game.RemoveGameHandler<GroupfightGameHandler>();
                game.RemoveGameHandler<SpecialistLimitGameHandler>();
            }
            catch(Exception ex) 
            {
                MPUtil.WriteToConsole(ex.ToString(), true);
            }
        }
    }
}
