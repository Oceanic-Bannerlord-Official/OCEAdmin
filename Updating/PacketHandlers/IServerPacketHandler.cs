using System;
using System.Collections.Generic;
using System.Text;

namespace OCEAdmin
{
    interface IServerPacketHandler
    {
        void Handle(IServerDataPacket packet);
    }
}
