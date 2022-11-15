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
    class ArcherLimit : Command
    {
        public Permissions CanUse() => Permissions.Admin;

        public string Command() => "!archerlimit";

        public string Description() => "Sets the limit on archers <int amount>. % is usable.";

        public CommandFeedback Execute(NetworkCommunicator networkPeer, string[] args)
        {
            int amount = -1;

            if (!ConfigManager.Instance.GetConfig().SpecialistSettings.Enabled)
            {
                return new CommandFeedback(CommandLogType.Player, 
                    msg: "Specialist limits are disabled. !speclimit true to enable them.",
                    peer: networkPeer);
            }

            if (args[0] == null)
            {
                return new CommandFeedback(CommandLogType.Player, msg: "Invalid arguments.", peer: networkPeer);
            }

            // Setting a archer specialist limit with a percentage.
            if(args[0].Contains("%")) {
                int.TryParse(args[0].Replace("%", ""), out amount);

                if (amount < 0 || amount > 100)
                {
                    return new CommandFeedback(CommandLogType.Player, msg: "Percentage must be a number between 0% and 100%.",
                        peer: networkPeer);
                }

                ConfigManager.Instance.GetConfig().SpecialistSettings.ArcherLimit = amount;
                ConfigManager.Instance.GetConfig().SpecialistSettings.UseArcherPercentage = true;

                return new CommandFeedback(CommandLogType.BroadcastToAdmins, 
                    msg: string.Format("** Command ** {0} has set the archer specialist limit to {1}%.", networkPeer.UserName, amount));
            }

            int.TryParse(args[0], out amount);

            // Setting a archer specialist with a standard fixed number.
            if (amount < 0)
            {
                return new CommandFeedback(CommandLogType.Player, msg: "Number is not an integer above -1.",
                    peer: networkPeer);
            }

            ConfigManager.Instance.GetConfig().SpecialistSettings.ArcherLimit = amount;
            ConfigManager.Instance.GetConfig().SpecialistSettings.UseArcherPercentage = false;

            return new CommandFeedback(CommandLogType.BroadcastToAdmins,
                msg: string.Format("** Command ** {0} has set the archer specialist limit to {1}.", networkPeer.UserName, amount));
        }
    }
}
