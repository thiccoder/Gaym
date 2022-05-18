using UnityEngine;

namespace Globals.Orders
{
    public abstract class Order : MonoBehaviour
    {
        [HideInInspector]
        public bool IsObjectTargeted;
        [HideInInspector]
        public bool completed = false;
        [HideInInspector]
        public GameEngine.Widget unit;
        public abstract void Issue(OrderTarget target);
        public abstract void Abort();
    }
}
