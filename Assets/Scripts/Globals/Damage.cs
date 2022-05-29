using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Globals
{
    public struct Damage
    {
        public Unit Dealer;
        public Unit Target;
        public float Amount;
        public Damage(Unit dealer,Unit target,float amount) 
        {
            Dealer = dealer;
            Target = target;    
            Amount = amount;
            Target.OnDamage(this);
        }
    }
}
