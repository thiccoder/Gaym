using Globals;
using UnityEngine;
using System.Collections.Generic;

public class Stop : Order
{
    
    private OrderQueue Queue;
    public void Start()
    {
        IsObjectTargeted = false;
        Queue = GetComponent<OrderQueue>();
    }
    public override void Invoke(OrderTarget target)
    {
        completed = Queue.Abort();
    }
    public override void Abort()
    {

    }
}