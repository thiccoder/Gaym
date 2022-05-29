using Assets.Scripts.GameEngine;
using Assets.Scripts.Globals.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Globals.Abilities
{

    [CreateAssetMenu(fileName = "New AttackObject", menuName = "Projectile Attack Object", order = 54)]
    public class ProjectileAttackObject : AttackObject
    {
        public GameObject ProjectilePrefab;
        [HideInInspector]
        public Projectile Projectile;
        public ProjectileAttackObject() : base()
        {
            HealthCost = 0;
            StaminaCost = 0;
            OrderType = typeof(Attack);
            TargetType = typeof(UnitTarget);
        }
        public override void Cast(Target target, Unit caster)
        {
            Projectile = Instantiate(ProjectilePrefab, caster.Transform.position, caster.Transform.rotation).GetComponent<Projectile>();
            Projectile.TargetPosition = (target as UnitTarget).Value.Transform.position;
            Projectile.attackObject = this;
            Projectile.Dealer = caster;
            Projectile.Target = (target as UnitTarget).Value;
        }
    }
}
