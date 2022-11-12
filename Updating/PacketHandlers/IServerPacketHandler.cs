using System;
using System.Collections.Generic;
using System.Text;

namespace OCEAdmin.Updating
{
    interface IServerPacketHandler
    {
        void Handle(IServerDataPacket packet);
    }
}
