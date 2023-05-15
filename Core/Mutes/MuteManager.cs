using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TaleWorlds.MountAndBlade;
using System.Xml;
using System.Threading;

namespace OCEAdmin
{
    public static class MuteManager
    {
        private static List<Mute> _mutes = new List<Mute>();

        private const string mutesFile = "mutes.xml";

        private static string GetLocalPath()
        {
            if (!Directory.Exists(MPUtil.GetPluginDir()))
            {
                Directory.CreateDirectory(MPUtil.GetPluginDir());
            }

            return Path.Combine(MPUtil.GetPluginDirNoData(), mutesFile);
        }

        public static List<Mute> GetMutes() { return _mutes; }

        public static void AddMute(Mute mute)
        {
            NetworkCommunicator peer = MPUtil.GetPeerFromID(mute.playerID);

            if (peer == null)
                return;

            // Mutes the player on the current server session.
            peer.GetPlayerExtensionComponent().Mute();

            // Add the player to the permanent mutes.
            _mutes.Add(mute);

            // Update the local storage.
            UpdateMutes(_mutes);
        }

        public static void TempMute(Mute mute)
        {
            NetworkCommunicator peer = MPUtil.GetPeerFromID(mute.playerID);

            if (peer == null)
                return;

            // Mutes the player on the current server session.
            peer.GetPlayerExtensionComponent().Mute();
        }

        public static void RemoveMute(string id) 
        {
            foreach(Mute mute in _mutes)
            {
                if(mute.playerID == id)
                {
                    _mutes.Remove(mute);
                }
            }

            // Update the local storage.
            UpdateMutes(_mutes);

            // Search for the player ID to see if they are on
            // the server.
            NetworkCommunicator peer = MPUtil.GetPeerFromID(id);

            if (peer == null)
                return;

            // Unmute the player for the local session.
            peer.GetPlayerExtensionComponent().Unmute();
        }

        public static void LoadMutes()
        {
            if (!File.Exists(GetLocalPath()))
            {
                UpdateMutes(new List<Mute>());
            }
            else
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Mute>));

                using (Stream reader = new FileStream(GetLocalPath(), FileMode.Open))
                {
                    try
                    {
                        List<Mute> muteList = (List<Mute>)serializer.Deserialize(reader);

                        UpdateMutes(muteList);

                        foreach (Mute peer in muteList)
                        {
                            MPUtil.WriteToConsole("Adding " + peer.nickname + " to the list of mutes.");
                        }
                    }
                    catch (Exception error)
                    {
                        MPUtil.WriteToConsole("Attempted to read from mutes.xml with error: " + error.Message);
                    }
                }
            }
        }

        public static void UpdateMutes(List<Mute> mutes)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Mute>));
            Stream fs = new FileStream(GetLocalPath(), FileMode.Create);

            XmlTextWriter writer = new XmlTextWriter(fs, Encoding.Unicode);
            writer.Formatting = System.Xml.Formatting.Indented;
            writer.Indentation = 4;

            serializer.Serialize(writer, mutes);
            writer.Close();
        }
    }
}
