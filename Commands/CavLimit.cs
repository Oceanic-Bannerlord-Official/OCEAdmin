﻿using NetworkMessages.FromServer;
using OCEAdmin.Core;
using PersistentEmpires;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade;

namespace OCEAdmin.Commands
{
    class CavLimit : ICommand
    {
        public Role CanUse() => Role.Admin;

        public string Command() => "!cavlimit";

        public string Description() => "Sets the limit on cavalry <int amount>.";

        public CommandFeedback Execute(NetworkCommunicator networkPeer, string[] args)
        {
            int amount = -1;

            if (!SessionManager.SpecialistSettings.Enabled)
            {
                return new CommandFeedback(CommandLogType.Player,
                    msg: "Specialist limits are disabled. !speclimit true to enable them.",
                    peer: networkPeer);
            }

            if (args[0] == null)
            {
                return new CommandFeedback(CommandLogType.Player, msg: "Invalid arguments.", peer: networkPeer);
            }

            int.TryParse(args[0], out amount);

            // Setting a cavalry specialist with a standard fixed number.
            if (amount < 0)
            {
                return new CommandFeedback(CommandLogType.Player, msg: "Number is not an integer above -1.",
                    peer: networkPeer);
            }

            SessionManager.SpecialistSettings.CavLimit = amount;

            return new CommandFeedback(CommandLogType.BroadcastToAdmins,
                msg: string.Format("** Command ** {0} has set the cavalry specialist limit to {1}.", networkPeer.UserName, amount));
        }
    }
}