using OCEAdmin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade;

namespace OCEAdmin.Commands
{
    class Autoadmin : ICommand
    {
        public Role CanUse() => Role.Admin;

        public string Command() => "!Autoadmin";

        public string Description() => "Toggles the autoadmin setting for cavalry. !Autoadmin <optional true/false>";

        public CommandFeedback Execute(NetworkCommunicator networkPeer, string[] args)
        {
            bool toggleValue = false;

            // Check if args array has at least one element
            if (args.Length > 0)
            {
                // Check if the first argument is a valid boolean value
                if (bool.TryParse(args[0], out toggleValue))
                {
                    // Toggle the GroupfightMode based on the provided boolean value
                    SessionManager.AutoAdminSettings.DismountSystemEnabled = toggleValue;
                }
            }
            else
            {
                // Toggle the GroupfightMode without specifying true or false in args
                SessionManager.AutoAdminSettings.DismountSystemEnabled = !SessionManager.AutoAdminSettings.DismountSystemEnabled;
            }

            // Prepare the message for CommandFeedback
            string toggleStatus = SessionManager.GroupfightMode ? "enabled" : "disabled";
            string message = $"Auto admin (cavalry mounting) mode has been {toggleStatus}.";

            return new CommandFeedback(CommandLogType.BroadcastToAdmins, msg: message);
        }
    }
}
