using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade;

namespace OCEAdmin.Commands
{
    public class PeerSearchCommand : ICommand
    {
        public virtual Permissions CanUse() => Permissions.Admin;

        public virtual string Command() => null;

        public virtual string Description() => null;       

        public virtual CommandFeedback OnSearchValidation(NetworkCommunicator networkPeer, string[] args) 
        {
            if (args.Length == 0)
            {
                return new CommandFeedback(CommandLogType.Player, msg: "Please provide an input.",
                    peer: networkPeer);
            }

            return null;
        }

        public CommandFeedback Execute(NetworkCommunicator networkPeer, string[] args)
        {
            // We need to give the new command some agency on it's own
            // search validation, so reroute before we pass with the
            // peer search validation.
            CommandFeedback searchValidation = OnSearchValidation(networkPeer, args);

            // If we validated unsuccessfully, we don't need to continue.
            if (searchValidation != null)
            {
                return searchValidation;
            }

            CommandSession session = CommandManager.Instance.GetCommandSession(networkPeer, this);
            List<NetworkCommunicator> peers = new List<NetworkCommunicator>();
            bool search = true;
            string input = string.Join(" ", args);

            // If the session exists, we want to check if there
            // input matches an expected integer within the
            // session table.
            if (session != null)
            {
                if (session.IsValidAnswer(input))
                {
                    // We now know that we have an integer.
                    int index = int.Parse(input);
                    if (index < 0 || index > session.peers.Count)
                    {
                        // Extend the session.
                        session.timeExecuted = DateTime.Now;

                        return new CommandFeedback(CommandLogType.Player, msg: $"Number out of range. Choose a number between 0 and {session.peers.Count}.",
                        peer: networkPeer);
                    }

                    peers.Add(session.GetPeerAtSelectedIndex(int.Parse(input)));

                    // We've got a user-selected peer from the search
                    // index table. Don't run the standard search.
                    search = false;
                }
            }

            // If we have no session to grab a peer from, we're
            // going to need to grab all the peers from the input.
            if (search)
            {
                peers = MPUtil.GetPeersFromName(input);
            }
            if (peers.Count < 1)
            {
                return new CommandFeedback(CommandLogType.Player, msg: "No target player was found!",
                    peer: networkPeer);
            }
            else if (peers.Count > 1 && peers.Count <= 4)
            {
                session = CommandManager.Instance.CreateCommandSession(this, networkPeer, peers);

                return new CommandFeedback(CommandLogType.Player, msgs: session.GenerateSelectionString(),
                    peer: networkPeer);
            }
            else if (peers.Count > 4)
            {
                return new CommandFeedback(CommandLogType.Player, msg: "More than 4 players with your search criteria were found. Try narrowing it down.",
                    peer: networkPeer);
            }

            // If there was a session, we need
            // to consume it.
            if (session != null)
            {
                CommandManager.Instance.commandSessions.Remove(session);
            }

            // Reroute the old command implementation
            // to a new method of the command.
            return OnRunAction(networkPeer, peers.First());
        }

        public virtual CommandFeedback OnRunAction(NetworkCommunicator networkPeer, NetworkCommunicator foundPeer) { return null; }
    }
}
