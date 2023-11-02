using System.Threading;
using TaleWorlds.MountAndBlade;
using TaleWorlds.Core;

namespace OCEAdmin.Core.Missions
{
    class MissionListener : IMissionListener
    {
        MissionData missionData;

        public void setMissionData(MissionData missionData)
        {
            this.missionData = missionData;
        }

        public void OnConversationCharacterChanged()
        {

        }

        public void OnEndMission()
        {
            // Run a thread that will create a start a mission after a delay
            Thread t = new Thread(new ParameterizedThreadStart(StartMissionThread.ThreadProc));
            t.Start(missionData);

            TaleWorlds.MountAndBlade.Mission.Current.RemoveListener(this);
        }

        public void OnEquipItemsFromSpawnEquipment(Agent agent, Agent.CreationType creationType)
        {

        }

        public void OnEquipItemsFromSpawnEquipmentBegin(Agent agent, Agent.CreationType creationType)
        {

        }

        public void OnInitialDeploymentPlanMade(BattleSideEnum battleSide, bool isFirstPlan)
        {

        }

        public void OnMissionModeChange(MissionMode oldMissionMode, bool atStart)
        {

        }

        public void OnResetMission()
        {

        }
    }
}
