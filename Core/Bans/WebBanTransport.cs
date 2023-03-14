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

namespace OCEAdmin
{
    public class WebBanTransport : IBanTransport
    {
        public void LoadList()
        {
            EndPoint endpoint = new GetBansEndPoint();

            endpoint.OnResponseHandler += OnBansReceived;
            endpoint.Request();
        }

        public void OnAddBan(Ban ban)
        {
            EndPoint endpoint = new AddBanEndPoint();

            BanManager.AddBan(ban);

            endpoint.SetArgs(new List<Tuple<string, string>> {
                Tuple.Create("key", ConfigManager.Instance.GetConfig().WebKey),
                Tuple.Create("steam_link", ban.gameID),
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
                Tuple.Create("key", ConfigManager.Instance.GetConfig().WebKey),
                Tuple.Create("steamid", id),
            });

            endpoint.Request();
        }

        public void OnBansReceived(APIResponse response)
        {
            if (response.data == null)
                return;

            MPUtil.WriteToConsole("Loading bans from the web API. They will not be loaded from the config.");

            List<Ban> bans = JsonConvert.DeserializeObject<List<Ban>>(response.data);

            BanManager.Update(bans);
        }
    }
}
