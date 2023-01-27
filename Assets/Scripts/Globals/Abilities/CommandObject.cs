using System;
using UnityEngine;

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
    }
}
