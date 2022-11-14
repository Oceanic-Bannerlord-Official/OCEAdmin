using NetworkMessages.FromServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using OCEAdmin.Core;

namespace OCEAdmin.Commands
{
    class Kill : Command
    {
        public Permissions CanUse() => Permissions.Admin;

        public string Command() => "!kill";

        public string Description() => "Kills a provided username. Usage !kill <player name>";

        public CommandFeedback Execute(NetworkCommunicator networkPeer, string[] args)
        {
            if (args.Length == 0)
            {
                return new CommandFeedback(CommandLogType.Player, msg: "Please provide a username.",
                    peer: networkPeer);
            }

            NetworkCommunicator targetPeer = MPUtil.GetPeerFromName(string.Join(" ", args));

            if (targetPeer == null)
            {
                return new CommandFeedback(CommandLogType.Player, msg: "Target player was not found!",
                    peer: networkPeer);
            }

            if (!targetPeer.ControlledAgent.Equals(null)) {
                Agent agent = targetPeer.ControlledAgent;
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
                agent.RegisterBlow(blow, attackCollisionDataForDebugPurpose);

                return new CommandFeedback(CommandLogType.BroadcastToAdminsAndTarget, 
                    msg: string.Format("** Command ** {0} has slayed {1}.", networkPeer.UserName, targetPeer.UserName),
                    targetMsg: "** Command ** You have been slain.", targetPeer: targetPeer);
            }

            return new CommandFeedback(CommandLogType.Player, msg: "Target is not alive!", peer: networkPeer);
        }
    }
}
