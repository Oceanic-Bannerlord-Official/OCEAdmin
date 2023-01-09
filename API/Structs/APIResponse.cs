using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMExtended.API
{
    public struct APIResponse
    {
        public int status { get; set; }
        public string message { get; set; }
        public string data { get; set; }
    }
}
