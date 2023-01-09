using HarmonyLib;
using Newtonsoft.Json;
using OCEAdmin.Core;
using OCEAdmin.Features.Nicknames;
using SMExtended.API;
using SMExtended.API.Endpoints;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace OCEAdmin.Patches
{
    class PatchAddNetworkPeer
    {
        public static bool Prefix(NetworkCommunicator networkPeer)
        {
            string steamID = MPUtil.GetSteamID(networkPeer);
            EndPoint endpoint = new GetNicknameEndPoint();

            endpoint.SetArgs(new List<Tuple<string, string>> {
                Tuple.Create("steam", steamID),
                Tuple.Create("key", ConfigManager.Instance.GetConfig().APIKey)
            });

            endpoint.Request();

            if (endpoint.response.data != null)
            {
                Identity identity = JsonConvert.DeserializeObject<Identity>(endpoint.response.data);

                string nickname = identity.nickname;

                // Add it to the nickname storage.
                // We need to store the nicknames because we don't want
                // to call the API everytime we sync a client.
                NicknameManager.Instance.AddToStorage(steamID, nickname);

                Traverse.Create(networkPeer.VirtualPlayer).Property("UserName").SetValue(nickname);
            }

            MBNetwork.NetworkPeers.Add(networkPeer);

            return false;
        }

    }
}