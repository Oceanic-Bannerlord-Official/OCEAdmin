using System;
using System.Collections.Generic;
using System.Text;

namespace OCEAdmin
{
    [Handles(typeof(UpdateCompletePacket))]
    class UpdateCompletePacketHandler : IServerPacketHandler
    {
        public void Handle(IServerDataPacket packet)
        {
            MPUtil.WriteToConsole("Checksum check complete! This server is already up-to-date.");
            UpdateManager.Instance.Finish();
        }
    }
}
