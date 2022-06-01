using Assets.Scripts.Globals.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Globals.Abilities
{
    public class AbilityObject : ScriptableObject
    {
        public Type CommandType;
        public Type TargetType;
        public float HealthCost;
        public float StaminaCost;
        public Vector2 Range;
        public float Delay;
        public virtual void Cast(Target target, Unit caster)
        {

        }
    }
}
