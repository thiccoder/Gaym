using Assets.Scripts.Globals.Commands;
using UnityEngine;

namespace Assets.Scripts.Globals.Abilities
{
    public class ActionObject : ScriptableObject
    {
        public virtual void OnIssue(Target target, Unit caster)
        {

        }
        public virtual void OnAbort(Unit caster) 
        {
            
        }

    }
}
