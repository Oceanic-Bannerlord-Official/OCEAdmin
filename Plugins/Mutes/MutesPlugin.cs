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
using OCEAdmin.Core.Plugin;
using OCEAdmin.Plugins.Commands;
using HarmonyLib;
using OCEAdmin.Plugins.Logging;
using System.Reflection;

namespace OCEAdmin.Plugins.Mutes
{
    public class MutesPlugin : PluginBase
    {
        public override string Name => "Mutes";
        public override string Description => "Allows for the muting of players.";
        public override bool IsCore => true;

        private List<Mute> _mutes = new List<Mute>();

        private const string mutesFile = "mutes.xml";

        public override async Task Load()
        {
            await base.Load();

            CommandsPlugin commands = OCEAdminSubModule.GetPlugin<CommandsPlugin>();

            if (commands != null)
            {
                await commands.Register(new MuteCommand());
                await commands.Register(new TempMute());
                await commands.Register(new Unmute());
            }
        }

        public override void OnPatch(Harmony harmony)
        {
            base.OnPatch(harmony);

            var original = typeof(ChatBox).GetMethod("ServerPrepareAndSendMessage", BindingFlags.NonPublic | BindingFlags.Instance);
            var prefix = typeof(PatchChatBox).GetMethod("Prefix");
            harmony.Patch(original, prefix: new HarmonyMethod(prefix));
        }

        private string GetLocalPath()
        {
            if (!Directory.Exists(MPUtil.GetPluginDir()))
            {
                Directory.CreateDirectory(MPUtil.GetPluginDir());
            }

            return Path.Combine(MPUtil.GetPluginDirNoData(), mutesFile);
        }

        public List<Mute> GetMutes() { return _mutes; }

        public void AddMute(Mute mute)
        {
            NetworkCommunicator peer = MPUtil.GetPeerFromID(mute.playerID);

            if (peer == null)
                return;

            // Mutes the player on the current server session.
            peer.GetPlayer().Mute();

            // Add the player to the permanent mutes.
            _mutes.Add(mute);

            // Update the local storage.
            UpdateMutes(_mutes);
        }

        public void TempMute(Mute mute)
        {
            NetworkCommunicator peer = MPUtil.GetPeerFromID(mute.playerID);

            if (peer == null)
                return;

            // Mutes the player on the current server session.
            peer.GetPlayer().Mute();
        }

        public void RemoveMute(string id) 
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
            peer.GetPlayer().Unmute();
        }

        public Task LoadMutes()
        {
            MPUtil.WriteToConsole("Loading player mutes...");

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

                        _mutes = muteList;

                        foreach (Mute peer in muteList)
                        {
                            MPUtil.WriteToConsole("Adding " + peer.nickname + " to the list of mutes.");
                        }
                    }
                    catch (Exception error)
                    {
                        MPUtil.WriteToConsole("Attempted to read from mutes.xml with error: " + error.Message, true);
                    }
                }
            }

            return Task.CompletedTask;
        }

        public void UpdateMutes(List<Mute> mutes)
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
