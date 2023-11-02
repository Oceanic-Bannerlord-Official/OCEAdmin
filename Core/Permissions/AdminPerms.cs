using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCEAdmin.Core.Permissions
{
    public class AdminPerms
    {
        public string PlayerId { get; set; }
        public string PermType { get; set; }

        public static AdminPerms New(string playerId, string permType)
        {
            return new AdminPerms() { PlayerId = playerId, PermType = permType };
        }

        public static AdminPerms New(string playerId, Role permType)
        {
            return new AdminPerms() { PlayerId = playerId, PermType = permType.ToString() };
        }
    }
}
