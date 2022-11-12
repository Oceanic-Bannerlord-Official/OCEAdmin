using System;
using System.Collections.Generic;
using System.Text;

namespace OCEAdmin
{
    class HandlerRegistry
    {
        public static Dictionary<Type, IServerPacketHandler> storage = new Dictionary<Type, IServerPacketHandler>();
    }
}
