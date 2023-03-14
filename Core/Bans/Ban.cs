using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCEAdmin
{
    public class Ban
    {
        public string steamid;
        public string bannerID;
        public string nickname;

        public Ban(string steamid, string bannerID, string nickname)
        {
            this.steamid = steamid;
            this.bannerID = bannerID;
            this.nickname = nickname;
        }
    }
}
