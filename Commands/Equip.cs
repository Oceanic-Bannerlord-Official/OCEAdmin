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

namespace ChatCommands.Commands
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
            return "Command to equip a beefy set of armor";
        }

        // Include Internal CD: 30s.
        // Remember and apply health to new agent.
        // Color shield.
        public bool Execute(NetworkCommunicator networkPeer, string[] args)
        {
            string username = networkPeer.VirtualPlayer.UserName;

            if(username.Substring(0,1) != "[")
            {
                GameNetwork.BeginModuleEventAsServer(networkPeer);
                GameNetwork.WriteMessage(new ServerMessage("You aren't in a clan."));
                GameNetwork.EndModuleEventAsServer();

                return true;
            }

            string clan = username.Substring(username.IndexOf("[")+1, username.IndexOf("]")-1);

            Debug.Print(string.Format("Attempting to equip for clan: {0}", clan), 0, Debug.DebugColor.Green);

            List<Tuple<EquipmentIndex, string>> equipment = new List<Tuple<EquipmentIndex, string>>();

            if (AdminPanel.Instance.uniformManager.HasClanUniform(clan))
            {
                ClanUniform clanUniform = AdminPanel.Instance.uniformManager.GetUniformList(clan);

                Debug.Print(string.Format("Equipping for clan: {0}", clan), 0, Debug.DebugColor.Green);
                bool isOfficer = false;

                foreach(string officerID in clanUniform.officerIDs)
                {
                    if (networkPeer.VirtualPlayer.Id.ToString() == officerID)
                    {
                        isOfficer = true;
                        break;
                    }
                }

                Debug.Print(string.Format("IsOfficer: {0}", isOfficer), 0, Debug.DebugColor.Green);

                foreach (UniformPart part in clanUniform.uniformParts)
                {
                    string itemName = "";

                    if(isOfficer)
                    {
                        itemName = part.officerParts[new Random().Next(0, part.officerParts.Count-1)];
                    } else
                    {
                        itemName = part.parts[new Random().Next(0, part.parts.Count-1)];
                    }

                    Debug.Print(string.Format("Item name: {0}", itemName), 0, Debug.DebugColor.Green);

                    equipment.Add(new Tuple<EquipmentIndex, string>(part.itemSlot, itemName));
                }

                if(equipment != null)
                {
                    AdminPanel.Instance.GivePlayerAgentCosmeticEquipment(networkPeer, equipment);
                }
            }        

            return true;
        }
    }
}
