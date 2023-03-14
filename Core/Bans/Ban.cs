using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCEAdmin
{
    public class Ban
    {
        public string gameID;
        public string bannerID;
        public string nickname;

        public Ban(string gameID, string bannerID, string nickname)
        {
            this.gameID = gameID;
            this.bannerID = bannerID;
            this.nickname = nickname;
        }
    }
}
