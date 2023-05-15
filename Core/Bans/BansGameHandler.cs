using OCEAdmin.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade.DedicatedCustomServer;
using TaleWorlds.MountAndBlade.Network.Messages;
using TaleWorlds.MountAndBlade;

namespace OCEAdmin
{
    public class BansGameHandler : GameHandler
    {
        public override void OnAfterSave() { }

        public override void OnBeforeSave() { }

        protected override void OnPlayerConnect(VirtualPlayer virtualPlayer)
        {
            NetworkCommunicator peer = (NetworkCommunicator)virtualPlayer.Communicator;

            if (peer.IsBanned())
            {
                DedicatedCustomServerSubModule.Instance.DedicatedCustomGameServer.KickPlayer(virtualPlayer.Id, false);
            }
        }
    }
}
