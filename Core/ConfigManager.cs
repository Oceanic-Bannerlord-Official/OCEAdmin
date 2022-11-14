using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace OCEAdmin.Core
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

            string configPath = Path.Combine(MPUtil.GetPluginDir(), configFile);

            if (!File.Exists(configPath))
            {
                config.AdminPassword = MPUtil.RandomString(6);
                config.Admins = new List<string>();
                config.Admins.Add("2.0.0.AdminIDHere");
                config.Admins.Add("2.0.0.AdminIDHere");
                config.AllowLoginCommand = true;
                config.UniformSettings = new UniformSettings()
                {
                    Enabled = false,
                    UpdateFiles = false,
                    UpdatePort = 8400
                };

                XmlSerializer serializer = new XmlSerializer(typeof(Config));
                Stream fs = new FileStream(configPath, FileMode.Create);

                XmlTextWriter writer = new XmlTextWriter(fs, Encoding.Unicode);
                writer.Formatting = Formatting.Indented;
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
        }
    }
}
