using System;
using UnityEngine;

namespace Globals
{
    public struct OrderTarget
    {
        public readonly bool IsObject;
        public readonly Vector3 Position;
        public readonly GameObject Object;
        public OrderTarget(Vector3 pos)
        {
            Object = null;
            Position = pos;
            IsObject = false;
        }
        public OrderTarget(GameObject obj)
        {
            Object = obj;
            Position = Vector3.zero;
            IsObject = true;
        }
    }
    internal struct StoredOrder
    {   
        public readonly OrderTarget Target;
        public readonly Type OrderType;
        public StoredOrder(Type orderType,OrderTarget target)
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
        public enum VisionType : byte 
    {
        None = 0,
        Revealed,
        Visible
    }
    public abstract class Order : MonoBehaviour
    {
        [HideInInspector]
        public bool IsObjectTargeted;
        [HideInInspector]
        public bool completed = false;
        public abstract void Issue(OrderTarget target);
        public abstract void Abort();
    }
    
}
