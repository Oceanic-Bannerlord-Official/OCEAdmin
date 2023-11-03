using OCEAdmin.Core.Plugin;
using System;
using System.Collections.Generic;
using System.Timers;
using System.Threading.Tasks;
using HarmonyLib;
using System.Reflection;
using TaleWorlds.MountAndBlade;

namespace OCEAdmin.Plugins.Logging
{
    public class LoggingPlugin : PluginBase
    {
        public override string Name => "Logging";
        public override string Description => "Allows for logging of player messages.";
        public override bool IsCore => true;

        public Queue<string> Queue;

        private Timer saveTimer;

        private ILogger logger;

        public LoggingPlugin() { }

        public override async Task Load()
        {
            await base.Load();
            await Start();
        }

        public override void OnPatch(Harmony harmony)
        {
            base.OnPatch(harmony);

            var original = typeof(GameNetwork).GetMethod("AddNewPlayerOnServer", BindingFlags.Public | BindingFlags.Static);
            var prefix = typeof(LogPatchAddNewPlayerOnServer).GetMethod("Prefix");
            harmony.Patch(original, prefix: new HarmonyMethod(prefix));
        }

        public Task Add(string msg)
        {
            Queue.Enqueue(msg);

            return Task.CompletedTask;
        }

        public Task Start()
        {
            Queue = new Queue<string>();
            logger = new LocalLogger(this);

            saveTimer = new Timer(TimeSpan.FromSeconds(30).TotalMilliseconds);
            saveTimer.Elapsed += logger.Save;
            saveTimer.Start();

            return Task.CompletedTask;
        }
    }
}
