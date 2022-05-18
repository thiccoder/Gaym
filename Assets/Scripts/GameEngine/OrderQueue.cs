using System.Collections.Generic;
using UnityEngine;
using Globals.Orders;

namespace GameEngine
{
    internal class OrderQueue : MonoBehaviour
    {
        private readonly Queue<StoredOrder> Orders = new();
        private Order current = null;
        public void Update()
        {
            if (Orders.Count != 0 && (current is null || current.completed))
            {
                Issue();
            }

        }
        public void Add(StoredOrder order)
        {
            Orders.Enqueue(order);
        }
        public Order Issue()
        {
            current = Orders.Dequeue().Issue(gameObject);
            return current;
        }
        public Order IssueImmediate(StoredOrder order)
        {
            Clear();
            Add(order);
            return Issue();
        }
        public bool Abort()
        {
            if (current is not null)
            {
                current.Abort();
            }
            current = null;
            return current is null;
        }
        public bool Clear()
        {
            Orders.Clear();
            if (current is not null)
            {
                current.Abort();
            }
            current = null;
            return Orders.Count == 0 && current is null;
        }
    }
}