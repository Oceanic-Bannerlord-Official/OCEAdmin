using System.Collections.Generic;

namespace OCEAdmin.Core
{
    public class Config
    {
        public string AdminPassword { get; set; }
        public List<string> Admins { get; set; }
        public bool AllowLoginCommand { get; set; }
        public UniformSettings UniformSettings { get; set; }
        public AutoAdminSettings AutoAdminSettings { get; set; }
    }

    public class UniformSettings
    {
        public bool Enabled { get; set; }
        public bool UpdateFiles { get; set; }
        public int UpdatePort { get; set; }
    }

    public class AutoAdminSettings
    {
        public bool DismountSystemEnabled { get; set; }
        public int DismountSlayTime { get; set; }
    }

}
