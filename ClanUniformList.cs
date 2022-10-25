using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;

namespace ChatCommands
{

    public class UniformManager
    {
        public List<ClanUniform> uniforms;

        public UniformManager()
        {
            this.uniforms = new List<ClanUniform>();
        }
        public void Add(ClanUniform uniform)
        {
            uniforms.Add(uniform);
        }

        public bool HasClanUniform(string tag)
        {
            foreach (ClanUniform clanUniform in uniforms)
            {
                if (clanUniform.clanTag.Contains(tag))
                {
                    return true;
                }
            }

            return false;
        }

        public ClanUniform GetUniformList(string tag)
        {
            ClanUniform temp = null;

            foreach(ClanUniform clanUniform in uniforms)
            {
                if(clanUniform.clanTag.Contains(tag))
                {
                    temp = clanUniform;
                }
            }

            return temp;
        }
    }

    public class UniformPart
    {
        public EquipmentIndex itemSlot;

        // These are the possible variations for standard enlisted
        // of the clan.
        public List<string> parts;

        // These are the possible variations for those a part of
        // the clan with the officer ID.
        public List<string> officerParts;

        public UniformPart(EquipmentIndex itemSlot, List<string> parts, List<string> officerParts)
        {
            this.itemSlot = itemSlot;
            this.parts = parts;
            this.officerParts = officerParts;
        }
    }

    public class ClanUniform
    {
        // The ingame clan tag
        public string clanTag;

        // We use this to assign the officer uniform to these players.
        public List<String> officerIDs;

        // List of uniform parts.
        public List<UniformPart> uniformParts;
    }
}
