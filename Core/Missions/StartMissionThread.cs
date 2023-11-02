using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OCEAdmin.Core.Missions
{
    class StartMissionThread
    {
        // To successfully change the map, this thread must be called when the mission has ended
        public static void ThreadProc(Object missionData)
        {
            // Give us some buffer between the OnMissionEnd event and starting the next mission
            Thread.Sleep(500);

            // Prevent infinite loop if for some reason a call to StartMission 
            MissionHooks.Instance.StartMissionOnly((MissionData)missionData);
            MissionHooks.Instance.EndingCurrentMissionThenStartingNewMission = false;
        }
    }
}
