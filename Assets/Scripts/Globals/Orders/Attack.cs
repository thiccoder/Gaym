using Assets.Scripts.GameEngine;
using Assets.Scripts.Globals.Abilities;

namespace Assets.Scripts.Globals.Orders
{
    public class Attack : Order
    {
        public AttackObject attackObject;
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
        public void Issue(UnitTarget target)
        {
            attackObject.AttackPos = transform;
            attackObject.Cast(target);
            Issuing = false;
            Completed = true;
        }
    }
}