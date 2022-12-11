using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using TaleWorlds.Core;

namespace OCEAdmin
{
    public class ClanUniform
    {
        // List of factions that this uniform can be applied onto.
        public List<string> factions;

        // List of units that this uniform is applicable to.
        // todo: create a system to reject this uniform if a uniform
        // has a unit that is not applicable with the faction list.
        public List<string> units;

        // List of uniform parts.
        public List<UniformPart> uniformParts;

        // Returns if this uniform can be used with the inputted unit.
        public bool IsForUnit(string unit)
        {
            return units.Contains(unit);
        }

        public bool IsForFaction(string fac)
        {
            return factions.Contains(fac);
        }
    }
}
