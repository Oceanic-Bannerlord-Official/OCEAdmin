using HarmonyLib;
using NetworkMessages.FromServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.ObjectSystem;

namespace OCEAdmin.Patches
{
    class PatchGetUsedCosmeticsFromPeer
    {
		public static bool Prefix(out MissionPeer __state, MissionPeer missionPeer)
        {
			__state = missionPeer;

			MPUtil.WriteToConsole(__state.GetNetworkPeer().UserName);
			return true;
        }

		public static void Postfix(ref Dictionary<string, string> __result, MissionPeer __state)
        {
			NetworkCommunicator networkPeer = __state.GetNetworkPeer();

			if (UniformManager.Instance.PlayerHasUniform(networkPeer))
			{
				__result = UniformManager.Instance.GetUniformCosmeticsDictionary(networkPeer);
			}
		}
	}
}
