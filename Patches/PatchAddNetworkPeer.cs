using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade.Diamond;
using Messages.FromCustomBattleServerManager.ToCustomBattleServer;
using HarmonyLib;
using TaleWorlds.MountAndBlade;
using TaleWorlds.Core;
using TaleWorlds.Library;
using System.Reflection;

namespace OCEAdmin.Patches
{
    class PatchAddNetworkPeer
    {
        public static bool Prefix(NetworkCommunicator networkPeer)
        {
            //Traverse.Create(networkPeer.VirtualPlayer).Property("UserName").SetValue("[ASTG] Adolphus is the best");

            MBNetwork.NetworkPeers.Add(networkPeer);

            Debug.Print("> ModifiedAddNetworkPeer: " + networkPeer.UserName, 0, Debug.DebugColor.White, 17179869184UL);

            return false;
        }

    }
}
