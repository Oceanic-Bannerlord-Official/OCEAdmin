using System.Collections.Generic;

namespace OCEAdmin.Core
{
    public class Config
    {
        public string AdminPassword { get; set; }
        public List<string> Admins { get; set; }
        public bool AllowLoginCommand { get; set; }
        public SpecialistSettings SpecialistSettings { get; set; }
        public AutoAdminSettings AutoAdminSettings { get; set; }
        public UniformSettings UniformSettings { get; set; }
    }

    public class SpecialistSettings
    {
        public bool Enabled { get; set; }
        public int CavLimit { get; set; }
        public bool UseCavPercentage { get; set; }
        public int ArcherLimit { get; set; }
        public bool UseArcherPercentage { get; set; }
    }

    public class AutoAdminSettings
    {
        public bool DismountSystemEnabled { get; set; }
        public int DismountSlayTime { get; set; }
    }

    public class UniformSettings
    {
        public bool Enabled { get; set; }
        public bool UpdateFiles { get; set; }
        public int UpdatePort { get; set; }
    }

}
