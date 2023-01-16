using OCEAdmin.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace OCEAdmin
{
    public class RoleComponent
    {
        private static List<RoleComponent> All = new List<RoleComponent>();

        public static RoleComponent GetFor(NetworkCommunicator networkPeer)
        {
            return All.Find(x => x.GetNetworkPeer() == networkPeer);
        }

        public static void RemoveAt(NetworkCommunicator networkPeer)
        {
            All.Remove(GetFor(networkPeer));
        }

        public static RoleComponent Create(NetworkCommunicator networkPeer)
        {
            RoleComponent component = new RoleComponent(networkPeer);
            All.Add(component);

            return component;
        }

        private NetworkCommunicator networkPeer { get; set; }

        public RoleComponent(NetworkCommunicator networkPeer)
        {
            this.networkPeer = networkPeer;
        }

        public NetworkCommunicator GetNetworkPeer()
        {
            return networkPeer;
        }

        // The current role that the player
        // will have attached to their peer.
        public Role role;
        
        // The current command session that this player
        // is running through.
        private CommandSession session;

        public List<CommandSession> commandSessions = new List<CommandSession>();

        public const float commandSessionTimeOut = 15f;

        public bool HasPermission(Role role)
        {
            return this.role >= role;
        }

        public CommandSession GetCommandSession(ICommand command)
        {
            if(session.command.Command() == command.Command())
            {
                TimeSpan diff = DateTime.Now - session.timeExecuted;

                if (diff.TotalSeconds >= commandSessionTimeOut)
                {
                    commandSessions.Remove(session);

                    return null;
                }
                else
                {
                    return session;
                }
            }

            return null;
        }

        public bool HasCommandSession()
        {
            return session != null;
        }

        public void UpdateRole(Role role)
        {
            this.role = role;
        }

        public void ConsumeCommandSession()
        {
            session = null;
        }

        public CommandSession CreateCommandSession(ICommand command, List<NetworkCommunicator> peersResult)
        {
            CommandSession session = new CommandSession()
            {
                command = command,
                executor = this.GetNetworkPeer(),
                peers = peersResult,
                timeExecuted = DateTime.Now
            };

            this.session = session;

            return session;
        }
    }
}
