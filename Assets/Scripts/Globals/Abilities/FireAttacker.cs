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
        public FireAttacker() : base() { }
        public override void OnIssue(Target target,  Unit caster)
        {
            PlayEffects(caster);
            caster.Transform.LookAt((target as UnitTarget).Value.Transform.position);
            foreach (Widget unit in FindObjectsOfType<Widget>())
            {
                if (unit == (Widget)caster) 
                { 
                    continue;
                }
                Vector3 targetPos = unit.transform.position;
                targetPos.y = caster.Transform.position.y;

                Vector3 unitDirection = targetPos - caster.Transform.position;

                float angle = Vector3.SignedAngle(unitDirection, caster.Transform.forward, Vector3.up);
                if (angle >= -AreaAngle / 2 && angle <= AreaAngle / 2)
                {
                    float distance = Vector3.Distance(unit.transform.position, caster.Transform.position);

                    if (distance <= Range.y)
                    {
                        DealDamage(caster, unit);
                    }
                }
            }
        }
    }
}
