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
        [SerializeField] private string tag;
        public FireAttacker() : base() { }

        public override void OnIssue(Target target,  Unit caster)
        {
            foreach (GameObject unit in GameObject.FindGameObjectsWithTag("Enemy")) // ищем объекты по тегу. Думаю, можно изменить метод поиска.
            {
                Widget newUnit = unit.GetComponent<Widget>();
                
                Vector3 targetPos = newUnit.transform.position;
                targetPos.y = caster.Transform.position.y;

                Vector3 unitDirection = targetPos - caster.Transform.position;

                float angle = Vector3.SignedAngle(unitDirection, caster.Transform.forward, Vector3.up);

                float distance = Vector3.Distance(unit.transform.position, caster.Transform.position);

                if (distance <= AreaDistance && angle >= -AreaAngle && angle <= AreaAngle)
                {
                    Debug.Log($"{newUnit} {angle} {distance} attacked!");
                    DealDamage(caster, newUnit);
                }
            }
        }
    }
}
