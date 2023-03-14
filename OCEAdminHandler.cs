﻿using NetworkMessages.FromServer;
using OCEAdmin.Commands;
using OCEAdmin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.DedicatedCustomServer;
using TaleWorlds.MountAndBlade.Network.Messages;
using TaleWorlds.PlayerServices;

namespace OCEAdmin
{
    class OCEAdminHandler : GameHandler
    {
        public override void OnAfterSave() {}

        public override void OnBeforeSave() { }

        protected override void OnGameNetworkBegin()
        {
            this.AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegisterer.RegisterMode.Add);
        }

        protected override void OnPlayerConnect(VirtualPlayer virtualPlayer) {
            NetworkCommunicator peer = (NetworkCommunicator) virtualPlayer.Communicator;

            if (peer.IsBanned()) {
                DedicatedCustomServerSubModule.Instance.DedicatedCustomGameServer.KickPlayer(virtualPlayer.Id, false);
            }
        }

        protected override void OnGameNetworkEnd()
        {
            base.OnGameNetworkEnd();
            this.AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegisterer.RegisterMode.Remove);
        }

        private void AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegisterer.RegisterMode mode)
        {
            GameNetwork.NetworkMessageHandlerRegisterer networkMessageHandlerRegisterer = new GameNetwork.NetworkMessageHandlerRegisterer(mode);
            if (GameNetwork.IsServer) {
                networkMessageHandlerRegisterer.Register<NetworkMessages.FromClient.PlayerMessageAll>(new GameNetworkMessage.ClientMessageHandlerDelegate<NetworkMessages.FromClient.PlayerMessageAll>(this.HandleClientEventPlayerMessageAll));
            }
        }

        private bool HandleClientEventPlayerMessageAll(NetworkCommunicator networkPeer, NetworkMessages.FromClient.PlayerMessageAll message)
        {
            if (message.Message.StartsWith("!")) {
                string[] argsWithCommand = message.Message.Split(' ');
                string command = argsWithCommand[0];
                string[] args = argsWithCommand.Skip(1).ToArray();
                CommandManager.Instance.Execute(networkPeer, command, args).Log();
            }
            return true;
        }
    }
}
