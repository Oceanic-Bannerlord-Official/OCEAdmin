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
    class SpecLimit : Command
    {
        public Permissions CanUse() => Permissions.Admin;

        public string Command() => "!speclimit";

        public string Description() => "Toggles specialist limits. <optional true|false>";

        public CommandFeedback Execute(NetworkCommunicator networkPeer, string[] args)
        {
            // Set it to be opposite of what we have currently.
            bool status = !ConfigManager.Instance.GetConfig().SpecialistSettings.Enabled;

            // If there is a true or false value, override the toggle and make it this value.
            if (args[0] != null)
            {
                bool.TryParse(args[0], out status);
            }

            // This won't save or persist to the config.
            ConfigManager.Instance.GetConfig().SpecialistSettings.Enabled = status;

            return new CommandFeedback(CommandLogType.Player,
                msg: string.Format("** Command ** {0} has enabled specialist limits.", networkPeer.UserName));
        }
    }
}
