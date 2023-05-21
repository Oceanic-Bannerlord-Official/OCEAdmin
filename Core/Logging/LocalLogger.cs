using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace OCEAdmin.Core.Logging
{
    public class LocalLogger : ILogger
    {
        public string logFileName { get; set; }

        public void Save(object sender, ElapsedEventArgs e)
        {
            if (!Directory.Exists(GetLogPath()))
            {
                Directory.CreateDirectory(GetLogPath());
            }

            lock (LogManager.Queue)
            {
                if (LogManager.Queue.Count > 0)
                {
                    using (var writer = File.AppendText(logFileName))
                    {
                        while (LogManager.Queue.Count > 0)
                        {
                            var logString = LogManager.Queue.Dequeue();
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
