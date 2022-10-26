using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using TaleWorlds.Core;

namespace ChatCommands
{
    public class ClanUniform
    {
        // The ingame clan tag
        public string clanTag;

        // We use this to assign the officer uniform to these players.
        public List<String> officerIDs;

        public string unitOverride;

        // List of uniform parts.
        public List<UniformPart> uniformParts;

        public void Serialize(string fileName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ClanUniform));
            Stream fs = new FileStream(fileName, FileMode.Create);

            XmlTextWriter writer = new XmlTextWriter(fs, Encoding.Unicode);
            writer.Formatting = Formatting.Indented;
            writer.Indentation = 4;

            serializer.Serialize(writer, this);
            writer.Close();
        }

        public bool GetClan(string tag)
        {
            if(clanTag.Contains(tag))
            {
                return true;
            }

            return false;
        }
    }
}
