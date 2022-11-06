using NetworkMessages.FromServer;
using System;
using TaleWorlds.MountAndBlade;


namespace OCEAdmin.Commands
{
    class Reset : Command
    {
        public bool CanUse(NetworkCommunicator networkPeer)
        {
            bool isAdmin = false;
            bool isExists = AdminManager.Admins.TryGetValue(networkPeer.VirtualPlayer.Id.ToString(), out isAdmin);
            return isExists && isAdmin;
        }

        public string Command()
        {
            return "!reset";
        }

        public string Description()
        {
            return "Resets the current mission. !reset";
        }

        public bool Execute(NetworkCommunicator networkPeer, string[] args)
        {
            AdminPanel.Instance.BroadcastMessage("Resetting the map.");

            MPUtil.BroadcastToAdmins(string.Format("** Command ** {0} has reset the map.", networkPeer.UserName));

            AdminPanel.Instance.ResetMission();

            return true;
        }
    }
}