using NetworkMessages.FromClient;
using OCEAdmin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;
using static TaleWorlds.MountAndBlade.MultiplayerClassDivisions;

namespace OCEAdmin.Features
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
			if(Mission.Current.GetMissionBehavior<SpecialistLimitMissionBehavior>() == null)
            {
				MPUtil.WriteToConsole("SpecialistLimitMissionBehavior is null");
            }

			MPUtil.WriteToConsole("SpecialistLimitMissionBehavior Exists");

			return Mission.Current.GetMissionBehavior<SpecialistLimitMissionBehavior>().RequestTroopChange(peer, message);
		}

		protected override void OnPlayerDisconnect(VirtualPlayer peer)
		{
			SpecialistLimitMissionBehavior mb = Mission.Current.GetMissionBehavior<SpecialistLimitMissionBehavior>();

			if(mb != null)
            {
				mb.OnPlayerDisconnect((NetworkCommunicator)peer.Communicator);
			}
		}
	}
}
