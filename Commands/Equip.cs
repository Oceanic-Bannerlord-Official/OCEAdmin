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

namespace OCEAdmin.Commands
{
    class Equip : Command
    {
        public bool CanUse(NetworkCommunicator networkPeer)
        {
            bool isAdmin = false;
            bool isExists = AdminManager.Admins.TryGetValue(networkPeer.VirtualPlayer.Id.ToString(), out isAdmin);
            return isExists && isAdmin;
        }

        public string Command()
        {
            return "!equip";
        }

        public string Description()
        {
            return "Equip a clan armor set for your current unit. 30s internal CD.";
        }

        // Include Internal CD: 30s.
        // Remember and apply health to new agent.
        // Color shield.
        public bool Execute(NetworkCommunicator networkPeer, string[] args)
        {
            UniformManager uniformManager = AdminPanel.Instance.uniformManager;

            // Checking if the player is in a clan.
            if (!MPUtil.IsInClan(networkPeer))
            {
                MPUtil.SendChatMessage(networkPeer, "You aren't in a clan.");

                return true;
            }

            // Grabbing the clan string from the player's name.
            string clan = MPUtil.GetClanTag(networkPeer);

            // Retrieving the character id from the player's current agent.
            string curUnit = MPUtil.GetUnitID(networkPeer);

            if (!uniformManager.HasClanUniformForUnit(clan, curUnit))
            {
                MPUtil.SendChatMessage(networkPeer, 
                    string.Format("{0} has no uniform for your current unit type of {1}.", clan, curUnit));

                return true;
            }

            ClanUniform clanUniform = uniformManager.GetUniform(clan);
            bool isOfficer = clanUniform.officerIDs.Contains(MPUtil.GetPlayerID(networkPeer));
            List<Tuple<EquipmentIndex, string>> equipment = new List<Tuple<EquipmentIndex, string>>();

            foreach (UniformPart part in clanUniform.uniformParts)
            {
                equipment.Add(new Tuple<EquipmentIndex, string>(part.itemSlot, part.GetPart(isOfficer)));
            }

            if(equipment != null)
            {
                AdminPanel.Instance.GivePlayerAgentCosmeticEquipment(networkPeer, equipment);
            }
                   

            return true;
        }
    }
}
