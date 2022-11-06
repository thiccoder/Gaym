using Assets.Scripts.Globals.Commands;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Globals.Abilities
{
    public class AreaAttacker : Attacker
    {
        [SerializeField]
        protected float AreaAngle;
        public AreaAttacker() : base()
        {
            CommandType = typeof(Attack);
            TargetType = typeof(UnitTarget);
        }

        public override void OnIssue(Target target, Unit caster)
        {

        }
    }
}
