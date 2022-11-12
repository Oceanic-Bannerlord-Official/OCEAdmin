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
            // Checking if the player is in a clan.
            if (!MPUtil.IsInClan(networkPeer))
            {
                MPUtil.SendChatMessage(networkPeer, "You aren't in a clan.");

                return true;
            }

            // Retrieving the character id from the player's current agent.
            string curUnit = MPUtil.GetUnitID(networkPeer);

            // Grabbing the clan string from the player's name.
            Clan clan = UniformManager.Instance.GetClan(MPUtil.GetClanTag(networkPeer));

            if(clan == null)
            {
                MPUtil.SendChatMessage(networkPeer,
                    string.Format("{0} is not registed with the uniform system.", clan.tag));

                return true;
            }

            ClanUniform uniform = clan.GetUniformForUnit(curUnit);

            if (uniform == null)
            {
                MPUtil.SendChatMessage(networkPeer, 
                    string.Format("{0} has no uniform for your current unit type of {1}.", clan.tag, curUnit));

                return true;
            }

            bool isOfficer = clan.officerIDs.Contains(MPUtil.GetPlayerID(networkPeer));
            List<Tuple<EquipmentIndex, string>> equipment = new List<Tuple<EquipmentIndex, string>>();

            foreach (UniformPart part in uniform.uniformParts)
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
