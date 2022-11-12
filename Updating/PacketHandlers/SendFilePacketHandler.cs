using System;
using System.Collections.Generic;
using System.Text;

namespace OCEAdmin
{
    [Handles(typeof(SendFilePacket))]
    class SendFilePacketHandler : IServerPacketHandler
    {
        public void Handle(IServerDataPacket packet)
        {
            SendFilePacket sfp = (SendFilePacket)packet;
            MPUtil.WriteToConsole("Receiving file..");
            UpdateManager.Instance.ReceiveFile(sfp);
        }
    }
}
