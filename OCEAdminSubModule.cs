using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using HarmonyLib;
using System.Reflection;
using System.IO;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml;
using System.Text;
using System.Threading;
using OCEAdmin.Patches;
using OCEAdmin.Updating;
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
            ConfigManager.LoadConfig();

            // Creates a new instance of the in-game command manager.
            CommandManager.Instance.Initialize();

            // Commences the uniform update service.
            UpdateManager.Instance.Initialise();
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
