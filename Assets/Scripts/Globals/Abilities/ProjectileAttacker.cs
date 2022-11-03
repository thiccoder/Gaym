using Assets.Scripts.GameEngine;
using Assets.Scripts.Globals.Commands;
using System;
using UnityEngine;

namespace Assets.Scripts.Globals.Abilities
{
    [CreateAssetMenu(fileName = "New Attacker", menuName = "Projectile Attacker", order = 51)]
    public class ProjectileAttacker : Attacker
    {
        public GameObject ProjectilePrefab;
        public ProjectileAttacker() : base()
        {
            CommandType = typeof(Attack);
            TargetType = typeof(UnitTarget);
        }
        public override void OnIssue(Target target, Unit caster)
        {
            Projectile projectile = Instantiate(ProjectilePrefab, caster.Transform.position, caster.Transform.rotation).GetComponent<Projectile>();
            projectile.TargetPosition = (target as UnitTarget).Value.Transform.position;
            projectile.attackObject = this;
            projectile.Dealer = caster;
            projectile.Target = (target as UnitTarget).Value;
        }
    }
}
