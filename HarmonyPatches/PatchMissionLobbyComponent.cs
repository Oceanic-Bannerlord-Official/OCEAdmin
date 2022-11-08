using HarmonyLib;
using NetworkMessages.FromServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade;

namespace OCEAdmin.HarmonyPatches
{
    class PatchMissionLobbyComponent
    {
        public static bool Prefix(MissionLobbyComponent __instance, MissionPeer killerPeer, Agent killedAgent, MissionPeer assistorPeer)
        {
            if (killedAgent.MissionPeer == null)
            {
                NetworkCommunicator networkCommunicator = GameNetwork.NetworkPeers.SingleOrDefault((NetworkCommunicator x) => x.GetComponent<MissionPeer>() != null && x.GetComponent<MissionPeer>().ControlledFormation != null && x.GetComponent<MissionPeer>().ControlledFormation == killedAgent.Formation);
                if (networkCommunicator != null)
                {
                    MissionPeer component = networkCommunicator.GetComponent<MissionPeer>();
                    killerPeer.OnKillAnotherPeer(component);
                }
            }
            else
            {
                killerPeer.OnKillAnotherPeer(killedAgent.MissionPeer);
            }
            MissionMultiplayerGameModeBase gameMode = Traverse.Create(__instance).Field("_gameMode").GetValue() as MissionMultiplayerGameModeBase;
            if (gameMode != null)
            {
                if (killerPeer.ControlledAgent != null)
                {
                    TaleWorlds.MountAndBlade.Team killerTeam = killerPeer.Team;
                    killerTeam = killerPeer.ControlledAgent.Team;

                    if (killerTeam != null)
                    {
                        if (killerTeam.IsEnemyOf(killedAgent.Team))
                        {
                            Traverse.Create(killerPeer).Field("_score").SetValue(killerPeer.Score + gameMode.GetScoreForKill(killedAgent));
                            Traverse.Create(killerPeer).Field("_killCount").SetValue(killerPeer.KillCount + 1);
                        }
                        else
                        {
                            Traverse.Create(killerPeer).Field("_score").SetValue((int)((float)gameMode.GetScoreForKill(killedAgent) * 1.5f));
                            Traverse.Create(killerPeer).Field("_killCount").SetValue(killerPeer.KillCount - 1);
                        }
                    }

                }

            }
            MissionScoreboardComponent missionScoreboardComponent = Traverse.Create(__instance).Field("_missionScoreboardComponent").GetValue() as MissionScoreboardComponent;

            if (missionScoreboardComponent != null)
            {
                missionScoreboardComponent.PlayerPropertiesChanged(killerPeer.GetNetworkPeer());
            }

            GameNetwork.BeginBroadcastModuleEvent();
            GameNetwork.WriteMessage(new KillDeathCountChange(killerPeer.GetNetworkPeer(), null, killerPeer.KillCount, killerPeer.AssistCount, killerPeer.DeathCount, killerPeer.Score));
            GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
            return false;
        }
    }
}
