using Assets.Scripts.GameEngine;

namespace Assets.Scripts.Globals.Orders
{
    public class Stop : Order
    {
        public override void Issue(Target target)
        {
            Issuing = false;
            Completed = true;
        }
        public override void Abort()
        {
            Issuing = false;
            Completed = false;
        }
        public override string ToOrderString()
        {
            return "Stop";
        }
    }
}