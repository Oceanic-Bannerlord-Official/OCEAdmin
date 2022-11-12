using System;
using System.Collections.Generic;
using System.Text;

namespace OCEAdmin.Updating
{
    class HandlerRegistry
    {
        public static Dictionary<Type, IServerPacketHandler> storage = new Dictionary<Type, IServerPacketHandler>();
    }
}
