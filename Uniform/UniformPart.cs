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

        public string GetPart(bool isOfficer)
        {
            if(isOfficer)
            {
                return this.officerVariations;
            }

            return this.variations;
        }
    }
}
