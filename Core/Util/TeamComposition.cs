using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace OCEAdmin.Core
{
    public class TeamComposition
    {
        public Dictionary<CompositionType, List<NetworkCommunicator>> Collection = new Dictionary<CompositionType, List<NetworkCommunicator>>();

        public BasicCultureObject culture;

        public TeamComposition(BasicCultureObject culture)
        {
            this.culture = culture;

            Collection.Add(CompositionType.Infantry, new List<NetworkCommunicator>());
            Collection.Add(CompositionType.Cavalry, new List<NetworkCommunicator>());
            Collection.Add(CompositionType.Archer, new List<NetworkCommunicator>());
        }

        public int GetCompositionAmount(CompositionType compositionType)
        {
            return Collection[compositionType].Count;
        }

        public int GetTotal()
        {
            int total = 0;

            foreach (var compositionList in Collection.Values)
            {
                total += compositionList.Count;
            }

            return total;
        }

        public void AddPlayerToCollection(CompositionType compositionType, NetworkCommunicator peer)
        {
            Collection[compositionType].Add(peer);
        }
    }

    public enum CompositionType
    {
        Infantry,
        Cavalry,
        Archer
    }
}
