using Newtonsoft.Json;
using OCEAdmin.API.Endpoints;
using OCEAdmin.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade;

namespace OCEAdmin.Core.Permissions
{
    public static class AdminManager
    {
        private static List<AdminPerms> _admins = new List<AdminPerms>();

        public static Task AddAdmin(AdminPerms admin)
        {
            _admins.Add(admin);
            return Task.CompletedTask;
        }

        public static List<AdminPerms> GetAdmins()
        {
            return _admins;
        }

        public static Task LoadAdmins()
        {
            if (!Config.Get().UseWebAdmin)
            {
                _admins = Config.Get().Admins;
                return Task.CompletedTask;
            }

            EndPoint endpoint = new GetAdminsEndPoint();

            endpoint.OnResponseHandler += OnAdminsReceived;
            endpoint.Request();

            return Task.CompletedTask;
        }

        public static void OnAdminsReceived(APIResponse response)
        {
            if (response.data == null)
                return;

            MPUtil.WriteToConsole("Loading admins from the web API. They will not be loaded from the config.");

            List<AdminData> admins = JsonConvert.DeserializeObject<List<AdminData>>(response.data);

            Config.Get().Admins.Clear();

            foreach (AdminData admin in admins)
            {
                string bannerlordID = "2.0.0." + admin.steamid;
                Config.Get().Admins.Add(AdminPerms.New(bannerlordID, Role.Admin));
                MPUtil.WriteToConsole($"Adding '{admin.username}' from the web API.");
            }
        }
    }
}
