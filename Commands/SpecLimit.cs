using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NetworkMessages.FromServer;
using OCEAdmin.Core;
using OCEAdmin.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade;
using PersistentEmpires;

namespace OCEAdmin.Commands
{
    class SpecLimit : ICommand
    {
        public Role CanUse() => Role.Admin;

        public string Command() => "!speclimit";

        public string Description() => "Toggles specialist limits. <optional true|false>";

        public CommandFeedback Execute(NetworkCommunicator networkPeer, string[] args)
        {
            // Set it to be opposite of what we have currently.
            bool status = !SessionManager.SpecialistSettings.Enabled;

            // If there is a true or false value, override the toggle and make it this value.
            if (args.Length > 0)
            {
                bool.TryParse(args[0], out status);
            }

            // This won't save or persist to the config.
            SessionManager.SpecialistSettings.Enabled = status;

            string statusString = (status == true) ? "enabled" : "disabled";

            return new CommandFeedback(CommandLogType.BroadcastToAdmins,
                msg: string.Format("** Command ** {0} has {1} the specialist management behaviour script.", networkPeer.UserName, statusString));
        }
    }
}
