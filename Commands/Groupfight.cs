using OCEAdmin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade;

namespace OCEAdmin.Commands
{
    class Groupfight : ICommand
    {
        public Role CanUse() => Role.Admin;

        public string Command() => "!groupfight";

        public string Description() => "Toggles the groupfight setting. !groupfight <optional true/false>";

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
                    SessionManager.GroupfightMode = toggleValue;
                }
            }
            else
            {
                // Toggle the GroupfightMode without specifying true or false in args
                SessionManager.GroupfightMode = !SessionManager.GroupfightMode;
            }

            // Prepare the message for CommandFeedback
            string toggleStatus = SessionManager.GroupfightMode ? "enabled" : "disabled";
            string message = $"Groupfight mode has been {toggleStatus}.";

            return new CommandFeedback(CommandLogType.Broadcast, msg: message);
        }
    }
}
