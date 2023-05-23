using Newtonsoft.Json;
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

        public async Task Load()
        {
            MPUtil.WriteToConsole("Starting bans heartbeat for API.");

            await StartHeartbeat();
        }

        public async Task OnAddBan(Ban ban)
        {
            WebRequest webRequest = new WebRequest(EndPoint.AddBan);
            BanManager.AddBan(ban);

            webRequest.SetArgs(new List<Tuple<string, string>> {
                Tuple.Create("key", Config.Get().WebKey),
                Tuple.Create("steam_link", ban.steamid),
                Tuple.Create("reason", "Ingame ban."),
                Tuple.Create("banner", ban.bannerID),
            });

            // Don't need a response from it.
            await webRequest.Request();
        }

        public async Task OnRemoveBan(string id)
        {
            WebRequest webRequest = new WebRequest(EndPoint.DeleteBan);
            BanManager.RemoveBan(id);

            webRequest.SetArgs(new List<Tuple<string, string>> {
                Tuple.Create("key", Config.Get().WebKey),
                Tuple.Create("steamid", id),
            });

            // Don't need a response from it.
            await webRequest.Request();
        }

        private async Task GetBans()
        {
            WebRequest webRequest = new WebRequest(EndPoint.GetBans);

            webRequest.OnResponseHandler += OnBansReceived;
            await webRequest.Request();
        }

        private Task OnBansReceived(APIResponse response)
        {
            MPUtil.WriteToConsole("Heartbeat: Loading bans from the web API.");

            List<Ban> bans = JsonConvert.DeserializeObject<List<Ban>>(response.data);

            foreach (Ban ban in bans)
            {
                MPUtil.WriteToConsole("Adding " + ban.nickname + " to the list of bans.");
            }

            BanManager.Update(bans);

            return Task.CompletedTask;
        }

        private async Task StartHeartbeat()
        {
            // We want to do an initial heartbeat to pool the bans table.
            MPUtil.WriteToConsole("Listening for bans...");

            await OnHeartbeat();

            System.Timers.Timer timer = new System.Timers.Timer(_tickTime);
            timer.Start();
            timer.AutoReset = true;
            timer.Elapsed += OnTick;
        }

        private async void OnTick(object sender, ElapsedEventArgs e)
        {
            await OnHeartbeat();
        }

        private async Task OnHeartbeat()
        {
            MPUtil.WriteToConsole("Monitoring web for changes...");
            WebRequest webRequest = new WebRequest(EndPoint.GetBanChecksum);

            webRequest.OnResponseHandler += OnHeartbeatChecksumReceived;
            await webRequest.Request();
        }

        private async Task OnHeartbeatChecksumReceived(APIResponse response)
        {
            try
            {
                Heartbeat heartbeat = JsonConvert.DeserializeObject<Heartbeat> (response.data);

                if(heartbeat.checksum != _checksum)
                {
                    MPUtil.WriteToConsole("Bans checksum different to the web panel. Reloading...");

                    await GetBans();
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
