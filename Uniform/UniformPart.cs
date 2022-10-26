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
        public List<string> parts;

        // These are the possible variations for those a part of
        // the clan with the officer ID.
        public List<string> officerParts;

        public List<string> GetParts(bool isOfficer)
        {
            if(isOfficer)
            {
                return this.officerParts;
            }

            return this.parts;
        }
    }
}
