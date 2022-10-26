using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;

namespace ChatCommands
{
    public class UniformPart
    {
        public EquipmentIndex itemSlot;

        // These are the possible variations for standard enlisted
        // of the clan.
        public List<string> variations;

        // These are the possible variations for those a part of
        // the clan with the officer ID.
        public List<string> officerVariations;

        // Returns a random variation from the officer or enlisted category.
        public string GetPart(bool isOfficer)
        {
            if(isOfficer && officerVariations.Count > 0)
            {
                return officerVariations[new Random().Next(0, this.officerVariations.Count - 1)];
            }

            return variations[new Random().Next(0, this.variations.Count - 1)];
        }
    }
}
