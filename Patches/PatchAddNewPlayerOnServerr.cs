using Newtonsoft.Json;
using OCEAdmin.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace OCEAdmin
{
    public class PatchAddNewPlayerOnServer
    {
        public static bool Prefix(ref PlayerConnectionInfo playerConnectionInfo)
        {
            string name = playerConnectionInfo.Name;

            if (name.Contains("{") || name.Contains("}"))
            {
                playerConnectionInfo.Name = name.Replace("{", "").Replace("}", ""); // Remove curly braces
            }

            LogManager.Add(playerConnectionInfo.Name + " (" + playerConnectionInfo.PlayerID.ToString() + ") is connecting to the server.");

            return true;
        }
    }
}
