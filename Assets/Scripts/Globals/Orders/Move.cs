using Assets.Scripts.GameEngine;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Globals.Orders
{
    public class Move : Order
    {
        [SerializeField]
        private NavMeshAgent agent;
        [SerializeField]
        private float moveSpeed;
        [SerializeField]
        private float turnSpeed;
        public float MoveSpeed { get { return moveSpeed; } set { moveSpeed = value; agent.speed = value; } }
        public float TurnSpeed { get { return turnSpeed; } set { turnSpeed = value; agent.angularSpeed = value; } }
        public void Start()
        {
            agent.stoppingDistance = unit.Size * 2;
            agent.radius = unit.Size;
            agent.updateUpAxis = true;
        }
        public override void Issue(Target target)
        {
            Issue(target as LocationTarget);
        }
        public void Issue(LocationTarget target)
        {
            Issuing = true;
            Completed = false;
            Debug.Log(agent is null);
            agent.isStopped = false;
            agent.destination = target.Value;
            agent.stoppingDistance = unit.Size * 2;
            agent.radius = unit.Size;
        }
        public override void Abort()
        {
            agent.isStopped = true;
            Completed = true;
            Issuing = false;
        }
        public void Update()
        {
            if (Issuing)
            {
                float distanceToTarget = Vector3.SqrMagnitude(transform.position - agent.destination);
                if (distanceToTarget < agent.stoppingDistance * agent.stoppingDistance)
                {
                    Completed = true;
                    Issuing = false;
                }
            }
        }
        public override string ToOrderString()
        {
            return $"Move to {agent.destination}";
        }
    }
}