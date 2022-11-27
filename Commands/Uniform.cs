using NetworkMessages.FromServer;
using System;
using TaleWorlds.MountAndBlade;


namespace OCEAdmin.Commands
{

    class Uniform : Command
    {
        public Permissions CanUse() => Permissions.Player;
        public string Command() => "!uniform";

        public string Description() => "Allows you to disable your uniform.";

        public CommandFeedback Execute(NetworkCommunicator networkPeer, string[] args)
        {
            string playerID = networkPeer.VirtualPlayer.Id.ToString();

            if (UniformManager.Instance.usingUniform.ContainsKey(playerID))
            {
                bool val;
                    
                if(UniformManager.Instance.usingUniform.TryGetValue(playerID, out val))
                {
                    UniformManager.Instance.usingUniform.Remove(playerID);
                    UniformManager.Instance.usingUniform.Add(playerID, !val);

                    return new CommandFeedback(CommandLogType.Player, msg: "** Command ** Your clan uniforms have been set to: " + val.ToString(),
                        peer: networkPeer);
                }
            }
            else
            {
                UniformManager.Instance.usingUniform.Add(playerID, false);

                return new CommandFeedback(CommandLogType.Player, msg: "** Command ** You have disabled clan uniforms.",
                    peer: networkPeer);
            }

            return new CommandFeedback(CommandLogType.Player, msg: "** Command ** - Unexpected error.",
                peer: networkPeer);
        }
    }
}