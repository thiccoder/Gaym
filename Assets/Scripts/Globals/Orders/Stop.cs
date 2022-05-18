using GameEngine;

namespace Globals.Orders
{
    public class Stop : Order
    {
        private OrderQueue Queue;
        public void Start()
        {
            IsObjectTargeted = false;
            Queue = GetComponent<OrderQueue>();
        }
        public override void Issue(OrderTarget target)
        {
            completed = Queue.Abort();
        }
        public override void Abort()
        {
        }
    }
}