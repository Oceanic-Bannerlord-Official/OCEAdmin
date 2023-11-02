using Newtonsoft.Json;
using OCEAdmin.Plugins.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.MountAndBlade;

namespace OCEAdmin.Plugins.Logging
{
    public class LogPatchAddNewPlayerOnServer
    {
        public static bool Prefix(ref PlayerConnectionInfo playerConnectionInfo)
        {
            string name = playerConnectionInfo.Name;

            LoggingPlugin logger = OCEAdminSubModule.GetPlugin<LoggingPlugin>();

            if (logger != null)
            {
                logger.Add(playerConnectionInfo.Name + " (" + playerConnectionInfo.PlayerID.ToString() + ") is connecting to the server.");
            }

            return true;
        }
    }
}
