using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCEAdmin
{
    public class Clan
    {
        public string tag;

        // We use this to assign the officer uniform to these players.
        public List<string> officerIDs = new List<string>();

        public List<ClanUniform> uniforms = new List<ClanUniform>();

        public bool HasUniform()
        {
            return (uniforms.Count > 0);
        }

        public ClanUniform GetUniformForUnit(string unit)
        {
            foreach (ClanUniform clanUniform in uniforms)
            {
                if (clanUniform.IsForUnit(unit))
                {
                    return clanUniform;
                }
            }

            return null;
        }
    }
}
