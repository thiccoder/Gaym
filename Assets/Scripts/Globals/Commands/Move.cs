using Assets.Scripts.GameEngine;
using Assets.Scripts.Globals.Abilities;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Globals.Commands
{
    public class Move : Command
    {
        [SerializeField]
        protected NavMeshAgent agent;
        [SerializeField]
        protected Mover Mover;
        public override void Issue(Target target)
        {
            Issue(target as LocationTarget);
        }
        public virtual void Issue(LocationTarget target)
        {
            Issuing = true;
            Completed = false;
            Mover.OnIssue(target, Caster);
        }
        public override void Abort()
        {
            Mover.OnAbort(Caster);
            Completed = true;
            Issuing = false;
        }
        private void Update()
        {
            if (Issuing)
            {
                Mover.OnUpdate(Caster);
                float distanceToTargetSqr = Vector3.SqrMagnitude(transform.position - agent.destination);
                if (distanceToTargetSqr < agent.stoppingDistance * agent.stoppingDistance)
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