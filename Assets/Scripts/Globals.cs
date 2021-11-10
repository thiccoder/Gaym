using System;
using UnityEngine;

namespace Globals
{
    public static class Functions
    {
        public static int FourCC(string id)
        {
            int res = 0;
            for (int i = 0; i < 4; i++)
            {
                res += (id[i] + 1) * (i + 1);
            }
            return res;
        }
    }
    public struct OrderTarget
    {
        public string Type;
        public Vector3 Point;
        public GameObject Object;
        public OrderTarget(Vector3 Point)
        {
            Object = null;
            this.Point = Point;
            Type = "Tloc";
        }
        public OrderTarget(GameObject Object)
        {
            this.Object = Object;
            Point = Vector3.zero;
            Type = "Tobj";
        }
        public dynamic Get()
        {
            if (Type == "Tloc")
            {
                return Point;
            }
            else if (Type == "Tobj")
            {
                return Object;
            }
            else
            {
                return null;
            }
        }
    }
    public struct StoredOrder
    {
        public string id;
        public OrderTarget Target;
        public StoredOrder(string id,OrderTarget Target)
        {
            this.id = id;
            this.Target = Target;
        }
        public Order Issue(GameObject obj)
        {
            var orders = obj.GetComponents<Order>();
            foreach (var order in orders)
            {
                if (order.id == id)
                {
                    order.Issue(Target);
                    return order;
                }
            }
            return null;
        }
    }
    public enum TargetType
    { 
        Point = 0,
        Object = 1
    }
    public class Node
    {
        public Node(Vector2Int position, float traverseDist, float heuristicDist, Node parent)
        {
            Position = position;
            TraverseDistance = traverseDist;
            Parent = parent;
            EstimatedTotalCost = TraverseDistance + heuristicDist;
        }

        public Vector2Int Position { get; }
        public float TraverseDistance { get; }
        public float EstimatedTotalCost { get; }
        public Node Parent { get; }
    }
    [Flags]
    public enum TerrainType : byte
    {
        All = 0,
        NonWalkable = 1,
        NonFlyable = 2,
        NonFloatable = 4,
        NonBuildable = 8,
        None = NonWalkable | NonFlyable | NonFloatable | NonBuildable,
        Walkable = None - NonWalkable,
        Flyable = None - NonFlyable,
        Floatable = None - NonFloatable,
        Buildable = None - NonBuildable,
    }
    public class Order : MonoBehaviour
    {
        [HideInInspector]
        public string id;
        [HideInInspector]
        public TargetType targetType;
        [HideInInspector]
        public bool completed;
        [HideInInspector]
        public char hotKey = default;
        protected Stats stats;
        public virtual void Issue(OrderTarget target) { }
        public virtual void Stop() { }
    }
}
