using Assets.Scripts.GameEngine;
using Assets.Scripts.Globals.Commands;
using UnityEngine.AI;
using UnityEngine;

namespace Assets.Scripts.Globals.Abilities
{
    [CreateAssetMenu(fileName = "New Mover", menuName = "Soldier Mover", order = 53)]
    public class SoldierMover : Mover
    {
        public SoldierMover() : base()
        {
            CommandType = typeof(Move);
            TargetType = typeof(LocationTarget);
        }
        public override void OnIssue(Target target,Unit caster) 
        {
            NavMeshAgent agent = ((Widget)caster).GetComponent<NavMeshAgent>();
            agent.isStopped = false;
            agent.stoppingDistance = ((Widget)caster).Size * 2;
            agent.radius = ((Widget)caster).Size;
            agent.destination = (target as LocationTarget).Value;
        }
        public override void OnAbort(Unit caster)
        {
            NavMeshAgent agent = ((Widget)caster).GetComponent<NavMeshAgent>();
            agent.isStopped = true;
        }
    }
}