using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace OCEAdmin.Core
{
    public static class ConfigManager
    {
        private static Config _config;
        public static Config Instance
        {
            get
            {
                if (_config == null)
                {
                    _config = new Config();
                }
                return _config;
            }
        }
        private const string configFile = "config.xml";

        public static void LoadConfig()
        {
            if (!Directory.Exists(MPUtil.GetPluginDir()))
            {
                Directory.CreateDirectory(MPUtil.GetPluginDir());
            }

            string configPath = Path.Combine(MPUtil.GetPluginDir(), configFile);

            if (!File.Exists(configPath))
            {
                _config.AdminPassword = MPUtil.RandomString(6);
                _config.Admins = new List<string>();
                _config.Admins.Add("2.0.0.AdminIDHere");
                _config.Admins.Add("2.0.0.AdminIDHere");
                _config.AllowLoginCommand = true;
                _config.UniformSettings = new UniformSettings()
                {
                    Enabled = false,
                    UpdateFiles = false
                };

                XmlSerializer serializer = new XmlSerializer(typeof(Config));
                Stream fs = new FileStream(configPath, FileMode.Create);

                XmlTextWriter writer = new XmlTextWriter(fs, Encoding.Unicode);
                writer.Formatting = Formatting.Indented;
                writer.Indentation = 4;

                serializer.Serialize(writer, _config);
                writer.Close();
            }
            else
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Config));
                Config _config = new Config();

                using (Stream reader = new FileStream(configPath, FileMode.Open))
                {
                    _config = (Config)serializer.Deserialize(reader);
                }
            }
        }
    }
}
