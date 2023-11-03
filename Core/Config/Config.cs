using OCEAdmin.Core.Permissions;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;


namespace OCEAdmin
{
    public class Config
    {
        private const string configFile = "config.xml";

        private static Config _instance;
        public string AdminPassword { get; set; }
        public List<AdminPerms> Admins { get; set; }
        public bool AllowLoginCommand { get; set; }
        public bool GroupfightMode { get; set; }

        public static Config Get()
        {
            return _instance;
        }

        public static async Task Load()
        {
            await LoadDefaults();

            if (!File.Exists(GetConfigPath()))
            {
                await LoadDefaults();

                MPUtil.WriteToConsole("No config file found. Conducting first time setup. Loading defaults.");

                // Store the temporary variables to the config location for user setup.
                await Serialize();
            }
            else
            {
                await Deserialize();
            }
        }

        public static Task LoadDefaults()
        {
            _instance = new Config();
            _instance.AdminPassword = MPUtil.RandomString(6);
            _instance.AllowLoginCommand = true;
            _instance.Admins = new List<AdminPerms>
            {
                AdminPerms.New("2.0.0.76561198259745840", Role.Admin),
                AdminPerms.New("2.0.0.76561198026885688", Role.Admin)
            };

            return Task.CompletedTask;
        }

        public static Task Serialize()
        {
            if (!Directory.Exists(MPUtil.GetPluginDir()))
            {
                Directory.CreateDirectory(MPUtil.GetPluginDir());
            }

            XmlSerializer serializer = new XmlSerializer(typeof(Config));
            Stream fs = new FileStream(GetConfigPath(), FileMode.Create);

            XmlTextWriter writer = new XmlTextWriter(fs, Encoding.Unicode);
            writer.Formatting = System.Xml.Formatting.Indented;
            writer.Indentation = 4;

            serializer.Serialize(writer, _instance);
            writer.Close();

            return Task.CompletedTask;
        }

        public static Task Deserialize()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Config));

            using (Stream reader = new FileStream(GetConfigPath(), FileMode.Open))
            {
                _instance = (Config)serializer.Deserialize(reader);
            }

            return Task.CompletedTask;
        }

        public static string GetConfigPath()
        {
            return Path.Combine(MPUtil.GetPluginDirNoData(), configFile);
        }
    }
}
