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
        public override bool OnIssue(Target target, Unit caster)
        {
            Unit targetUnit = (target as UnitTarget).Value;
            Vector3 targetPos = targetUnit.Transform.position;
            if (IsInRange(targetPos, caster))
            {
                PlayEffects(caster);
                Projectile projectile = Instantiate(ProjectilePrefab, caster.Transform.position, caster.Transform.rotation).GetComponent<Projectile>();
                projectile.TargetPosition = targetPos;
                projectile.attacker = this;
                projectile.Dealer = caster;            
                projectile.Target = targetUnit;
                return true;
            }
            return false;
        }
    }
}
