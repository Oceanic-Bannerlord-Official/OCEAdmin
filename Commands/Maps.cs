using NetworkMessages.FromServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.DedicatedCustomServer;
using OCEAdmin.Core;

namespace OCEAdmin.Commands
{

    class Maps : ICommand
    {
        public Role CanUse() => Role.Admin;

        public string Command() => "!maps";

        public string Description() => "Lists available maps for the current, or a different, game type. !maps <game type>";

        public CommandFeedback Execute(NetworkCommunicator networkPeer, string[] args)
        {
            List<string> availableMaps = new List<string>();

            if(args.Length == 1)
            {
                availableMaps = AdminPanel.Instance.GetMapsForGameType(args[0]);
            }
            else
            {
                availableMaps = AdminPanel.Instance.GetAllAvailableMaps();
            }

            MPUtil.SendChatMessage(networkPeer, "Maps: ");

            foreach (var map in availableMaps)
            {
                MPUtil.SendChatMessage(networkPeer, map);
            }

            string currentMapId = "";
            MultiplayerOptions.Instance.GetOptionFromOptionType(MultiplayerOptions.OptionType.Map).GetValue(out currentMapId);

            MPUtil.SendChatMessage(networkPeer, string.Format("Current map: {0}", currentMapId));

            return new CommandFeedback(CommandLogType.None);
        }
    }
}
