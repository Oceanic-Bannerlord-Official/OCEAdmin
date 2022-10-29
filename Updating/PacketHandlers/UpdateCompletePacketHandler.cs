using System;
using System.Collections.Generic;
using System.Text;

namespace OCEAdmin.Updating
{
    [Handles(typeof(UpdateCompletePacket))]
    class UpdateCompletePacketHandler : IServerPacketHandler
    {
        public void Handle(IServerDataPacket packet)
        {
            UpdateManager.Instance.Finish();
        }
    }
}
