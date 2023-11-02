using NetworkMessages.FromClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade.Network.Messages;
using TaleWorlds.MountAndBlade;
using OCEAdmin.Core;
using System.Reflection;

namespace OCEAdmin.Plugins.Groupfight
{
    public class GroupfightGameHandler : GameHandler
    {
        public override void OnAfterSave() { }

        public override void OnBeforeSave() { }

        protected override void OnGameNetworkBegin()
        {
            base.OnGameNetworkBegin();
            this.AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegisterer.RegisterMode.Add);
        }
        protected override void OnGameNetworkEnd()
        {
            base.OnGameNetworkEnd();
            this.AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegisterer.RegisterMode.Remove);
        }

        private void AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegisterer.RegisterMode mode)
        {
            GameNetwork.NetworkMessageHandlerRegisterer registerer = new GameNetwork.NetworkMessageHandlerRegisterer(mode);

            if (GameNetwork.IsServer)
            {
                registerer.Register<RequestTroopIndexChange>(new GameNetworkMessage.ClientMessageHandlerDelegate<RequestTroopIndexChange>(this.HandleClientEventLobbyEquipmentUpdated));
            }
        }

        private bool HandleClientEventLobbyEquipmentUpdated(NetworkCommunicator peer, RequestTroopIndexChange message)
        {
            if (!SessionManager.GroupfightMode)
                return true;

            if (IsProhibitedUnit(peer, message.SelectedTroopIndex))
            {
                peer.SendChatMessage("♙ | Groupfight mode enabled. Class swap prohibited.");
                return false;
            }

            return true;
        }

        public bool IsProhibitedUnit(NetworkCommunicator peer, int unitIndex)
        {
            BasicCultureObject culture = MPUtil.GetTeamCulture(peer);
            List<MultiplayerClassDivisions.MPHeroClass> classes = MultiplayerClassDivisions.GetMPHeroClasses(culture).ToList<MultiplayerClassDivisions.MPHeroClass>();

            if (classes[unitIndex] != null)
            {
                MultiplayerClassDivisions.MPHeroClass selection = classes[unitIndex];

                if (MPUtil.cavClasses.Contains(selection.IconType))
                {
                    return true;
                }
                else if (MPUtil.archerClasses.Contains(selection.IconType))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
