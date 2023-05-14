using NetworkMessages.FromServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade;
using OCEAdmin.Core;

namespace OCEAdmin.Commands
{
    class Login : ICommand
    {
        public Role CanUse() => Role.Player;

        public string Command() => "!login";

        public string Description() => "Used to login with the admin password. Usage !login <password>";

        public CommandFeedback Execute(NetworkCommunicator networkPeer, string[] args)
        {
            if(!ConfigManager.Instance.GetConfig().AllowLoginCommand)
            {
                return new CommandFeedback(CommandLogType.Player, 
                    msg: "** Command ** The login command has been disabled from the config file.", peer: networkPeer);
            }

            if (args.Length == 0 || args.Length > 1) {
                return new CommandFeedback(CommandLogType.Player,
                  msg: "Please only provide a password. Usage: !login <password>", peer: networkPeer);
            }

            String password = args[0];
            Config config = ConfigManager.Instance.GetConfig();

            if (!password.Equals(config.AdminPassword)) {
                return new CommandFeedback(CommandLogType.Player,
                  msg: "Incorrect password.", peer: networkPeer);
            }

            PlayerExtensionComponent component = networkPeer.GetPlayerExtensionComponent();
            component.UpdateRole(Role.Admin);

            return new CommandFeedback(CommandLogType.BroadcastToAdminsAndTarget, 
                msg: string.Format("** Login ** {0} has logged in with the admin password!", networkPeer.UserName),
                targetMsg: "Login successful. Welcome!", targetPeer: networkPeer);
        }
    }
}
