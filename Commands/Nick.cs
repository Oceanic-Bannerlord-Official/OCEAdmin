﻿using NetworkMessages.FromServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using OCEAdmin.Core;
using SMExtended.API;
using SMExtended.API.Endpoints;
using Newtonsoft.Json;

namespace OCEAdmin.Commands
{
    class Nick : ICommand
    {
        public Permissions CanUse() => Permissions.Player;

        public string Command() => "!Nick";

        public string Description() => "Changes your nickname when you rejoin the server. !nick <string [35] nickname>";

        // We don't want to give the client the abillity to overwhelm
        // the API server. Rate limit them.
        private static Dictionary<string, DateTime> lastExecutionTimes = new Dictionary<string, DateTime>();

        public CommandFeedback Execute(NetworkCommunicator networkPeer, string[] args)
        {
            if (args.Length == 0)
            {
                return new CommandFeedback(CommandLogType.Player, msg: "Please provide a nickname.",
                    peer: networkPeer);
            }

            if (args.Length >= 35)
            {
                return new CommandFeedback(CommandLogType.Player, msg: "Nickname must be less than 35 characters.",
                    peer: networkPeer);
            }

            string id = networkPeer.VirtualPlayer.Id.Id2.ToString();

            // Check if the command has been executed within the past 30 seconds
            // for this particular networkPeer
            if (lastExecutionTimes.ContainsKey(id) && DateTime.Now - lastExecutionTimes[id] < TimeSpan.FromSeconds(30))
            {
                return new CommandFeedback(CommandLogType.Player, msg: "This command can only be executed once every 30 seconds.",
                    peer: networkPeer);
            }

            // Update the last execution time for this networkPeer
            lastExecutionTimes[id] = DateTime.Now;

            string input = string.Join(" ", args);

            UpdateNickname(networkPeer, input);

            return new CommandFeedback(CommandLogType.Player, msg: "Nickname is updating. Rejoin to have your name changed.", peer: networkPeer);
        }

        public void UpdateNickname(NetworkCommunicator networkPeer, string name)
        {
            string steamID = MPUtil.GetSteamID(networkPeer);
            EndPoint endpoint = new SetNicknameEndPoint();

            // Applying the API key to the args will allow a server to edit
            // usernames that don't belong to it, bypassing the need for 
            // a steam ticket.
            endpoint.SetArgs(new List<Tuple<string, string>> {
                Tuple.Create("steam", steamID),
                Tuple.Create("nick", name),
                Tuple.Create("key", ConfigManager.Instance.GetConfig().APIKey)
            });

            // Fire and forget. We don't actually need to
            // update anything on the server.
            endpoint.Request();          
        }
    }
}
