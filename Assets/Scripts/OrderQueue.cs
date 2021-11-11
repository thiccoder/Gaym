using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Globals;

public class OrderQueue : MonoBehaviour
{
    private Queue<StoredOrder> Orders = new Queue<StoredOrder>();
    private Order current = null;

    // Update is called once per frame
    public void Update()
    {
        if ( Orders.Count != 0 && (current == null || current.completed)) 
        {
            Issue();
        }

    }
    public void Add(StoredOrder ord) 
    {
        Orders.Enqueue(ord);
    }
    public void Issue()
    {
        current = Orders.Dequeue().Issue(gameObject);
    }
    public void Clear()
    {
        Orders.Clear();
        if (current != null)
        {
            current.Stop();
        }
        current = null;
    }
}
