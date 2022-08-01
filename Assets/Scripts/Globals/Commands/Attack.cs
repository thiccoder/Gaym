using Assets.Scripts.GameEngine;
using Assets.Scripts.Globals.Abilities;
using UnityEngine;

namespace Assets.Scripts.Globals.Commands
{
    public class Attack : Command
    {
        public Attacker attackObject;
        private UnitTarget target;
        private float time;
        public void Start()
        {
            Range = attackObject.Range;
        }
        public override void Abort()
        {
            Issuing = false;
            Completed = false;
        }
        public override void Issue(Target target)
        {
            Issue(target as UnitTarget);
        }
        public void Issue(UnitTarget targ)
        {
            target = targ;
            Issuing = true;
            Completed = false;
            time = 0;
        }
        public void Update()
        {
            if (Issuing)
            {
                if (target.Value.Alive)
                {
                    if (time >= attackObject.Delay)
                    {
                        attackObject.OnIssue(target, (Unit)Caster);
                        time = 0;
                    }
                }
                else
                {
                    Completed = true;
                    Issuing = false;
                }
                time += Time.deltaTime;
            }
        }
        public override string ToCommandString()
        {
            return $"Attack {target.Value}";
        }
    }
}