using System;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using HarmonyLib;
using System.Reflection;
using ChatCommands.Patches;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace ChatCommands
{
    public class ChatCommandsSubModule : MBSubModuleBase
    {
        public static ChatCommandsSubModule Instance { get; private set; }

        private void setup() {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string configPath = Path.Combine(basePath, "chatCommands.json");
            if (!File.Exists(configPath))
            {
                Config config = new Config();
                config.AdminPassword = Helpers.RandomString(6);
                config.Admins = new List<string>();
                config.Admins.Add("test");
                config.Admins.Add("test2");

                ConfigManager.SetConfig(config);
                string json = JsonConvert.SerializeObject(config);
                File.WriteAllText(configPath, json);
            }
            else {
                string configString = File.ReadAllText(configPath);
                Config config = JsonConvert.DeserializeObject<Config>(configString);
                ConfigManager.SetConfig(config);
            }
        }

        private void TestClanUniformSetup()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string clanUniformsPath = Path.Combine(basePath, "clanUniforms.json");
            if (!File.Exists(clanUniformsPath))
            {
                UniformManager uniformManager = new UniformManager();
                ClanUniform clanUniform = new ClanUniform();

                clanUniform.clanTag = "ASTG";
                clanUniform.officerIDs = new List<String> { "2.0.0.76561198259745840" };
                clanUniform.uniformParts = new List<UniformPart>();
                clanUniform.uniformParts.Add(new UniformPart(EquipmentIndex.Head, 
                    new List<string> { "mp_nasal_helmet_over_cloth_headwrap" }, 
                    new List<string> { "mp_nasal_helmet_over_cloth_headwrap" }));
                clanUniform.uniformParts.Add(new UniformPart(EquipmentIndex.Body,
                    new List<string> { "mp_veteran_mercenary_armor" },
                    new List<string> { "mp_veteran_mercenary_armor" }));
                clanUniform.uniformParts.Add(new UniformPart(EquipmentIndex.Cape,
                    new List<string> { "mp_a_battania_cloak_a" },
                    new List<string> { "mp_battanian_leather_shoulder_a" }));
                clanUniform.uniformParts.Add(new UniformPart(EquipmentIndex.Gloves,
                    new List<string> { "mp_leather_gloves" },
                    new List<string> { "mp_leather_gloves" }));
                clanUniform.uniformParts.Add(new UniformPart(EquipmentIndex.Leg,
                    new List<string> { "mp_fine_town_boots" },
                    new List<string> { "mp_fine_town_boots" }));

                uniformManager.Add(clanUniform);

                AdminPanel.Instance.uniformManager = uniformManager;

                string json = JsonConvert.SerializeObject(uniformManager);
                File.WriteAllText(clanUniformsPath, json);
            }
            else
            {
                string clanUniformListString = File.ReadAllText(clanUniformsPath);
                UniformManager uniformManager = JsonConvert.DeserializeObject<UniformManager>(clanUniformListString);
                AdminPanel.Instance.uniformManager = uniformManager;
            }
        }

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            this.setup();
            this.TestClanUniformSetup();

            string basePath = AppDomain.CurrentDomain.BaseDirectory;

            var watcher = new FileSystemWatcher(@basePath);
            watcher.NotifyFilter = NotifyFilters.Attributes
                     | NotifyFilters.CreationTime
                     | NotifyFilters.DirectoryName
                     | NotifyFilters.FileName
                     | NotifyFilters.LastAccess
                     | NotifyFilters.LastWrite
                     | NotifyFilters.Security
                     | NotifyFilters.Size;
            watcher.Changed += OnChanged;
            watcher.Filter = "*.json";
            watcher.EnableRaisingEvents = true;

            Debug.Print("** CHAT COMMANDS BY MENTALROB LOADED **", 0, Debug.DebugColor.Green);
           
            CommandManager cm = new CommandManager();
            Harmony.DEBUG = true;

            var harmony = new Harmony("mentalrob.chatcommands.bannerlord");
            // harmony.PatchAll(assembly);
            var original = typeof(ChatBox).GetMethod("ServerPrepareAndSendMessage", BindingFlags.NonPublic | BindingFlags.Instance);
            var prefix = typeof(PatchChatBox).GetMethod("Prefix");
            harmony.Patch(original, prefix: new HarmonyMethod(prefix));
        }

        private static void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                return;
            }

            Debug.Print("Attempting live update on " + e.FullPath, 0, Debug.DebugColor.Green);

            if(e.FullPath.Contains("clanUniforms"))
            {
                string basePath = AppDomain.CurrentDomain.BaseDirectory;
                string clanUniformsPath = Path.Combine(basePath, "clanUniforms.json");
                string clanUniformListString = File.ReadAllText(clanUniformsPath);
                UniformManager uniformManager = JsonConvert.DeserializeObject<UniformManager>(clanUniformListString);
                AdminPanel.Instance.uniformManager = uniformManager;
            }

            Debug.Print("Update complete on " + e.FullPath, 0, Debug.DebugColor.Green);
        }

        protected override void OnSubModuleUnloaded() {
            Debug.Print("** CHAT COMMANDS BY MENTALROB UNLOADED **", 0, Debug.DebugColor.Green);
            // Game.OnGameCreated -= OnGameCreated;
        }

        public override void OnMultiplayerGameStart(Game game, object starterObject) {

            Debug.Print("** CHAT HANDLER ADDED **", 0, Debug.DebugColor.Green);
            game.AddGameHandler<ChatHandler>();
            // game.AddGameHandler<ManipulatedChatBox>();
            
        }
        public override void OnGameEnd(Game game) {
            game.RemoveGameHandler<ChatHandler>();
        }

    }
}
