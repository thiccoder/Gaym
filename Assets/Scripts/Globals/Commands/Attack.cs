using Assets.Scripts.GameEngine;
using Assets.Scripts.Globals.Abilities;
using System;
using UnityEngine;

namespace Assets.Scripts.Globals.Commands
{
    public class Attack : Command
    {
        public Attacker attacker;
        private UnitTarget target;
        private float time;
        public void Start()
        {
            Range = attacker.Range;
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
                if (target != null && target.Value.Alive)
                {
                    if (time >= attacker.Delay)
                    {
                        time = 0;
                    }
                    if (time < float.Epsilon) 
                    {
                        if (!attacker.OnIssue(target, (Unit)Caster)) 
                        {
                            Issuing = false;
                            Completed= false;
                        }
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