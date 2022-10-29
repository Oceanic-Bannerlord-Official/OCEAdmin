﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace OCEAdmin
{
    public class UniformManager
    {
        public List<ClanUniform> uniforms;

        public bool IsLoaded = false;

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

        public bool IsEnabled()
        {
            return false;
        }

        public void Populate()
        {
            // Refresh the list if it's already populated.
            this.uniforms = new List<ClanUniform>();

            string uniformDir = Path.Combine(GetDir(), "uniforms");

            if(!Directory.Exists(uniformDir))
            {
                Directory.CreateDirectory(uniformDir);
                GetTestData().Serialize(uniformDir);
            }
            else
            {
                IEnumerable<string> subdirs = Directory.GetDirectories(uniformDir);

                foreach(string dir in subdirs)
                {
                    string[] files = Directory.GetFiles(dir);

                    foreach(string file in files)
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(ClanUniform));
                        ClanUniform clanUniform = new ClanUniform();

                        using(Stream reader = new FileStream(file, FileMode.Open))
                        {
                            clanUniform = (ClanUniform)serializer.Deserialize(reader);
                        }

                        uniforms.Add(clanUniform);

                        Debug.Print(string.Format("Loading uniform for {0} with unit type: {1}", clanUniform.clanTag, clanUniform.unitOverride), 0, Debug.DebugColor.Green);
                    }
                }
            }

            IsLoaded = true;
        }

        public ClanUniform GetTestData()
        {
            ClanUniform clanUniform = new ClanUniform();

            clanUniform.clanTag = "ASTG";
            clanUniform.unitOverride = "mp_heavy_infantry_vlandia_troop";
            clanUniform.officerIDs = new List<String> { "2.0.0.76561198259745840" };
            clanUniform.uniformParts = new List<UniformPart>();

            UniformPart headPart = new UniformPart();
            headPart.itemSlot = EquipmentIndex.Head;
            headPart.variations = new List<string> { "mp_nasal_helmet_over_cloth_headwrap" };
            headPart.officerVariations = new List<string> { "mp_nasal_helmet_over_cloth_headwrap" };

            UniformPart chestPart = new UniformPart();
            chestPart.itemSlot = EquipmentIndex.Body;
            chestPart.variations = new List<string> { "mp_veteran_mercenary_armor" };
            chestPart.officerVariations = new List<string> { "mp_veteran_mercenary_armor" };

            UniformPart cloakPart = new UniformPart();
            cloakPart.itemSlot = EquipmentIndex.Cape;
            cloakPart.variations = new List<string> { "mp_battanian_leather_shoulder_a" };
            cloakPart.officerVariations = new List<string> { "mp_battanian_leather_shoulder_a" };

            UniformPart handsPart = new UniformPart();
            handsPart.itemSlot = EquipmentIndex.Gloves;
            handsPart.variations = new List<string> { "mp_leather_gloves" };
            handsPart.officerVariations = new List<string> { "mp_leather_gloves" };

            UniformPart legPart = new UniformPart();
            legPart.itemSlot = EquipmentIndex.Leg;
            legPart.variations = new List<string> { "mp_fine_town_boots" };
            legPart.officerVariations = new List<string> { "mp_fine_town_boots" };

            clanUniform.uniformParts.Add(headPart);
            clanUniform.uniformParts.Add(chestPart);
            clanUniform.uniformParts.Add(cloakPart);
            clanUniform.uniformParts.Add(handsPart);
            clanUniform.uniformParts.Add(legPart);

            return clanUniform;
        }

        public string GetDir()
        {
            return "../../OCEAdmin";
        }

        public void Add(ClanUniform uniform)
        {
            uniforms.Add(uniform);
        }

        public bool HasClanUniform(string clan)
        {
            foreach (ClanUniform clanUniform in uniforms)
            {
                if (clanUniform.GetClan(clan))
                {
                    return true;
                }
            }

            return false;
        }

        public bool HasClanUniformForUnit(string clan, string unit)
        {
            foreach (ClanUniform clanUniform in uniforms)
            {
                if (clanUniform.GetClan(clan) && clanUniform.IsForUnit(unit))
                {
                    return true;
                }
            }

            return false;
        }

        public ClanUniform GetUniform(string clan)
        {
            foreach (ClanUniform clanUniform in uniforms)
            {
                if (clanUniform.GetClan(clan))
                {
                    return clanUniform;
                }
            }

            return null;
        }
    }
}