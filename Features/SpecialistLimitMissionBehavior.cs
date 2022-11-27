using NetworkMessages.FromClient;
using NetworkMessages.FromServer;
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
		private static TeamSpecialistCollection DefenderCollection = new TeamSpecialistCollection();
		private static TeamSpecialistCollection AttackerCollection = new TeamSpecialistCollection();

		public override void AfterStart()
        {
            MPUtil.WriteToConsole("SpecialistLimitMissionBehavior loaded.");
			MissionPeer.OnTeamChanged += this.OnTeamChanged;
		}

        public override void OnRemoveBehavior()
        {
			MissionPeer.OnTeamChanged -= this.OnTeamChanged;

			base.OnRemoveBehavior();
		}

        // Remove a player's contribution to the spec limit on their previous team when swapping.
        public void OnTeamChanged(NetworkCommunicator networkPeer, Team prevTeam, Team newTeam)
        {
			if(networkPeer != null && prevTeam != null && newTeam != null)
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
			//if (AdminPanel.Instance.GetWarmupState() == MultiplayerWarmupComponent.WarmupStates.InProgress)
				//return true;

			// Identify the index to a class type since Bannerlord doesn't do that for us.
			UnitType requestedUnitType = this.GetSpecialistType(networkPeer, message.SelectedTroopIndex);
			Team team = MPUtil.GetPeerTeam(networkPeer);
			TeamSpecialistCollection teamSpecialists = this.GetCollection(team);
			UnitType existingSpecialist = teamSpecialists.GetSpecialist(networkPeer);

			if (requestedUnitType.Equals(UnitType.Infantry)) 
			{
				// If they are swapping to infantry, remove them from any specialist lists.
				if(existingSpecialist != UnitType.Infantry)
                {
					teamSpecialists.Remove(networkPeer);
				}

				return true;
			}

			// If we're trying to swap from the same class type within a class (i.e light
			// cavalry to heavy cavalry), we don't want to do any further checks.
			if (existingSpecialist == requestedUnitType)
				return true;

			int unitCap = teamSpecialists.GetUnitCap(requestedUnitType);
			int unitAmount = teamSpecialists.GetCurrentAmount(requestedUnitType);

			// Specialists for the team are full, inform the player and reject the request.
			if (!this.HasAvaliableSpecs(team, requestedUnitType))
			{
				MPUtil.SendChatMessage(networkPeer, "** SPECIALIST LIMITS ** Your class swap has been rejected.");
				MPUtil.SendChatMessage(networkPeer, string.Format("** SPECIALIST LIMITS ** That class is currently full ({0}/{1}).", unitAmount, unitCap));

				MissionPeer component = networkPeer.GetComponent<MissionPeer>();

				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new UpdateSelectedTroopIndex(networkPeer, component.SelectedTroopIndex));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.ExcludeOtherTeamPlayers, networkPeer);

				return false;
			}

			// Use-case of swapping from cavalry to archer or vise versa,
			// make sure it's all cleared incase.
			teamSpecialists.Remove(networkPeer);

			teamSpecialists.Add(networkPeer, requestedUnitType);
			unitAmount = teamSpecialists.GetCurrentAmount(requestedUnitType);

			MPUtil.SendChatMessage(networkPeer, string.Format("** SPECIALIST LIMITS ** Unit loaded! ({0}/{1}).", unitAmount, unitCap));

			return true;
		}

		// Returns true if a team collection has any avaliable specialists for a unitType.
		private bool HasAvaliableSpecs(Team team, UnitType unitType)
		{
			TeamSpecialistCollection teamSpecialists = this.GetCollection(team);

			if (unitType.Equals(UnitType.Cavalry) && teamSpecialists.Cavalry.Count >= ConfigManager.Instance.GetConfig().SpecialistSettings.CavLimit)
			{
				return false;
			}
			else if (teamSpecialists.Archers.Count >= ConfigManager.Instance.GetConfig().SpecialistSettings.ArcherLimit)
			{
				return false;
			}

			return true;
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

			public TeamSpecialistCollection()
            {
				Cavalry = new List<NetworkCommunicator>();
				Archers = new List<NetworkCommunicator>();
			}

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
				Cavalry.RemoveAll(peer => peer.VirtualPlayer.Id == networkPeer.VirtualPlayer.Id);
				Archers.RemoveAll(peer => peer.VirtualPlayer.Id == networkPeer.VirtualPlayer.Id);
			}

			public int GetUnitCap(UnitType unitType)
            {
				switch (unitType)
				{
					case UnitType.Cavalry:
						return ConfigManager.Instance.GetConfig().SpecialistSettings.CavLimit;
					case UnitType.Archer:
						return ConfigManager.Instance.GetConfig().SpecialistSettings.ArcherLimit;
				}

				return 0;
			}

			public int GetCurrentAmount(UnitType unitType)
            {
				switch (unitType)
				{
					case UnitType.Cavalry:
						return Cavalry.Count;
					case UnitType.Archer:
						return Archers.Count;
				}

				return 0;
			}

			public UnitType GetSpecialist(NetworkCommunicator networkPeer)
            {
				if (Cavalry.Find(peer => peer.VirtualPlayer.Id == networkPeer.VirtualPlayer.Id) != null)
                {
					return UnitType.Cavalry;
                }

				if (Archers.Find(peer => peer.VirtualPlayer.Id == networkPeer.VirtualPlayer.Id) != null)
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
