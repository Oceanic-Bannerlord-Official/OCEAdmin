using NetworkMessages.FromServer;
using OCEAdmin.Core;
using OCEAdmin.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade;

namespace OCEAdmin.Commands
{
    class AutoAdmin : ICommand
    {
        public Permissions CanUse() => Permissions.Admin;

        public string Command() => "!autoadmin";

        public string Description() => "Toggles cavalry auto admin. <optional true|false>";

        public CommandFeedback Execute(NetworkCommunicator networkPeer, string[] args)
        {
            // Set it to be opposite of what we have currently.
            bool status = !ConfigManager.Instance.GetConfig().AutoAdminSettings.DismountSystemEnabled;

            // If there is a true or false value, override the toggle and make it this value.
            if (args.Length > 0)
            {
                bool.TryParse(args[0], out status);
            }

            // This won't save or persist to the config.
            ConfigManager.Instance.GetConfig().AutoAdminSettings.DismountSystemEnabled = status;

            CavalryDismountMissionBehavior missionBehavior = Mission.Current.GetMissionBehavior<CavalryDismountMissionBehavior>();

            if (ConfigManager.Instance.GetConfig().AutoAdminSettings.DismountSystemEnabled)
            {
                if (missionBehavior == null)
                {
                    Mission.Current.AddMissionBehavior(new CavalryDismountMissionBehavior());
                }
            }
            else
            {
                if (missionBehavior != null)
                {
                    Mission.Current.RemoveMissionBehavior(missionBehavior);
                }
            }

            string statusString = (status == true) ? "enabled" : "disabled";

            return new CommandFeedback(CommandLogType.BroadcastToAdmins,
                msg: string.Format("** Command ** {0} has {1} the cavalry dismount auto-admin.", networkPeer.UserName, statusString));
        }
    }
}
