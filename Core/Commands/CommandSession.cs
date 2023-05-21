using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade;

namespace OCEAdmin.Commands
{
    public class CommandSession
    {
        // The command that this is attached to.
        public ICommand command { get; set; }

        // When this command session was executed.
        public DateTime timeExecuted { get; set; }

        // The player that started this command session.
        public NetworkCommunicator executor { get; set; }

        // Any peers that are attached to this command session
        // as a result from the executed command.
        public List<NetworkCommunicator> peers { get; set; }

        public List<string> GenerateSelectionString()
        {
            var selection = new List<string>
            {
                $"** Command - {command.Command()} has found {peers.Count} targets."
            };

            int i = 1;

            foreach(NetworkCommunicator peer in peers)
            {
                selection.Add($"[{i}] " + peer.GetUsername());
                i++;
            }

            selection.Add($"Retype {command.Command()} 1-4 in the next 15s to confirm the target.");

            return selection;
        }

        public bool IsValidAnswer(string answer)
        {
            int number;
            if (int.TryParse(answer, out number))
            {
                return number >= 1 && number <= 4;
            }
            return false;
        }

        public NetworkCommunicator GetPeerAtSelectedIndex(int index)
        {
            return peers[index-1];
        }
    }
}
