using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Assets.Scripts.Globals.Orders;
using System;
using System.Linq.Expressions;
using System.Collections;

namespace Assets.Scripts.GameEngine
{
    internal class OrderQueue : MonoBehaviour
    {
        private Queue<StoredOrder> orders = new();
        private Order current;
        public int Count => orders.Count;
        public void Start()
        {
            current = GetComponent<Stop>();
            current.Issue(Target.Null);
        }
        public void Update()
        {
            if ((current is not null) && (!current.Issuing))
            {
                print(current);
                current = null;
            }
            if ((current is null) && (Count != 0))
            {
                Issue();
                print(current);
            }
        }
        public void Add(StoredOrder order)
        {
            orders.Enqueue(order);
        }
        public Order Issue()
        {
            if (Count == 0) throw new InvalidOperationException("Sequence contains no elements");
            current = orders.Dequeue().Issue(gameObject);
            return current;
        }
        public Order IssueImmediate(StoredOrder order)
        {
            Clear();
            Add(order);
            return Issue();
        }
        public void Abort()
        {
            if (current is not null && current.CanAbort)
            {
                current.Abort();
                current = null;
            }
        }
        public void Clear()
        {
            Queue<StoredOrder> newOrders = new(orders.Where(x => !x.GetOrder(gameObject).CanAbort));
            orders.Clear();
            orders = newOrders;
            Abort();
        }
    }
}