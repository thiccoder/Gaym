using Assets.Scripts.GameEngine;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Globals.Commands
{
    public class Move : Command
    {
        [SerializeField]
        protected NavMeshAgent agent;
        [SerializeField]
        protected float _moveSpeed;
        [SerializeField]
        protected float _turnSpeed;
        public float MoveSpeed { get { return _moveSpeed; } set { _moveSpeed = value; agent.speed = value; } }
        public float TurnSpeed { get { return _turnSpeed; } set { _turnSpeed = value; agent.angularSpeed = value; } }
        private void Start()
        {
            agent.stoppingDistance = Caster.Size * 2;
            agent.radius = Caster.Size;
            agent.updateUpAxis = true;
        }
        public override void Issue(Target target)
        {
            Issue(target as LocationTarget);
        }
        public virtual void Issue(LocationTarget target)
        {
            Issuing = true;
            Completed = false;
            agent.isStopped = false;
            agent.destination = target.Value;
            agent.stoppingDistance = Caster.Size * 2;
            agent.radius = Caster.Size;
        }
        public override void Abort()
        {
            agent.isStopped = true;
            Completed = true;
            Issuing = false;
        }
        private void Update()
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
        public override string ToCommandString()
        {
            return $"Move to {agent.destination}";
        }
    }
}