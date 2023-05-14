using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade;

namespace OCEAdmin.Core
{
    public class PatchMissionStateDefaults
    {
        public static IEnumerable<MissionBehavior> Postfix(IEnumerable<MissionBehavior> __result)
        {
            __result = new List<MissionBehavior>() { (MissionBehavior) new PlayerExtensionComponentBehavior() }.Concat<MissionBehavior>(__result);
            return __result;
        }
    }
}
