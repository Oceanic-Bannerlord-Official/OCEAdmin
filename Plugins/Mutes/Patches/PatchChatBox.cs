using HarmonyLib;
using OCEAdmin.Plugins.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace OCEAdmin.Plugins.Mutes
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

            LoggingPlugin logger = OCEAdminSubModule.GetPlugin<LoggingPlugin>();

            if (logger != null)
            {
                logger.Add(fromPeer.GetUsername() + " types in chat: " + message);
            }

            return true;
        }
    }
}
