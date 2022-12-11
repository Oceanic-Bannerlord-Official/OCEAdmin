using System;
using System.Collections.Generic;
using System.Text;
using TaleWorlds.MountAndBlade;
using OCEAdmin.Core;

namespace OCEAdmin.Commands
{
    interface Command
    {
        string Command();
        Permissions CanUse();

        CommandFeedback Execute(NetworkCommunicator networkPeer, string[] args);

        string Description();
    }

    public enum Permissions
    {
        Player,
        Admin
    }
}
