using Assets.Scripts.GameEngine;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Globals.Commands
{
    public class MoveFree : Move
    {
        public override void Issue(Target target)
        {
            Issue(target as LocationTarget);
        }
        public override void Issue(LocationTarget target)
        {
            Issuing = false;
            Completed = true;
            agent.isStopped = false;
            agent.destination = target.Value;
            agent.stoppingDistance = Caster.Size * 2;
            agent.radius = Caster.Size;
        }
        public override void Abort()
        {
            agent.isStopped = true;
            Completed = false;
            Issuing = false;
        }
        public override string ToCommandString()
        {
            return $"Move freely to {agent.destination}";
        }
    }
}