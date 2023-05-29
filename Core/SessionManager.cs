using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCEAdmin.Core
{
    public static class SessionManager
    {
        public static bool GroupfightMode = false;

        public static SpecialistSettings SpecialistSettings = new SpecialistSettings();

        public static AutoAdminSettings AutoAdminSettings = new AutoAdminSettings();

        public static async Task UpdateFromConfig()
        {
            GroupfightMode = Config.Get().GroupfightMode;
            SpecialistSettings = Config.Get().SpecialistSettings;
            AutoAdminSettings = Config.Get().AutoAdminSettings;
        }
    }
}
