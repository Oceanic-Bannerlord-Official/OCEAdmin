using System.Collections.Generic;

namespace OCEAdmin.Core
{
    public class Config
    {
        public string AdminPassword { get; set; }
        public List<string> Admins { get; set; }
        public bool AllowLoginCommand { get; set; }
        public AutoAdminSettings AutoAdminSettings { get; set; }
    }

    public class AutoAdminSettings
    {
        public bool DismountSystemEnabled { get; set; }
        public int DismountSlayTime { get; set; }
    }
}
