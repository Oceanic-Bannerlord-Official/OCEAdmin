using System;
using System.Collections.Generic;
using System.IO;
using System.Timers;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCEAdmin.Core.Logging
{
    public static class LogManager
    {
        public static Queue<string> Queue;
        private static Timer saveTimer;
        private static ILogger logger;

        public static Task Add(string msg)
        {
            Queue.Enqueue(msg);

            return Task.CompletedTask;
        }

        public static Task Start()
        {
            Queue = new Queue<string>();
            logger = new LocalLogger();
            saveTimer = new Timer(TimeSpan.FromMinutes(2).TotalMilliseconds);
            saveTimer.Elapsed += logger.Save;
            saveTimer.Start();

            return Task.CompletedTask;
        }
    }
}
