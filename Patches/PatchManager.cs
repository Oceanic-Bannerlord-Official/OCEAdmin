using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade;

namespace OCEAdmin.Patches
{
    public static class PatchManager
    {
        private static Harmony _harmony;

        public static void LoadPatches()
        {
            MPUtil.WriteToConsole("Loading Harmony patches...");

            Harmony.DEBUG = true;
            _harmony = new Harmony("OCEAdmin.Bannerlord");

            LoadChatBoxPatch();
        }

        private static void LoadChatBoxPatch()
        {
            var original = typeof(ChatBox).GetMethod("ServerPrepareAndSendMessage", BindingFlags.NonPublic | BindingFlags.Instance);
            var prefix = typeof(PatchChatBox).GetMethod("Prefix");
            _harmony.Patch(original, prefix: new HarmonyMethod(prefix));
            MPUtil.WriteToConsole("Patched ChatBox::ServerPrepareAndSendMessage");
        }
    }
}
