using Globals;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class Move : Order
{
    private NavMeshAgent agent;
    private float moveSpeed;
    private float turnSpeed;
    public float MoveSpeed { get { return moveSpeed; } set { moveSpeed = value;agent.speed = value; } }
    public float TurnSpeed { get { return turnSpeed; } set { turnSpeed = value; agent.angularSpeed = value; } }
    public float Size;
    public void Start()
    {
        IsObjectTargeted = false;
        agent = GetComponent<NavMeshAgent>();
    }
    public override void Invoke(OrderTarget target)
    {
        completed = false;
        agent.isStopped = false;
        agent.destination = target.Position;
        agent.stoppingDistance = Size * 2;
        agent.radius = Size;
    }
    public override void Abort()
    {
        agent.isStopped = true;
        completed = true;
    }
    public void Update()
    {
        completed = agent.isStopped;
    }
}