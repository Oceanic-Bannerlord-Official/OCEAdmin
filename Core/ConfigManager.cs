using Newtonsoft.Json;
using OCEAdmin.API;
using OCEAdmin.API.Endpoints;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace OCEAdmin
{
    public class ConfigManager
    {
        private const string configFile = "config.xml";

        private static ConfigManager instance;
        public static ConfigManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ConfigManager();
                }
                return instance;
            }
            set { }
        }

        private Config config;

        public void SetConfig(Config config)
        {
            ConfigManager.Instance.config = config;
        }

        public Config GetConfig()
        {
            return ConfigManager.Instance.config;
        }

        public void LoadConfig()
        {
            config = new Config();

            if (!Directory.Exists(MPUtil.GetPluginDir()))
            {
                Directory.CreateDirectory(MPUtil.GetPluginDir());
            }

            string configPath = Path.Combine(MPUtil.GetPluginDirNoData(), configFile);

            if (!File.Exists(configPath))
            {
                config.AdminPassword = MPUtil.RandomString(6);
                config.UseWebAdmin = false;
                config.AllowLoginCommand = true;
                config.Admins = new List<AdminPerms>
                {
                    AdminPerms.New("2.0.0.IDHere", Role.Admin),
                    // Testing:
                    // Adolphus - AdminPerms.New("2.0.0.76561198259745840", Role.Admin),
                    // Muz - AdminPerms.New("2.0.0.76561198026885688", Role.Admin)
                };
                config.SpecialistSettings = new SpecialistSettings()
                {
                    Enabled = true,
                    ArcherLimit = 20,
                    CavLimit = 10
                };
                config.AutoAdminSettings = new AutoAdminSettings()
                {
                    DismountSystemEnabled = true,
                    DismountSlayTime = 10
                };

                XmlSerializer serializer = new XmlSerializer(typeof(Config));
                Stream fs = new FileStream(configPath, FileMode.Create);

                XmlTextWriter writer = new XmlTextWriter(fs, Encoding.Unicode);
                writer.Formatting = System.Xml.Formatting.Indented;
                writer.Indentation = 4;

                serializer.Serialize(writer, config);
                writer.Close();
            }
            else
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Config));

                using (Stream reader = new FileStream(configPath, FileMode.Open))
                {
                    config = (Config)serializer.Deserialize(reader);
                }
            }

            Instance.SetConfig(config);

            // Replace the current admins loaded from the xml with
            // the ones from the OCEAdmin API.
            if(config.UseWebAdmin)
            {
                LoadAdminsFromAPI();
            }
        }

        public void LoadAdminsFromAPI()
        {
            EndPoint endpoint = new GetAdminsEndPoint();

            endpoint.OnResponseHandler += OnAdminsReceived;
            endpoint.Request();
        }

        public void OnAdminsReceived(APIResponse response)
        {
            if (response.data == null)
                return;

            MPUtil.WriteToConsole("Loading admins from the web API. They will not be loaded from the config.");

            List<AdminData> admins = JsonConvert.DeserializeObject<List<AdminData>>(response.data);

            Instance.config.Admins.Clear();

            foreach (AdminData admin in admins)
            {
                Instance.config.Admins.Add(AdminPerms.New(admin.steamid, Role.Admin));
                MPUtil.WriteToConsole($"Adding '{admin.username}' from the web API.");
            }
        }

        private struct AdminData
        {
            public string steamid { get; set; }
            public string username { get; set; }
        }
    }
}
