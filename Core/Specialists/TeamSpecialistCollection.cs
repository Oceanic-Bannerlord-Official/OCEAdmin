using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade;

namespace OCEAdmin
{
    // The data set that stores the specialists for each team.
    public class TeamSpecialistCollection
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
            if (networkPeer.VirtualPlayer == null)
                return;

            if (networkPeer.VirtualPlayer.Id == null)
                return;

            Cavalry.RemoveAll(peer => peer.VirtualPlayer.Id == networkPeer.VirtualPlayer.Id);
            Archers.RemoveAll(peer => peer.VirtualPlayer.Id == networkPeer.VirtualPlayer.Id);
        }

        public int GetUnitCap(UnitType unitType)
        {
            switch (unitType)
            {
                case UnitType.Cavalry:
                    return Config.Get().SpecialistSettings.CavLimit;
                case UnitType.Archer:
                    return Config.Get().SpecialistSettings.ArcherLimit;
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

    public enum UnitType
    {
        Infantry,
        Cavalry,
        Archer
    }
}
