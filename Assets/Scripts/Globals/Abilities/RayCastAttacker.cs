using Assets.Scripts.Globals.Commands;
using System;
using UnityEngine;

namespace Assets.Scripts.Globals.Abilities
{
    [CreateAssetMenu(fileName = "New Attacker", menuName = "RayCast Attacker", order = 52)]
    public class RayCastAttacker : Attacker
    {
        [SerializeField]
        private LayerMask mask;
        public RayCastAttacker() : base()
        {
            CommandType = typeof(Attack);
            TargetType = typeof(UnitTarget);
        }
        public override bool OnIssue(Target target, Unit caster)
        {
            Unit targetUnit = (target as UnitTarget).Value;
            Vector3 targetPos = targetUnit.Transform.position;
            if (IsInRange(targetPos, caster))
            {
                Vector3 casterPos = caster.Transform.position;
                PlayEffects(caster);
                caster.Transform.LookAt(targetPos);
                Vector3 dir = Vector3.Normalize(targetPos- casterPos);
                bool tooClose = Physics.Raycast(casterPos, dir, Range.x,mask.value);
                bool inRange = Physics.Raycast(casterPos + dir * Range.x, dir, Range.y,mask.value);
                if (!tooClose && inRange)
                {
                    DealDamage(caster, targetUnit);
                    return true;
                }
            }
            return false;
        }
    }
}
