using Assets.Scripts.Globals.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Globals.Abilities
{

    [CreateAssetMenu(fileName = "New AttackObject", menuName = "RayCast Attack Object", order = 53)]
    public class RayCastAttackObject : AttackObject
    {
        public RayCastAttackObject() : base()
        {
            HealthCost = 0;
            StaminaCost = 0;
            CommandType = typeof(Attack);
            TargetType = typeof(UnitTarget);
        }
        public override void Cast(Target target, Unit caster)
        {
            if (Physics.Raycast(caster.Transform.position, caster.Transform.forward, out RaycastHit hitInfo, Range.y))
            {
                Destroy(hitInfo.collider.gameObject);
            }
        }
    }
}
