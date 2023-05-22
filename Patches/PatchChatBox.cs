using HarmonyLib;
using OCEAdmin.Core.Logging;
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
            if(fromPeer.IsMuted())
            {
                fromPeer.SendChatMessage("You are muted.");
                return false;
            }
            else if(message.StartsWith("!"))
            { 
                return false;
            }

            LogManager.Add(fromPeer.GetUsername() + " types in chat: " + message);

            return true;
        }
    }
}
