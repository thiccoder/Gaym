using Assets.Scripts.GameEngine;
using Assets.Scripts.Globals.Commands;
using System;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.SocialPlatforms;

namespace Assets.Scripts.Globals.Abilities
{
    public class CommandObject : ActionObject
    {
        public Type CommandType;
        public Type TargetType;
        public float HealthCost;
        public float StaminaCost;
        public Vector2 Range;
        public float Delay;
        protected bool IsInRange(Vector3 loc,Unit caster) 
        {
            Vector3 casterpos = caster.Transform.position;
            return ((loc - casterpos).sqrMagnitude >= Range.x * Range.x) && ((loc - casterpos).sqrMagnitude <= Range.y * Range.y);
        }
    }
}
