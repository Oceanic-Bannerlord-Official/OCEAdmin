using Newtonsoft.Json;
using OCEAdmin.API.Endpoints;
using OCEAdmin.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;
using OCEAdmin.Core;
using OCEAdmin.Commands;
using static TaleWorlds.Library.VirtualFolders.Win64_Shipping_Server;
using TaleWorlds.MountAndBlade;
using System.Threading;
using System.Timers;

namespace OCEAdmin
{
    public class WebBanTransport : IBanTransport
    {
        private string _checksum = "";
        private readonly int _tickTime = 30000; // 30s (Milliseconds)

        public void Load()
        {
            MPUtil.WriteToConsole("Starting bans heartbeat for API.");
            StartHeartbeat();
        }

        public void OnAddBan(Ban ban)
        {
            EndPoint endpoint = new AddBanEndPoint();

            BanManager.AddBan(ban);

            endpoint.SetArgs(new List<Tuple<string, string>> {
                Tuple.Create("key", Config.Get().WebKey),
                Tuple.Create("steam_link", ban.steamid),
                Tuple.Create("reason", "Ingame ban."),
                Tuple.Create("banner", ban.bannerID),
            });

            endpoint.Request();
        }

        public void OnRemoveBan(string id)
        {
            EndPoint endpoint = new DeleteBanEndPoint();
            BanManager.RemoveBan(id);

            endpoint.SetArgs(new List<Tuple<string, string>> {
                Tuple.Create("key", Config.Get().WebKey),
                Tuple.Create("steamid", id),
            });

            endpoint.Request();
        }

        private void GetBans()
        {
            EndPoint endpoint = new GetBansEndPoint();

            endpoint.OnResponseHandler += OnBansReceived;
            endpoint.Request();
        }

        private void OnBansReceived(APIResponse response)
        {
            if (response.data == null)
                return;

            MPUtil.WriteToConsole("Heartbeat: Loading bans from the web API.");

            List<Ban> bans = JsonConvert.DeserializeObject<List<Ban>>(response.data);

            foreach (Ban ban in bans)
            {
                MPUtil.WriteToConsole("Adding " + ban.nickname + " to the list of bans.");
            }

            BanManager.Update(bans);
        }

        private void StartHeartbeat()
        {
            // We want to do an initial heartbeat to pool the bans table.
            MPUtil.WriteToConsole("Listening for bans...");
            OnHeartbeat();

            System.Timers.Timer timer = new System.Timers.Timer(_tickTime);
            timer.Start();
            timer.AutoReset = true;
            timer.Elapsed += OnTick;
        }

        private void OnTick(object sender, ElapsedEventArgs e)
        {
            OnHeartbeat();
        }

        private void OnHeartbeat()
        {
            MPUtil.WriteToConsole("Monitoring web for changes...");
            EndPoint endpoint = new GetBanChecksumEndPoint();

            endpoint.OnResponseHandler += OnHeartbeatChecksumReceived;
            endpoint.Request();
        }

        private void OnHeartbeatChecksumReceived(APIResponse response)
        {
            try
            {
                if (response.data == null)
                    return;

                Heartbeat heartbeat = JsonConvert.DeserializeObject<Heartbeat> (response.data);

                if(heartbeat.checksum != _checksum)
                {
                    MPUtil.WriteToConsole("Bans checksum different to the web panel. Reloading...");

                    GetBans();
                    _checksum = heartbeat.checksum;
                }
                else
                {
                    MPUtil.WriteToConsole("Bans up-to-date.");
                }
            }
            catch (Exception e)
            {
                MPUtil.WriteToConsole("Skipped a web tick. Error: " + e.Message);
            }
        }
    }

    public struct Heartbeat 
    {
        public string checksum;
    }
}
