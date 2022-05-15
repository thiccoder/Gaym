using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Globals;

public class OrderQueue : MonoBehaviour
{
    private readonly Queue<StoredOrder> Orders = new();
    private Order current = null;
    public void Update()
    {
        if (Orders.Count != 0 && (current is null || current.completed))
        {
            Invoke();
        }

    }
    public void Add(StoredOrder order)
    {
        Orders.Enqueue(order);
    }
    public bool Invoke()
    {
        current = Orders.Dequeue().Issue(gameObject);
        return current is not null;
    }
    public bool InvokeInstant(StoredOrder order) 
    {
        Clear();
        Add(order);
        return Invoke();
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
