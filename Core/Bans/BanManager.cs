using OCEAdmin.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade;

namespace OCEAdmin.Core
{
    // This is the temporary storage of bans while the server is running.
    // Do not use this to add bans for persistance.
    // Refer to the loaded ban transport for bans.
    public static class BanManager
    {
        public static IBanTransport Handler;

        private static List<Ban> _bans = new List<Ban>();

        public static void Update(List<Ban> bans)
        {
            _bans = bans;
        }

        public static List<Ban> GetBans()
        {
            return _bans;
        }

        public static void AddBan(Ban ban)
        {
            GetBans().Add(ban);
        }

        public static void RemoveBan(string id)
        {
            foreach (Ban ban in GetBans())
            {
                if (ban.steamid == id)
                {
                    GetBans().Remove(ban);

                    break;
                }
            }
        }

        public static bool IsBanned(NetworkCommunicator peer)
        {
            foreach (Ban ban in GetBans())
            {
                if (ban.steamid == peer.VirtualPlayer.Id.Id2.ToString())
                {
                    return true;
                }
            }

            return false;
        }
    }
}
