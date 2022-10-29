using NetworkMessages.FromServer;
using System;
using TaleWorlds.MountAndBlade;


namespace OCEAdmin.Commands
{

    class Id : Command
    {
        public bool CanUse(NetworkCommunicator networkPeer)
        {
            return true;
        }

        public string Command()
        {
            return "!id";
        }

        public string Description()
        {
            return "Returns your unique ID into the chatbox.";
        }

        public bool Execute(NetworkCommunicator networkPeer, string[] args)
        {
            GameNetwork.BeginModuleEventAsServer(networkPeer);
            GameNetwork.WriteMessage(new ServerMessage(networkPeer.PlayerConnectionInfo.PlayerID.ToString()));
            GameNetwork.EndModuleEventAsServer();

            return true;
        }
    }
}