using Assets.Scripts.Globals.Commands;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.GameEngine;
using UnityEngine;

namespace Assets.Scripts.Globals.Abilities
{
    [CreateAssetMenu(fileName = "New Attacker", menuName = "Fire Attacker", order = 53)]
    public class FireAttacker : AreaAttacker
    {
        [SerializeField]
        private LayerMask mask;
        public FireAttacker() : base() { }
        public override bool OnIssue(Target target,  Unit caster)
        {

            Unit targetUnit = (target as UnitTarget).Value;
            Vector3 targetPos = targetUnit.Transform.position;
            if (IsInRange(targetPos, caster))
            {
                PlayEffects(caster);
                Vector3 casterPos = caster.Transform.position;
                caster.Transform.LookAt(targetPos);
                Collider[] discard = Physics.OverlapCapsule(casterPos, casterPos + Vector3.up * 100, Range.x, mask.value);
                Collider[] all = Physics.OverlapCapsule(casterPos, casterPos + Vector3.up * 100, Range.y, mask.value);
                HashSet<Collider> colliders = new(all);
                colliders.ExceptWith(discard);
                foreach (Collider collider in colliders)
                {
                    Widget unit = collider.GetComponent<Widget>();
                    if (unit == (Widget)caster)
                    {
                        continue;
                    }
                    Vector3 unitPos = unit.transform.position;
                    unitPos.y = casterPos.y;

                    Vector3 unitDirection = unitPos - casterPos;

                    float angle = Vector3.SignedAngle(unitDirection, caster.Transform.forward, Vector3.up);
                    if (angle >= -AreaAngle / 2 && angle <= AreaAngle / 2)
                    {
                        bool outofrange = !IsInRange(unitPos, caster);
                        if (!outofrange)
                        {
                            DealDamage(caster, unit);
                        }
                    }
                }
                return true;
            }
            return false;
        }
    }
}
