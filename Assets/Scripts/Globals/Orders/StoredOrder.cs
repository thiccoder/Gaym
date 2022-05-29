using System;
using UnityEngine;

namespace Assets.Scripts.Globals.Orders
{
    internal struct StoredOrder
    {
        public readonly Target Target;
        public readonly Type OrderType;
        public Order GetOrder(GameObject obj)
        {
            return (Order)obj.GetComponent(OrderType.Name);
        }
        public StoredOrder(Type orderType, Target target)
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
