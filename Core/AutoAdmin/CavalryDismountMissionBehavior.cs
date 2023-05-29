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

        public override void AfterStart() { }

        public override void OnClearScene() { }

        public override void OnAgentMount(Agent agent)
        {
            base.OnAgentMount(agent);

            if (!ShouldAffect(agent))
                return;

            agent.SetMaximumSpeedLimit(0f, true);
        }

        public override void OnAgentDismount(Agent agent)
        {
            base.OnAgentDismount(agent);

            if (!ShouldAffect(agent))
                return;

            agent.SetMaximumSpeedLimit(1f, true);
        }

        public bool ShouldAffect(Agent agent)
        {
            if (!Config.Get().AutoAdminSettings.DismountSystemEnabled)
                return false;

            //if (AdminPanel.Instance.GetWarmupState() == MultiplayerWarmupComponent.WarmupStates.InProgress)
            //return false;

            if (agent.IsAIControlled)
                return false;

            if (!agent.SpawnEquipment[TaleWorlds.Core.EquipmentIndex.Horse].IsEmpty)
                return false;

            return true;
        }
    }
}
