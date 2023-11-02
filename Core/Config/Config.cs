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
        public string WebKey { get; set; }
        public string APIUrl { get; set; }
        public string SteamAPI { get; set; }
        public bool UseWebAdmin { get; set; }
        public bool UseWebBans { get; set; }
        public List<AdminPerms> Admins { get; set; }
        public bool AllowLoginCommand { get; set; }
        public bool GroupfightMode { get; set; }
        
        public SpecialistSettings SpecialistSettings { get; set; }
        public AutoAdminSettings AutoAdminSettings { get; set; }

        public DiscordSettings DiscordSettings { get; set; }

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
            _instance.UseWebAdmin = true;
            _instance.UseWebBans = true;
            _instance.WebKey = "SetMe";
            _instance.APIUrl = "https://admin.bannerlord.au/api/";
            _instance.SteamAPI = null;
            _instance.AllowLoginCommand = true;
            _instance.Admins = new List<AdminPerms>
            {
                AdminPerms.New("2.0.0.76561198259745840", Role.Admin),
                AdminPerms.New("2.0.0.76561198026885688", Role.Admin)
            };
            _instance.SpecialistSettings = new SpecialistSettings()
            {
                Enabled = true,
                ArcherLimit = 20,
                CavLimit = 10
            };
            _instance.AutoAdminSettings = new AutoAdminSettings()
            {
                DismountSystemEnabled = true,
                DismountSlayTime = 10
            };
            _instance.DiscordSettings = new DiscordSettings()
            {
                enabled = false,
                client = "here",
                token = "here"
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

    public class AutoAdminSettings
    {
        public bool DismountSystemEnabled { get; set; }
        public int DismountSlayTime { get; set; }
    }

    public class SpecialistSettings
    {
        public bool Enabled { get; set; }
        public int CavLimit { get; set; }
        public int ArcherLimit { get; set; }
    }

    public class DiscordSettings
    {
        public bool enabled { get; set; }
        public string client { get; set; }
        public string token { get; set; }
    }
}
