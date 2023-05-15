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
    public class Player
    {
        private static List<Player> All = new List<Player>();

        public static Player GetFor(NetworkCommunicator networkPeer)
        {
            return All.Find(x => x.GetNetworkPeer() == networkPeer);
        }

        public static void RemoveAt(NetworkCommunicator networkPeer)
        {
            All.Remove(GetFor(networkPeer));
        }

        public static Player Create(NetworkCommunicator networkPeer)
        {
            Player component = new Player(networkPeer);
            All.Add(component);

            return component;
        }

        private NetworkCommunicator networkPeer { get; set; }

        public Player(NetworkCommunicator networkPeer)
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

        private bool _muted;

        public const float commandSessionTimeOut = 15f;

        public bool HasPermission(Role role)
        {
            return this.role >= role;
        }

        public bool IsMuted()
        {
            return _muted;
        }

        public void Mute()
        {
            _muted = true;
        }

        public void Unmute()
        {
            _muted = false;
        }

        public CommandSession GetCommandSession(ICommand command)
        {
            if (session == null)
                return null;

            if(session.command.Command() == command.Command())
            {
                TimeSpan diff = DateTime.Now - session.timeExecuted;

                if (diff.TotalSeconds >= commandSessionTimeOut)
                {
                    session = null;

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
