using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCEAdmin.API
{
    public class APIResponse
    {
        public int status { get; set; }
        public string status_message { get; set; }
        public string data { get; set; }
    }
}
