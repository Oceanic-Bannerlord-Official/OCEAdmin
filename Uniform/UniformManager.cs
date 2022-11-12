using System;
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
        public List<Clan> clans;

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

        public void LoadClans()
        {
            this.clans = new List<Clan>();

            string uniformDir = Path.Combine(MPUtil.GetPluginDir(), "uniforms");

            Directory.CreateDirectory(uniformDir);
            IEnumerable<string> subdirs = Directory.GetDirectories(uniformDir);

            foreach (string dir in subdirs)
            {
                string[] files = Directory.GetFiles(dir);

                foreach (string file in files)
                {
                    string clanName = new DirectoryInfo(dir).Name + ".xml";
                    string fileName = file.Replace("/", @"\").Replace(Path.GetDirectoryName(file) + @"\", "");

                    if (fileName == clanName)
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(Clan));
                        Clan clan = new Clan();

                        using (Stream reader = new FileStream(file, FileMode.Open))
                        {
                            clan = (Clan)serializer.Deserialize(reader);
                        }

                        clans.Add(clan);

                        MPUtil.WriteToConsole(string.Format("Clan '{0}' has been loaded!", clan.tag));
                    }
                }
            }
        }

        public void LoadUniforms()
        {
            string uniformDir = Path.Combine(MPUtil.GetPluginDir(), "uniforms");

            IEnumerable<string> subdirs = Directory.GetDirectories(uniformDir);

            foreach(string dir in subdirs)
            {
                string[] files = Directory.GetFiles(dir);

                foreach(string file in files)
                {
                    string clanName = new DirectoryInfo(dir).Name;
                    string fileName = file.Replace("/", @"\").Replace(Path.GetDirectoryName(file) + @"\", "");

                    if (fileName != clanName + ".xml")
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(ClanUniform));
                        ClanUniform clanUniform = new ClanUniform();

                        using (Stream reader = new FileStream(file, FileMode.Open))
                        {
                            clanUniform = (ClanUniform)serializer.Deserialize(reader);
                        }

                        bool foundClan = false;

                        foreach(Clan clan in clans)
                        {
                            if(clan.tag == clanName)
                            {
                                clan.uniforms.Add(clanUniform);
                                foundClan = true;
                            }
                        }

                        if(!foundClan)
                        {
                            MPUtil.WriteToConsole(string.Format("Could not load uniform file: {0}. This is because you are missing {1}.xml in the uniform's folder.",
                                file, clanName));
                        }
                        else
                        {
                            MPUtil.WriteToConsole(string.Format("Loading uniform '{0}' for {1}.", fileName, clanName));
                        }
                    }
                }
            }
        }
    
        public Clan GetClan(string clanTag)
        {
            foreach(Clan clan in clans)
            {
                if(clan.tag == clanTag)
                {
                    return clan;
                }
            }

            return null;
        }
    }
}
