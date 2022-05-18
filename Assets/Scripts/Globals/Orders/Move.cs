using GameEngine;
using UnityEngine.AI;

namespace Globals.Orders
{
    public class Move : Order
    {
        private NavMeshAgent agent;
        private float moveSpeed;
        private float turnSpeed;
        public float MoveSpeed { get { return moveSpeed; } set { moveSpeed = value; agent.speed = value; } }
        public float TurnSpeed { get { return turnSpeed; } set { turnSpeed = value; agent.angularSpeed = value; } }
        public void Start()
        {
            IsObjectTargeted = false;
            agent = GetComponent<NavMeshAgent>();
            unit = GetComponent<Widget>();
        }
        public override void Issue(OrderTarget target)
        {
            completed = false;
            agent.isStopped = false;
            agent.destination = target.Location;
            agent.stoppingDistance = unit.Size * 2;
            agent.radius = unit.Size;
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
}