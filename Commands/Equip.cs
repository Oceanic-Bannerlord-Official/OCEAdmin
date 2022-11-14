using NetworkMessages.FromServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.ObjectSystem;
using OCEAdmin.Core;

namespace OCEAdmin.Commands
{
    class Equip : Command
    {
        public Permissions CanUse() => Permissions.Admin;
        public string Command() => "!equip";

        public string Description() => "Equip a clan armor set for your current unit. 30s internal CD.";

        // Include Internal CD: 30s.
        // Remember and apply health to new agent.
        // Color shield.
        public CommandFeedback Execute(NetworkCommunicator networkPeer, string[] args)
        {
            // Checking if the player is in a clan.
            if (!MPUtil.IsInClan(networkPeer))
            {
                return new CommandFeedback(CommandLogType.Player, msg: "You aren't in a clan.",
                    peer: networkPeer);
            }

            // Retrieving the character id from the player's current agent.
            string curUnit = MPUtil.GetUnitID(networkPeer);

            // Grabbing the clan string from the player's name.
            Clan clan = UniformManager.Instance.GetClan(MPUtil.GetClanTag(networkPeer));

            if (clan == null)
            {
                return new CommandFeedback(CommandLogType.Player,
                    msg: string.Format("{0} is not registed with the uniform system.", clan.tag),
                    peer: networkPeer);
            }

            ClanUniform uniform = clan.GetUniformForUnit(curUnit);

            if (uniform == null)
            {
                return new CommandFeedback(CommandLogType.Player,
                    msg: string.Format("{0} has no uniform for your current unit type of {1}.", clan.tag, curUnit),
                    peer: networkPeer);
            }

            bool isOfficer = clan.officerIDs.Contains(MPUtil.GetPlayerID(networkPeer));
            List<Tuple<EquipmentIndex, string>> equipment = new List<Tuple<EquipmentIndex, string>>();

            foreach (UniformPart part in uniform.uniformParts)
            {
                equipment.Add(new Tuple<EquipmentIndex, string>(part.itemSlot, part.GetPart(isOfficer)));
            }

            if (equipment != null)
            {
                AdminPanel.Instance.GivePlayerAgentCosmeticEquipment(networkPeer, equipment);
            }


            return new CommandFeedback(CommandLogType.Player, msg: "Uniform set!", peer: networkPeer);
        }
    }
}
