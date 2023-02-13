using OCEAdmin.Commands;
using System.Collections.Generic;

namespace OCEAdmin
{
    public class Config
    {
        public string AdminPassword { get; set; }

        public bool UseWebAdmin { get; set; }
        public List<AdminPerms> Admins { get; set; }
        public bool AllowLoginCommand { get; set; }
        public SpecialistSettings SpecialistSettings { get; set; }
        public AutoAdminSettings AutoAdminSettings { get; set; }
    }

    public class AdminPerms
    {
        public string PlayerId { get; set; }
        public string PermType { get; set; }

        public static AdminPerms New(string playerId, string permType)
        {
            return new AdminPerms() { PlayerId = playerId, PermType = permType };
        }

        public static AdminPerms New(string playerId, Role permType)
        {
            return new AdminPerms() { PlayerId = playerId, PermType = permType.ToString() };
        }
    }

    public class AutoAdminSettings
    {
        public bool DismountSystemEnabled { get; set; }
        public int DismountSlayTime { get; set; }
    }

    public class SpecialistSettings
    {
        public bool Enabled { get; set; }
        public int CavLimit { get; set; }
        public int ArcherLimit { get; set; }
    }
}
