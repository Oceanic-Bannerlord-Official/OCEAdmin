using Newtonsoft.Json;
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

        public static void AddAdmin(AdminPerms admin)
        {
            _admins.Add(admin);
        }

        public static List<AdminPerms> GetAdmins()
        {
            return _admins;
        }

        public static async Task LoadAdmins()
        {
            if (!Config.Get().UseWebAdmin)
            {
                _admins = Config.Get().Admins;
            }

            WebRequest webRequest = new WebRequest(EndPoint.GetAdmins);

            webRequest.OnResponseHandler += OnAdminsReceived;
            await webRequest.Request();
        }

        public static Task OnAdminsReceived(APIResponse response)
        {
            MPUtil.WriteToConsole("Loading admins from the web API. They will not be loaded from the config.");

            List<AdminData> admins = JsonConvert.DeserializeObject<List<AdminData>>(response.data);

            foreach (AdminData admin in admins)
            {
                AddAdmin(AdminPerms.New(admin.steamid, Role.Admin));
                MPUtil.WriteToConsole($"Adding '{admin.username}' from the web API.");
            }

            return Task.CompletedTask;
        }
    }
}
