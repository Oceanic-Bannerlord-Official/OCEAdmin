using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TaleWorlds.Core;

namespace ChatCommands
{
    public class UniformManager
    {
        public List<ClanUniform> uniforms;

        // Singleton
        private static UniformManager instance;
        public static UniformManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UniformManager();
                }
                return instance;
            }
        }

        public void Populate()
        {
            // Refresh the list if it's already populated.
            this.uniforms = new List<ClanUniform>();

            if (!File.Exists(GetDir()))
            {
                GetTestData().Serialize(GetDir());
            }
            else
            {
                // Read
            }
        }

        public ClanUniform GetTestData()
        {
            ClanUniform clanUniform = new ClanUniform();

            clanUniform.clanTag = "ASTG";
            clanUniform.officerIDs = new List<String> { "2.0.0.76561198259745840" };
            clanUniform.uniformParts = new List<UniformPart>();

            UniformPart headPart = new UniformPart();
            headPart.itemSlot = EquipmentIndex.Head;
            headPart.parts = new List<string> { "mp_nasal_helmet_over_cloth_headwrap" };
            headPart.officerParts = new List<string> { "mp_nasal_helmet_over_cloth_headwrap" };

            UniformPart chestPart = new UniformPart();
            chestPart.itemSlot = EquipmentIndex.Body;
            chestPart.parts = new List<string> { "mp_veteran_mercenary_armor" };
            chestPart.officerParts = new List<string> { "mp_veteran_mercenary_armor" };

            UniformPart cloakPart = new UniformPart();
            cloakPart.itemSlot = EquipmentIndex.Cape;
            cloakPart.parts = new List<string> { "mp_battanian_leather_shoulder_a" };
            cloakPart.officerParts = new List<string> { "mp_battanian_leather_shoulder_a" };

            UniformPart handsPart = new UniformPart();
            handsPart.itemSlot = EquipmentIndex.Gloves;
            handsPart.parts = new List<string> { "mp_leather_gloves" };
            handsPart.officerParts = new List<string> { "mp_leather_gloves" };

            UniformPart legPart = new UniformPart();
            legPart.itemSlot = EquipmentIndex.Leg;
            legPart.parts = new List<string> { "mp_fine_town_boots" };
            legPart.officerParts = new List<string> { "mp_fine_town_boots" };

            clanUniform.uniformParts.Add(headPart);
            clanUniform.uniformParts.Add(chestPart);
            clanUniform.uniformParts.Add(cloakPart);
            clanUniform.uniformParts.Add(handsPart);
            clanUniform.uniformParts.Add(legPart);

            return clanUniform;
        }

        public string GetDir()
        {
            return Path.Combine("../../" + AppDomain.CurrentDomain.BaseDirectory, "clanUniforms.xml");
        }

        public void Add(ClanUniform uniform)
        {
            uniforms.Add(uniform);
        }

        public bool HasClanUniform(string tag)
        {
            foreach (ClanUniform clanUniform in uniforms)
            {
                if (clanUniform.GetClan(tag))
                {
                    return true;
                }
            }

            return false;
        }

        public ClanUniform GetUniformList(string tag)
        {
            foreach (ClanUniform clanUniform in uniforms)
            {
                if (clanUniform.GetClan(tag))
                {
                    return clanUniform;
                }
            }

            return null;
        }
    }
}
