using System;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using HarmonyLib;
using System.Reflection;
using OCEAdmin.Patches;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace OCEAdmin
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
                config.AdminPassword = MPUtil.RandomString(6);
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

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            this.setup();
            UniformManager.Instance.Populate();

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
            watcher.Filter = "*.xml";
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
