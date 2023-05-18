using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OCEAdmin
{
    public class Ban
    {
        public string steamid;
        public string bannerID;
        public string nickname;
        public DateTime timeBanned;
        public int minutes = -1;

        public Ban() { }

        public bool IsExpired()
        {
            // -1 indicates a permanent ban.
            if (minutes == -1)
                return false;

            // Legacy bans. Require manual unbanning.
            if (timeBanned == null)
                return false;

            if (DateTime.Now >= GetExpirationTime())
                return true;

            return false;
        }

        public DateTime GetExpirationTime()
        {
            return timeBanned.AddMinutes(minutes);
        }

        public Ban(string steamid, string bannerID, string nickname, int minutes = -1)
        {
            this.steamid = steamid;
            this.bannerID = bannerID;
            this.nickname = nickname;
            this.timeBanned = DateTime.Now;
            this.minutes = minutes;
        }
    }
}
