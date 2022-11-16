using NetworkMessages.FromClient;
using OCEAdmin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace OCEAdmin.Features
{
    public class SpecialistLimitMissionBehavior : MissionLogic
    {
        public override MissionBehaviorType BehaviorType => MissionBehaviorType.Logic;

		// These collections store the counts of each teams' specialists.
		private TeamSpecialistCollection DefenderCollection = new TeamSpecialistCollection();
		private TeamSpecialistCollection AttackerCollection = new TeamSpecialistCollection();

		public override void AfterStart()
        {
            MPUtil.WriteToConsole("SpecialistLimitMissionBehavior loaded.");
        }

		// Remove a player's contribution to the spec limit on their previous team when swapping.
        public override void OnAgentTeamChanged(Team prevTeam, Team newTeam, Agent agent)
        {
			NetworkCommunicator networkPeer = agent.MissionPeer.GetNetworkPeer();

			if(networkPeer != null)
            {
				this.GetCollection(prevTeam).Remove(networkPeer);
			}
        }

		// Called from the GameHandler reroute of when a player has disconnected from the server.
		public void OnPlayerDisconnect(NetworkCommunicator networkPeer)
        {
			Team team = MPUtil.GetPeerTeam(networkPeer);
			this.GetCollection(team).Remove(networkPeer);
		}

		// Called from the GameHandler reroute of when a player is attempting to select a class.
        public bool RequestTroopChange(NetworkCommunicator networkPeer, RequestTroopIndexChange message)
        {
			if (!ConfigManager.Instance.GetConfig().SpecialistSettings.Enabled)
				return true;

			// Don't want specialist limits in warmup.
			if (AdminPanel.Instance.GetWarmupState() == MultiplayerWarmupComponent.WarmupStates.InProgress)
				return true;

			// Identify the index to a class type since Bannerlord doesn't do that for us.
			UnitType requestedUnitType = this.GetSpecialistType(networkPeer, message.SelectedTroopIndex);

			if (requestedUnitType.Equals(UnitType.Infantry))
				return true;

			Team team = MPUtil.GetPeerTeam(networkPeer);
			TeamSpecialistCollection teamSpecialists = this.GetCollection(team);

			UnitType existingSpecialist = teamSpecialists.GetSpecialist(networkPeer);

			// If we're trying to swap from the same class type within a class (i.e light
			// cavalry to heavy cavalry), we don't want to do any further checks.
			if (existingSpecialist == requestedUnitType)
				return true;

			// Specialists for the team are full, inform the player and reject the request.
			if (!this.HasAvaliableSpecs(team, requestedUnitType))
			{
				MPUtil.SendChatMessage(networkPeer, "** SPECIALIST LIMITS ** Your class swap has been rejected.");
				MPUtil.SendChatMessage(networkPeer, "** SPECIALIST LIMITS ** That class is currently full.");

				// todo: maybe send a packet to update the client interface if possible

				return false;
			}

			// Use-case of swapping from cavalry to archer or vise versa,
			// make sure it's all cleared incase.
			teamSpecialists.Remove(networkPeer);

			teamSpecialists.Add(networkPeer, requestedUnitType);

			return true;
		}

		// Returns true if a team collection has any avaliable specialists for a unitType.
		private bool HasAvaliableSpecs(Team team, UnitType unitType)
		{
			TeamSpecialistCollection teamSpecialists = this.GetCollection(team);

			if (unitType.Equals(UnitType.Cavalry) && teamSpecialists.Cavalry.Count < ConfigManager.Instance.GetConfig().SpecialistSettings.CavLimit)
			{
				return true;
			}
			else if (teamSpecialists.Archers.Count < ConfigManager.Instance.GetConfig().SpecialistSettings.ArcherLimit)
			{
				return true;
			}

			return false;
		}

		// Returns the specialist type for a unit's index based on an icon that
		// is set in the multiplayerclassdivisions.xml
		private UnitType GetSpecialistType(NetworkCommunicator peer, int index)
		{
			BasicCultureObject culture = MPUtil.GetTeamCulture(peer);

			List<MultiplayerClassDivisions.MPHeroClass> classes = MultiplayerClassDivisions.GetMPHeroClasses(culture).ToList<MultiplayerClassDivisions.MPHeroClass>();

			for (int i = 0; i < classes.Count; i++)
			{
				if (index == i)
				{
					var curClass = classes[i];

					if (MPUtil.cavClasses.Contains(curClass.IconType))
					{
						return UnitType.Cavalry;
					}
					else if (MPUtil.archerClasses.Contains(curClass.IconType))
					{
						return UnitType.Archer;
					}
				}
			}

			return UnitType.Infantry;
		}

		// Returns the team collection for specialists given a team.
		private TeamSpecialistCollection GetCollection(Team team)
		{
			TeamSpecialistCollection teamSpecialists = DefenderCollection;

			if (team.IsAttacker)
			{
				teamSpecialists = AttackerCollection;
			}

			return teamSpecialists;
		}

		// The data set that stores the specialists for each team.
		private class TeamSpecialistCollection
		{
			public List<NetworkCommunicator> Cavalry { get; }
			public List<NetworkCommunicator> Archers { get; }

			public void Add(NetworkCommunicator networkPeer, UnitType unitType)
			{
				switch (unitType)
				{
					case UnitType.Cavalry:
						Cavalry.Add(networkPeer);
						break;
					case UnitType.Archer:
						Archers.Add(networkPeer);
						break;
				}
			}

			public void Remove(NetworkCommunicator networkPeer)
			{
				Cavalry.Remove(networkPeer);
				Archers.Remove(networkPeer);
			}

			public UnitType GetSpecialist(NetworkCommunicator networkPeer)
            {
				if(Cavalry.Find(peer => peer.VirtualPlayer.Id == networkPeer.VirtualPlayer.Id) != null)
                {
					return UnitType.Cavalry;
                }

				if(Archers.Find(peer => peer.VirtualPlayer.Id == networkPeer.VirtualPlayer.Id) != null)
                {
					return UnitType.Archer;
                }

				return UnitType.Infantry;
            }
		}

		private enum UnitType
		{
			Infantry,
			Cavalry,
			Archer
		}
	}
}
