using System;
using System.Collections.Generic;
using System.Text;

namespace OCEAdmin.Updating
{
    [Handles(typeof(SendFilePacket))]
    class SendFilePacketHandler : IServerPacketHandler
    {
        public void Handle(IServerDataPacket packet)
        {
            SendFilePacket sfp = (SendFilePacket)packet;

            UpdateManager.Instance.ReceiveFile(sfp);
        }
    }
}
