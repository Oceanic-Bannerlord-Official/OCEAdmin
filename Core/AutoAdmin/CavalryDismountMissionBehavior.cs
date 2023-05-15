using OCEAdmin.Core;
using OCEAdmin.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade;

namespace OCEAdmin.Features
{
    public class CavalryDismountMissionBehavior : MissionLogic
    {
        public override MissionBehaviorType BehaviorType => MissionBehaviorType.Logic;

        public List<NetworkCommunicator> alreadyWarnedPlayers;

        public override void AfterStart()
        {
            MPUtil.WriteToConsole("CavalryDismountMissionBehavior loaded.");
            alreadyWarnedPlayers = new List<NetworkCommunicator>();
        }

        public override void OnClearScene()
        {
            alreadyWarnedPlayers = new List<NetworkCommunicator>();
        }

        public override void OnAgentMount(Agent agent)
        {
            if (!Config.Get().AutoAdminSettings.DismountSystemEnabled)
                return;

            if (AdminPanel.Instance.GetWarmupState() == MultiplayerWarmupComponent.WarmupStates.InProgress)
                return;

            if (agent.IsAIControlled)
                return;   

            NetworkCommunicator networkPeer = agent.MissionPeer.GetNetworkPeer();     

            if (networkPeer != null)
            {
                // Exclude all agents that start with horses from the slay script.
                if(!agent.SpawnEquipment[TaleWorlds.Core.EquipmentIndex.Horse].IsEmpty)
                    return;

                // If the peer already has a countdown, we don't need to create another.
                if (CountdownTimer.PeerHasCountdown(networkPeer))
                    return;

                NetworkCommunicator alreadyWarnedPlayer = alreadyWarnedPlayers.FirstOrDefault(s => s == networkPeer);

                // If they've already been warned, instantly slay them.
                if(alreadyWarnedPlayer != null)
                {
                    Slay(alreadyWarnedPlayer);

                    return;
                }

                MPUtil.SendChatMessage(networkPeer, string.Format("** DISMOUNT ** - You will be slain in {0}s", 
                    Config.Get().AutoAdminSettings.DismountSlayTime));

                MPUtil.SendChatMessage(networkPeer, "** DISMOUNT ** - You are not the correct class for this mount.");

                CountdownTimer countdown = new CountdownTimer(networkPeer, Config.Get().AutoAdminSettings.DismountSlayTime);
                countdown.OnTimerComplete += PunishPlayer;
            }
        }

        public void PunishPlayer(object sender, CountdownTimerEventArgs e)
        {
            // Make sure the player isn't dead.
            if (e.networkPeer.ControlledAgent == null)
                return;

            // The player has dismounted, we don't need to punish.
            // Add them to the already warned list for next time.
            if (!e.networkPeer.ControlledAgent.HasMount)
            {
                alreadyWarnedPlayers.Add(e.networkPeer);

                MPUtil.SendChatMessage(e.networkPeer, "You have been spared from a slay. Do not mount a horse again.");

                return;
            }

            Slay(e.networkPeer);
        }

        private void Slay(NetworkCommunicator networkPeer)
        {
            MPUtil.Slay(networkPeer);
            MPUtil.SendChatMessage(networkPeer, "You have been slain by auto-admin.");
            MPUtil.BroadcastToAdmins(string.Format("** ADMIN ** {0} has been slain for mounting a horse as the wrong class.", networkPeer.UserName));
        }
    }
}
