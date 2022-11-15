using NetworkMessages.FromServer;
using OCEAdmin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace OCEAdmin
{
    public class MPUtil
    {
        private static Random random = new Random();

        public static string GetPluginDir()
        {
            return "../../OCEAdmin";
        }

        public static void WriteToConsole(string text) {
            Debug.Print(string.Format("[OCEAdmin] - " + text), 0, Debug.DebugColor.Green);
        }

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static void SendChatMessage(NetworkCommunicator networkPeer, string text) {
            GameNetwork.BeginModuleEventAsServer(networkPeer);
            GameNetwork.WriteMessage(new ServerMessage(text));
            GameNetwork.EndModuleEventAsServer();
        }

        public static List<NetworkCommunicator> GetAdmins() {
            var admins = new List<NetworkCommunicator>();
            foreach (NetworkCommunicator peer in GameNetwork.NetworkPeers)
            {
                bool isAdmin = false;
                bool isExists = AdminManager.Admins.TryGetValue(peer.VirtualPlayer.Id.ToString(), out isAdmin);

                if (isAdmin && isExists)
                {
                    if (peer.IsSynchronized)
                    {
                        admins.Add(peer);
                    }
                }
            }

            return admins;
        }

        public static NetworkCommunicator GetPeerFromName(string name)
        {
            NetworkCommunicator targetPeer = null;

            foreach (NetworkCommunicator peer in GameNetwork.NetworkPeers)
            {
                if (peer.UserName.ToLower().Contains(name.ToLower()))
                {
                    targetPeer = peer;
                    break;
                }
            }

            return targetPeer;
        }

        public static bool IsAdmin(NetworkCommunicator networkPeer)
        {
            bool isAdmin = false;
            bool isExists = AdminManager.Admins.TryGetValue(networkPeer.VirtualPlayer.Id.ToString(), out isAdmin);

            return isAdmin && isExists & networkPeer.IsSynchronized;
        }

        public static void BroadcastToAdmins(string text)
        {
            foreach(NetworkCommunicator admin in GetAdmins())
            {
                SendChatMessage(admin, text);
            }
        }

        public static void BroadcastToAll(string text)
        {
            GameNetwork.BeginBroadcastModuleEvent();
            GameNetwork.WriteMessage(new ServerMessage(text));
            GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None);
        }

        public static string GetClanTag(NetworkCommunicator networkPeer)
        {
            string username = networkPeer.VirtualPlayer.UserName;
            return username.Substring(username.IndexOf("[") + 1, username.IndexOf("]") - 1);
        }

        public static bool IsInClan(NetworkCommunicator networkPeer)
        {
            if (networkPeer.VirtualPlayer.UserName.Substring(0, 1) == "[")
            {
                return true;
            }

            return false;
        }


        public static void Slay(NetworkCommunicator networkPeer)
        {
            Slay(networkPeer.ControlledAgent);
        }

        public static void Slay(Agent agent)
        {
            if (!agent.Equals(null)) {
                Blow blow = new Blow(agent.Index);
                blow.DamageType = TaleWorlds.Core.DamageTypes.Pierce;
                blow.BoneIndex = agent.Monster.HeadLookDirectionBoneIndex;
                blow.Position = agent.Position;
                blow.Position.z = blow.Position.z + agent.GetEyeGlobalHeight();
                blow.BaseMagnitude = 2000f;
                blow.WeaponRecord.FillAsMeleeBlow(null, null, -1, -1);
                blow.InflictedDamage = 2000;
                blow.SwingDirection = agent.LookDirection;
                MatrixFrame frame = agent.Frame;
                blow.SwingDirection = frame.rotation.TransformToParent(new Vec3(-1f, 0f, 0f, -1f));
                blow.SwingDirection.Normalize();
                blow.Direction = blow.SwingDirection;
                blow.DamageCalculated = true;
                sbyte mainHandItemBoneIndex = agent.Monster.MainHandItemBoneIndex;
                AttackCollisionData attackCollisionDataForDebugPurpose = AttackCollisionData.GetAttackCollisionDataForDebugPurpose(false, false, false, true, false, false, false, false, false, false, false, false, CombatCollisionResult.StrikeAgent, -1, 0, 2, blow.BoneIndex, BoneBodyPartType.Head, mainHandItemBoneIndex, Agent.UsageDirection.AttackLeft, -1, CombatHitResultFlags.NormalHit, 0.5f, 1f, 0f, 0f, 0f, 0f, 0f, 0f, Vec3.Up, blow.Direction, blow.Position, Vec3.Zero, Vec3.Zero, agent.Velocity, Vec3.Up);

                Agent agentHorse = agent.MountAgent;

                if (!agentHorse.Equals(null))
                {
                    agentHorse.RegisterBlow(blow, attackCollisionDataForDebugPurpose);
                }

                agent.RegisterBlow(blow, attackCollisionDataForDebugPurpose);
            }
        }

        public static string GetPlayerID(NetworkCommunicator networkPeer)
        {
            return networkPeer.VirtualPlayer.Id.ToString();
        }

        public static string GetUnitID(NetworkCommunicator networkPeer)
        {
            return networkPeer.ControlledAgent.Character.StringId;
        }
    }
}
