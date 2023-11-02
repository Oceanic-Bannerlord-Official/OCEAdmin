using NetworkMessages.FromServer;
using OCEAdmin.Core;
using OCEAdmin.Core.Permissions;
using OCEAdmin.Plugins.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade;

namespace OCEAdmin.Plugins.Admin
{
    class GetSpecs : ICommand
    {
        public Role CanUse() => Role.Admin;

        public string Command() => "!getspecs";

        public string Description() => "Returns all the specialists for both teams in your chatbox. !getspecs";

        public CommandFeedback Execute(NetworkCommunicator networkPeer, string[] args)
        {
            Tuple<TeamComposition, TeamComposition> teams = MPUtil.GetTeamsComposition();

            networkPeer.SendChatMessage(string.Format("{0}: {1} Infantry, {2} Cavalry, {3} Archer(s). {4} Total.",
                teams.Item1.culture.Name,
                teams.Item1.GetCompositionAmount(CompositionType.Infantry),
                teams.Item1.GetCompositionAmount(CompositionType.Cavalry),
                teams.Item1.GetCompositionAmount(CompositionType.Archer),
                teams.Item1.GetTotal()));

            networkPeer.SendChatMessage(string.Format("{0}: {1} Infantry, {2} Cavalry, {3} Archer(s). {4} Total.",
                teams.Item2.culture.Name,
                teams.Item2.GetCompositionAmount(CompositionType.Infantry),
                teams.Item2.GetCompositionAmount(CompositionType.Cavalry),
                teams.Item2.GetCompositionAmount(CompositionType.Archer),
                teams.Item2.GetTotal()));

            // Handle it ourself.
            return new CommandFeedback(CommandLogType.None);
        }
    }
}
