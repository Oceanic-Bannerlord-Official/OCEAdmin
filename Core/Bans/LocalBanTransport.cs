using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TaleWorlds.Core;
using System.Xml;
using OCEAdmin.Core;
using TaleWorlds.Library;
using OCEAdmin.Commands;

namespace OCEAdmin
{
    public class LocalBanTransport : IBanTransport
    {
        private const string bansFile = "bans.xml";

        private string GetLocalPath()
        {
            if (!Directory.Exists(MPUtil.GetPluginDir()))
            {
                Directory.CreateDirectory(MPUtil.GetPluginDir());
            }

            return Path.Combine(MPUtil.GetPluginDirNoData(), bansFile);
        }

        public void OnAddBan(Ban ban)
        {
            BanManager.AddBan(ban);
            UpdateLocalStorage(BanManager.GetBans());
        }

        public void OnRemoveBan(string id)
        {
            BanManager.RemoveBan(id);
            UpdateLocalStorage(BanManager.GetBans());
        }

        public void Load()
        {
            MPUtil.WriteToConsole("Loading bans using local storage.");

            if (!File.Exists(GetLocalPath()))
            {
                UpdateLocalStorage(new List<Ban>());
            }
            else
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Ban>));

                using (Stream reader = new FileStream(GetLocalPath(), FileMode.Open))
                {
                    try
                    {
                        List<Ban> banList = (List<Ban>)serializer.Deserialize(reader);

                        BanManager.Update(banList);

                        foreach (Ban ban in banList)
                        {
                            MPUtil.WriteToConsole("Adding " + ban.nickname + " to the list of bans.");
                        }
                    } 
                    catch(Exception error) 
                    {
                        MPUtil.WriteToConsole("Attempted to read from bans.xml with error: " + error.Message);
                    }
                }
            }
        }

        private void UpdateLocalStorage(List<Ban> bans)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Ban>));
            Stream fs = new FileStream(GetLocalPath(), FileMode.Create);

            XmlTextWriter writer = new XmlTextWriter(fs, Encoding.Unicode);
            writer.Formatting = System.Xml.Formatting.Indented;
            writer.Indentation = 4;

            serializer.Serialize(writer, bans);
            writer.Close();
        }
    }
}
