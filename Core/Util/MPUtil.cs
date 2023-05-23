using NetworkMessages.FromServer;
using OCEAdmin.Core;
using OCEAdmin.Core.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.Diamond;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.ObjectSystem;
using static TaleWorlds.MountAndBlade.MultiplayerClassDivisions;

namespace OCEAdmin
{
    public class MPUtil
    {
        private static Random random = new Random();
        public static string GetPluginDir()
        {
            return "../../Modules/OCEAdmin/data";
        }

        public static string GetPluginDirNoData()
        {
            return "../../Modules/OCEAdmin";
        }

        public static string GetSteamID(NetworkCommunicator networkPeer)
        {
            return networkPeer.VirtualPlayer.Id.Id2.ToString();
        }

        public static void WriteToConsole(string text, bool log = false) {
            string msg = string.Format("[OCEAdmin] - " + text);

            Debug.Print(msg, 0, Debug.DebugColor.Green);

            if (log)
            {
                LogManager.Add(msg);
            }
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

        public static NetworkCommunicator GetPeerFromName(string name)
        {
            NetworkCommunicator targetPeer = null;

            foreach (NetworkCommunicator peer in GameNetwork.NetworkPeers)
            {
                if (peer.IsSynchronized)
                {
                    if (peer.GetUsername().ToLower().Contains(name.ToLower()))
                    {
                        return peer;
                    }
                }
            }

            return targetPeer;
        }

        public static NetworkCommunicator GetPeerFromID(string id)
        {
            NetworkCommunicator targetPeer = null;

            foreach (NetworkCommunicator peer in GameNetwork.NetworkPeers)
            {
                if (peer.IsSynchronized)
                {
                    if (peer.VirtualPlayer.Id.ToString().Contains(id))
                    {
                        return peer;
                    }
                }
            }

            return targetPeer;
        }

        public static List<NetworkCommunicator> GetPeersFromName(string name)
        {
            List<NetworkCommunicator> targetPeers = new List<NetworkCommunicator>();
            foreach (NetworkCommunicator peer in GameNetwork.NetworkPeers)
            {
                if (peer.IsSynchronized)
                {
                    if (peer.GetUsername().ToLower().Contains(name.ToLower()))
                    {
                        targetPeers.Add(peer);
                    }
                }
            }

            return targetPeers;
        }

        public static bool IsPermitted(NetworkCommunicator networkPeer, Role role)
        {
            return (networkPeer.GetPlayer().role >= role) & networkPeer.IsSynchronized;
        }

        public static void BroadcastToAdmins(string text)
        {
            foreach(NetworkCommunicator peer in GameNetwork.NetworkPeers)
            {
                Player player = peer.GetPlayer();

                if (player.HasPermission(Role.Admin))
                {
                    SendChatMessage(peer, text);
                }
            }
        }

        public static Tuple<TeamComposition, TeamComposition> GetTeamsComposition() 
        {
            Tuple<TeamComposition, TeamComposition> teams = new Tuple<TeamComposition, TeamComposition>(
                new TeamComposition(GetMapCulture(true)),
                new TeamComposition(GetMapCulture(false))
            );

            foreach (NetworkCommunicator peer in GameNetwork.NetworkPeers)
            {
                if (peer.ControlledAgent != null)
                {
                    // team 1 = attacker
                    CompositionType compositionType = CompositionType.Infantry;

                    if (peer.IsArcher())
                    {
                        compositionType = CompositionType.Archer;
                    }
                    else if (peer.IsCavalry())
                    {
                        compositionType = CompositionType.Cavalry;
                    }

                    TeamComposition team = peer.ControlledAgent.Team.IsAttacker ? teams.Item1 : teams.Item2;

                    team.AddPlayerToCollection(compositionType, peer);
                }
            }

            return teams;
        }

        public static void BroadcastToAll(string text)
        {
            GameNetwork.BeginBroadcastModuleEvent();
            GameNetwork.WriteMessage(new ServerMessage(text));
            GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None);
        }

        public static void Slay(NetworkCommunicator networkPeer)
        {
            Slay(networkPeer.ControlledAgent);
        }

        public static void Slay(Agent agent)
        {
            if (agent != null) {
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

                if (agentHorse != null)
                {
                    agentHorse.RegisterBlow(blow, attackCollisionDataForDebugPurpose);
                }

                agent.RegisterBlow(blow, attackCollisionDataForDebugPurpose);
            }
        }

        public static Team GetDefenderTeam()
        {
            return Mission.Current.Teams.Defender;
        }

        public static Team GetAttackerTeam()
        {
            return Mission.Current.Teams.Attacker;
        }

        public static BasicCultureObject GetTeamCulture(NetworkCommunicator networkPeer)
        {
            MissionPeer component = networkPeer.GetComponent<MissionPeer>();

            if (((component != null) ? component.Team : null) != null && component.Team.Side != BattleSideEnum.None)
            {
                string strValue = MultiplayerOptions.OptionType.CultureTeam1.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
                if (component.Team.Side != BattleSideEnum.Attacker)
                {
                    strValue = MultiplayerOptions.OptionType.CultureTeam2.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
                }

                return MBObjectManager.Instance.GetObject<BasicCultureObject>(strValue);
            }

            return null;
        }

        public static BasicCultureObject GetMapCulture(bool isAttacker)
        {
            string strValue = MultiplayerOptions.OptionType.CultureTeam1.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
            if (!isAttacker)
            {
                strValue = MultiplayerOptions.OptionType.CultureTeam2.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
            }

            return MBObjectManager.Instance.GetObject<BasicCultureObject>(strValue);
        }

        public static Team GetPeerTeam(NetworkCommunicator networkPeer)
        {
            if(networkPeer.GetComponent<MissionPeer>() != null)
                return networkPeer.GetComponent<MissionPeer>().Team;

            return null;
        }

        public static string GetPlayerID(NetworkCommunicator networkPeer)
        {
            return networkPeer.VirtualPlayer.Id.ToString();
        }

        public static string GetUnitIDFromIndex(NetworkCommunicator peer)
        {
            BasicCultureObject culture = MPUtil.GetTeamCulture(peer);
            int index = peer.GetComponent<MissionPeer>().SelectedTroopIndex;

            List<MultiplayerClassDivisions.MPHeroClass> classes = MultiplayerClassDivisions.GetMPHeroClasses(culture).ToList<MultiplayerClassDivisions.MPHeroClass>();

            for (int i = 0; i < classes.Count; i++)
            {
                if (index == i)
                {
                    return classes[i].StringId;
                }
            }

            return null;
        }

        public static string GetUnitID(NetworkCommunicator networkPeer)
        {
            if (networkPeer.ControlledAgent == null)
                return null;

            return networkPeer.ControlledAgent.Character.StringId;
        }

        public static TargetIconType[] cavClasses = new TargetIconType[] { TargetIconType.Cavalry_Heavy, TargetIconType.Cavalry_Light,
            TargetIconType.HorseArcher_Heavy, TargetIconType.HorseArcher_Light };

        public static TargetIconType[] archerClasses = new TargetIconType[] { TargetIconType.Archer_Heavy, TargetIconType.Archer_Light, 
            TargetIconType.Crossbowman_Heavy, TargetIconType.Crossbowman_Light };
    }
}
