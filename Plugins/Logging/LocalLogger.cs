using OCEAdmin.Plugins.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace OCEAdmin.Plugins.Logging
{
    public class LocalLogger : ILogger
    {
        LoggingPlugin plugin;

        public string logFileName = GetLogFileName();

        public LocalLogger(LoggingPlugin plugin)
        {
            this.plugin = plugin;
        }

        public void Save(object sender, ElapsedEventArgs e)
        {
            if (!Directory.Exists(GetLogPath()))
            {
                Directory.CreateDirectory(GetLogPath());
            }

            lock (plugin.Queue)
            {
                if (plugin.Queue.Count > 0)
                {
                    using (var writer = File.AppendText(logFileName))
                    {
                        while (plugin.Queue.Count > 0)
                        {
                            var logString = plugin.Queue.Dequeue();
                            writer.WriteLine(logString);
                        }
                    }
                }
            }

            // Generate new log file name if the date has changed
            string newLogFileName = GetLogFileName();
            if (newLogFileName != logFileName)
            {
                logFileName = newLogFileName;
            }
        }

        private static string GetLogFileName()
        {
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            return GetLogPath() + "/" + date + ".txt";
        }

        public static string GetLogPath()
        {
            return Path.Combine(MPUtil.GetPluginDirNoData(), "logs");
        }
    }
}
