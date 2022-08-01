using Assets.Scripts.Globals.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Globals.Abilities
{
    public class Attacker : AbilityObject
    {
        public float Damage;
        public Attacker() : base()
        {
            CommandType = typeof(Attack);
            TargetType = typeof(UnitTarget);
        }
        public virtual Damage DealDamage(Unit dealer, Unit target)
        {
            return new Damage(dealer, target, Damage);
        }
    }
}
