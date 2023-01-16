﻿using NetworkMessages.FromServer;
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
    class SpecLimit : ICommand
    {
        public Role CanUse() => Role.Admin;

        public string Command() => "!speclimit";

        public string Description() => "Toggles specialist limits. <optional true|false>";

        public CommandFeedback Execute(NetworkCommunicator networkPeer, string[] args)
        {
            // Set it to be opposite of what we have currently.
            bool status = !ConfigManager.Instance.GetConfig().SpecialistSettings.Enabled;

            // If there is a true or false value, override the toggle and make it this value.
            if (args.Length > 0)
            {
                bool.TryParse(args[0], out status);
            }

            // This won't save or persist to the config.
            ConfigManager.Instance.GetConfig().SpecialistSettings.Enabled = status;

            if(ConfigManager.Instance.GetConfig().SpecialistSettings.Enabled)
            {
                Mission.Current.AddMissionBehavior(new SpecialistLimitMissionBehavior());
            } else
            {
                SpecialistLimitMissionBehavior missionBehavior = Mission.Current.GetMissionBehavior<SpecialistLimitMissionBehavior>();

                if (missionBehavior != null)
                {
                    Mission.Current.RemoveMissionBehavior(missionBehavior);
                }
            }

            string statusString = (status == true) ? "enabled" : "disabled";

            return new CommandFeedback(CommandLogType.BroadcastToAdmins,
                msg: string.Format("** Command ** {0} has {1} the specialist management behaviour script.", networkPeer.UserName, statusString));
        }
    }
}
