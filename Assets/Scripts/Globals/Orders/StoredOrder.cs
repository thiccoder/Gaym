using System;
using UnityEngine;

namespace Globals.Orders
{
    internal struct StoredOrder
    {
        public readonly OrderTarget Target;
        public readonly Type OrderType;
        public StoredOrder(Type orderType, OrderTarget target)
        {
            OrderType = orderType;
            Target = target;
        }
        public Order Issue(GameObject obj)
        {
            var orders = obj.GetComponents<Order>();
            foreach (var order in orders)
            {
                if (order.GetType() == OrderType)
                {
                    order.Issue(Target);
                    return order;
                }
            }
            return null;
        }
    }
}
