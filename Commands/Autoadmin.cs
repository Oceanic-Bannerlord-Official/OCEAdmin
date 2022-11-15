using NetworkMessages.FromServer;
using OCEAdmin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade;

namespace OCEAdmin.Commands
{
    class AutoAdmin : Command
    {
        public Permissions CanUse() => Permissions.Admin;

        public string Command() => "!autoadmin";

        public string Description() => "Toggles cavalry auto admin. <true|false>";

        public CommandFeedback Execute(NetworkCommunicator networkPeer, string[] args)
        {
            if(args[0] == null)
                return new CommandFeedback(CommandLogType.Player, msg: "Invalid arguments.",
                    peer: networkPeer);

            if(args[0].ToLower() != "true" || args[0].ToLower() != "false")
                return new CommandFeedback(CommandLogType.Player, msg: "Autoadmin must be set true or false.",
                    peer: networkPeer);

            bool status = bool.Parse(args[0]);

            // This won't save or persist to the config.
            ConfigManager.Instance.GetConfig().AutoAdminSettings.DismountSystemEnabled = status;

            return new CommandFeedback(CommandLogType.Player, msg: "AutoAdmin has been toggled to the setting: " + args[0].ToLower(),
                peer: networkPeer);
        }
    }
}
