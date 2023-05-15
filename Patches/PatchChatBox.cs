using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace OCEAdmin.Patches
{
    class PatchChatBox
    {
        public static bool Prefix(ChatBox __instance, NetworkCommunicator fromPeer, bool toTeamOnly, string message) {
            bool textShown = !message.StartsWith("!") || !fromPeer.IsMuted;

            if(fromPeer.IsMuted)
            {
                MPUtil.SendChatMessage(fromPeer, "You are muted.");
            }

            return textShown;
        }
    }
}
