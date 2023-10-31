using NetworkMessages.FromClient;
using PersistentEmpires;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade.Network.Messages;
using TaleWorlds.MountAndBlade;
using OCEAdmin.Core;

namespace OCEAdmin
{
    public class SpecialistLimitGameHandler : GameHandler
    {
        public override void OnAfterSave() { }

        public override void OnBeforeSave() { }

        protected override void OnGameNetworkBegin()
        {
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

        // We reroute our methods to the mission behaviour because we have more access to the mission state.
        private bool HandleClientEventLobbyEquipmentUpdated(NetworkCommunicator peer, RequestTroopIndexChange message)
        {
            if (!SessionManager.SpecialistSettings.Enabled)
                return true;

            if (Mission.Current.GetMissionBehavior<SpecialistLimitMissionBehavior>() == null)
            {
                return true;
            }

            return Mission.Current.GetMissionBehavior<SpecialistLimitMissionBehavior>().RequestTroopChange(peer, message);
        }

        protected override void OnPlayerDisconnect(VirtualPlayer peer)
        {
            SpecialistLimitMissionBehavior mb = Mission.Current.GetMissionBehavior<SpecialistLimitMissionBehavior>();

            if (mb != null)
            {
                if((NetworkCommunicator)peer.Communicator != null)
                {
                    mb.OnPlayerDisconnect((NetworkCommunicator)peer.Communicator);
                }
            }
        }
    }
}
