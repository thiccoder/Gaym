using Assets.Scripts.Globals.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Globals.Abilities
{

    [CreateAssetMenu(fileName = "New AttackObject", menuName = "Attack Object", order = 52)]
    public class AttackObject : AbilityObject
    {
        public Transform AttackPos;
        public Vector2 Range;
        public AttackObject() : base()
        {
            HealthCost = 0;
            StaminaCost = 0;
            OrderType = typeof(Attack);
            TargetType = typeof(UnitTarget);
        }
    }
}
