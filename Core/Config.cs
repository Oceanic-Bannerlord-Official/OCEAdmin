using System.Collections.Generic;

namespace OCEAdmin.Core
{
    public class Config
    {
        public string AdminPassword { get; set; }
        public List<string> Admins { get; set; }
        public bool AllowLoginCommand { get; set; }
        public UniformSettings UniformSettings { get; set; }
    }

    public class UniformSettings
    {
        public bool Enabled { get; set; }
        public bool UpdateFiles { get; set; }
        public int UpdatePort { get; set; }
    }

}
