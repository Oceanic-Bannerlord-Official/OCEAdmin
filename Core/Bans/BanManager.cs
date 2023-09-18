using OCEAdmin.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade;

namespace OCEAdmin
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

        public static Task LoadBans()
        {
            if (Config.Get().UseWebBans)
            {
                BanManager.Handler = new WebBanTransport();
            }
            else
            {
                BanManager.Handler = new LocalBanTransport();
            }

            MPUtil.WriteToConsole("Loading bans...");

            Handler.Load();

            // StartBansTick();

            return Task.CompletedTask;
        }


        public static Task StartBansTick()
        {
            Timer timer = new Timer(CheckBanExpirations, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
            return Task.CompletedTask;
        }

        private static void CheckBanExpirations(object state)
        {
            lock(_bans)
            {
                foreach(Ban ban in _bans.ToList())
                {
                    if(ban.IsExpired())
                    {
                        Handler.OnRemoveBan(ban.steamid);
                    }
                }
            }
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
                if (ban.steamid.Contains(peer.VirtualPlayer.Id.Id2.ToString()))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
