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

        public string Description() => "Toggles cavalry auto admin. <optional true|false>";

        public CommandFeedback Execute(NetworkCommunicator networkPeer, string[] args)
        {
            // Set it to be opposite of what we have currently.
            bool status = !ConfigManager.Instance.GetConfig().AutoAdminSettings.DismountSystemEnabled;

            // If there is a true or false value, override the toggle and make it this value.
            if (args[0] != null)
            {
                bool.TryParse(args[0], out status);
            }

            // This won't save or persist to the config.
            ConfigManager.Instance.GetConfig().AutoAdminSettings.DismountSystemEnabled = status;



            return new CommandFeedback(CommandLogType.Player,
                msg: string.Format("** Command ** {0} has enabled cavalry dismonut auto-admin.", networkPeer.UserName));
        }
    }
}
