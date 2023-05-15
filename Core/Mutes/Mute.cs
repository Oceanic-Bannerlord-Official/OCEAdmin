using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCEAdmin
{
    public class Mute
    {
        public string playerID;
        public string adminID;
        public string nickname;

        public Mute() { }

        public Mute(string playerID, string adminID, string nickname)
        {
            this.playerID = playerID;
            this.adminID = adminID;
            this.nickname = nickname;
        }
    }
}
