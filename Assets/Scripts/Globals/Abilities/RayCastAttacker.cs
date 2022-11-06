using Assets.Scripts.Globals.Commands;
using System;
using UnityEngine;

namespace Assets.Scripts.Globals.Abilities
{
    [CreateAssetMenu(fileName = "New Attacker", menuName = "RayCast Attacker", order = 52)]
    public class RayCastAttacker : Attacker
    {
        public RayCastAttacker() : base()
        {
            CommandType = typeof(Attack);
            TargetType = typeof(UnitTarget);
        }
        public override void OnIssue(Target target, Unit caster)
        {
            if (Physics.Raycast(caster.Transform.position, Vector3.Normalize((target as UnitTarget).Value.Transform.position - caster.Transform.position), Range.y))
            {
                DealDamage(caster, (target as UnitTarget).Value);
            }
        }
    }
}
