using Assets.Scripts.Globals.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Globals.Abilities
{
    [CreateAssetMenu(fileName = "New AbilityObject", menuName = "Ability Object", order = 51)]
    public class AbilityObject : ScriptableObject
    {
        public Type OrderType;
        public Type TargetType;
        public float HealthCost;
        public float StaminaCost;
        public virtual void Cast(Target target) { }
    }
}
