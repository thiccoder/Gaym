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
    public struct StoredOrder
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
                    order.Invoke(Target);
                    return order;
                }
            }
            return null;
        }
    }
    public struct StoredTexture<T> where T : Texture
    {
        public readonly Texture Tex;
        public readonly bool IsPathTexture;
        public bool Active;
        public Vector2Int Position;
        public StoredTexture(Texture tex, Vector2Int position,bool active = true)
        {
            Tex = tex;
            Position = position;
            IsPathTexture = tex.GetType() == typeof(PathTexture);
            Active = active;
        }
    }
    [Flags]
    public enum TerrainType : short
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
        public abstract void Invoke(OrderTarget target);
        public abstract void Abort();
    }
    public struct Node
    {
        public Node(Vector2Int position, float traverseDist, float heuristicDist)
        {
            Position = position;
            TraverseDistance = traverseDist;
            EstimatedTotalCost = TraverseDistance + heuristicDist;
        }

        public Vector2Int Position;
        public float TraverseDistance;
        public float EstimatedTotalCost;
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode() 
        {
            return Position.GetHashCode()+123134;
        }
        public static bool operator ==(Node left, Node right)
        {
            return left.Equals(right);
        }
        public static bool operator !=(Node left, Node right)
        {
            return !(left==right);
        }
    }
}
