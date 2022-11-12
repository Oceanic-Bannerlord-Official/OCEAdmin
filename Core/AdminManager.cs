using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace OCEAdmin.Core
{
    public class AdminManager
    {
        public static Dictionary<string, bool> Admins = new Dictionary<string, bool>();

        public static bool PlayerIsAdmin(string peerId)
        {
            if(ConfigManager.Instance.Admins != null)
            {
                foreach (var adminInfo in ConfigManager.Instance.Admins)
                {
                    if (peerId == adminInfo)
                    {
                        Debug.Print(string.Format("Adding peer ID {0} to whitelisted admins.", adminInfo), 0, Debug.DebugColor.Green);
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
