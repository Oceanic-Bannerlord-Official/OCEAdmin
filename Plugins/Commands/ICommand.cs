using System;
using System.Collections.Generic;
using System.Text;
using TaleWorlds.MountAndBlade;
using OCEAdmin.Core;

namespace OCEAdmin.Plugins.Commands
{
    public interface ICommand
    {
        string Command();
        Role CanUse();
        CommandFeedback Execute(NetworkCommunicator networkPeer, string[] args);

        string Description();
    }
}
