using System;
using System.Collections.Generic;
using System.Text;

namespace OCEAdmin
{
    class NetworkPacketRegistry
    {
        public static Dictionary<int, Type> storage = new Dictionary<int, Type>();
    }
}
