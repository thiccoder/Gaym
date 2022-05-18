using UnityEngine;

namespace Globals.Orders
{
    public struct OrderTarget
    {
        public readonly OrderTargetType Type;
        public readonly Vector3 Location;
        public readonly GameObject Object;
        public OrderTarget(GameObject obj)
        {
            Object = obj;
            Location = Vector3.zero;
            Type = OrderTargetType.Object;
        }
        public OrderTarget(Vector3 loc)
        {
            Object = default;
            Location = loc;
            Type = OrderTargetType.Location;
        }
        public OrderTarget(OrderTargetType type = OrderTargetType.None)
        {
            Object = default;
            Location = Vector3.zero;
            Type = type;
        }
    }
}
