using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade;

namespace OCEAdmin.Commands
{
    public class CommandFeedback
    {
        public CommandLogType commandLogType;

        public NetworkCommunicator peer;
        public NetworkCommunicator targetPeer;

        public string msg;
        public List<string> msgs;
        public string targetMsg;

        public void Log()
        {
            switch (commandLogType)
            {
                case CommandLogType.Broadcast:
                    if (msg != null)
                    {
                        MPUtil.BroadcastToAll(msg);
                    }
                    break;
                case CommandLogType.BroadcastToAdmins:
                    if(msg != null)
                    {
                        MPUtil.BroadcastToAdmins(msg);
                    }
                    break;
                case CommandLogType.BroadcastToAdminsAndTarget:
                    if(msg != null && targetMsg != null && targetPeer != null)
                    {
                        MPUtil.BroadcastToAdmins(msg);
                        MPUtil.SendChatMessage(targetPeer, targetMsg);
                    }
                    break;
                case CommandLogType.Player:
                    if(msg != null && peer != null)
                    {
                        if(msgs != null)
                        {
                            foreach(string listMsg in msgs)
                            {
                                MPUtil.SendChatMessage(peer, listMsg);
                            }
                        }
                        else
                        {
                            MPUtil.SendChatMessage(peer, msg);
                        }
                    }
                    break;
                case CommandLogType.Both:
                    if (msg != null && peer != null && targetMsg != null && targetPeer != null)
                    {
                        MPUtil.SendChatMessage(peer, msg);
                        MPUtil.SendChatMessage(targetPeer, targetMsg);
                    }
                    break;
            }
        }

        // CommandLogType
        public CommandFeedback(CommandLogType commandLogType = CommandLogType.Player, string msg = null, string targetMsg = null, NetworkCommunicator peer = null, NetworkCommunicator targetPeer = null)
        {
            this.commandLogType = commandLogType;
            this.msg = msg;
            this.targetMsg = targetMsg;
            this.peer = peer;
            this.targetPeer = targetPeer;
        }
    }

    public enum CommandLogType
    {
        None,
        Broadcast,
        BroadcastToAdmins,
        BroadcastToAdminsAndTarget,
        Player,
        Both
    }
}
