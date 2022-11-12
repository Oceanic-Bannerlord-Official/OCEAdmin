using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using HarmonyLib;
using System.Reflection;
using OCEAdmin.Patches;
using System.IO;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml;
using System.Text;
using System;
using System.Threading;

namespace OCEAdmin
{
    public class OCEAdminSubModule : MBSubModuleBase
    {
        public static OCEAdminSubModule Instance { get; private set; }

        public const string baseDir = "../../OCEAdmin";
        public string configFile = "config.xml";

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            MPUtil.WriteToConsole("Loading chat commands...");
           
            CommandManager cm = new CommandManager();
            Harmony.DEBUG = true;

            var harmony = new Harmony("OCEAdmin.Bannerlord");
            var original = typeof(ChatBox).GetMethod("ServerPrepareAndSendMessage", BindingFlags.NonPublic | BindingFlags.Instance);
            var prefix = typeof(PatchChatBox).GetMethod("Prefix");
            harmony.Patch(original, prefix: new HarmonyMethod(prefix));
            MPUtil.WriteToConsole("Patched ChatBox::ServerPrepareAndSendMessage");
        }
        protected void Populate()
        {
            if (!Directory.Exists(baseDir))
            {
                Directory.CreateDirectory(baseDir);
            }

            string configPath = Path.Combine(baseDir, configFile);

            if (!File.Exists(configPath))
            {
                Config config = new Config();
                config.AdminPassword = MPUtil.RandomString(6);
                config.Admins = new List<string>();
                config.Admins.Add("2.0.0.AdminIDHere");
                config.Admins.Add("2.0.0.AdminIDHere");

                ConfigManager.SetConfig(config);
                XmlSerializer serializer = new XmlSerializer(typeof(Config));
                Stream fs = new FileStream(configPath, FileMode.Create);

                XmlTextWriter writer = new XmlTextWriter(fs, Encoding.Unicode);
                writer.Formatting = Formatting.Indented;
                writer.Indentation = 4;

                serializer.Serialize(writer, config);
                writer.Close();
            }
            else
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Config));
                Config config = new Config();

                using (Stream reader = new FileStream(configPath, FileMode.Open))
                {
                    config = (Config)serializer.Deserialize(reader);
                }

                ConfigManager.SetConfig(config);
            }
        }

        protected override void OnSubModuleUnloaded() {
            MPUtil.WriteToConsole("Unloading...");
        }

        public override void OnMultiplayerGameStart(Game game, object starterObject) {

            MPUtil.WriteToConsole("Chat Handler is being loaded...");
            game.AddGameHandler<ChatHandler>();
            this.Populate();

            Thread updateThread = new Thread(() => StartUpdateManager());
            updateThread.Start();
        }

        public static void StartUpdateManager()
        {
            // Begin the update process for the uniforms.
            UpdateManager.Instance.Initialise();
        }

        public override void OnGameEnd(Game game) {
            game.RemoveGameHandler<ChatHandler>();
        }

    }
}
